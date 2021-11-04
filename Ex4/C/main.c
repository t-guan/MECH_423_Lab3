#include <msp430.h> 

// configuration functions
void configureCS(void);
void configureUART(void);
void configureTimer(void);
void configureMiscPins(void);
void enableInterrupts(void);

// packet processing related functions
void processPacket(void);
unsigned char dequeueBuffer(void);
void enqueueBuffer(unsigned char val);
unsigned int combineBytes(unsigned char lower, unsigned char upper);
void processSpeed(unsigned int data);

// other functions
void delayNOP(unsigned int cycles);
void advanceStep(void);

// variables
unsigned char buffer[50];
unsigned int ixFront = 0;
unsigned int ixBack = 0;
unsigned int buffSize = 0;
unsigned int packetIndex = 0;
unsigned char RxByte; // receive byte
unsigned char StartByte;
unsigned char CmdByte;
unsigned char LowerDataByte;
unsigned char UpperDataByte;
unsigned char EscByte;
unsigned int fullData;
unsigned int l;
unsigned int u;
unsigned int maxTimerVal;
unsigned int dutyTimerVal;
unsigned int max;
unsigned int temp;
unsigned int prevclk0;//Store Previous Clock Value
unsigned int currclk0;//Store Current Clock Value
unsigned int enccounts0; //Store the encoder count between clock values
unsigned int prevclk1;//Store previous clock value
unsigned int currclk1;//Store current clock value
unsigned int enccounts1;//Store the encoder count between clock values

double duty;
double newTimerVal;
int i;

int main(void)
{
    WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

    configureCS();
    configureUART();
    configureTimer();
    configureMiscPins();
    enableInterrupts();

    while(1) {}
}

void configureCS(void)
{
    // User Guide pg 80
    CSCTL0_H = 0xA5; // this is the password when writing in word mode

    // User Guide page 81-83
    CSCTL1 |= DCOFSEL0 + DCOFSEL1; // DCO setting -> configured to 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = ACLK = SMCLK = DCO
    //CSCTL2 = SELS_3; // SMCLK source is set to 011b = DCOCLK
    //CSCTL3 = DIVS_5; // SMCLK source divider is set to 101b -> divider of 32. DEFAULT IS DIVIDE BY 8
}

void configureUART(void)
{
    // Reference: 2021W-UART_example.c, FR5739 page 74
    // Configure P2.5 and P2.6 for UCA1
    P2SEL0 &= ~(BIT5 + BIT6);
    P2SEL1 |= BIT5 + BIT6;

    // Reference: 2021W-UART_example.c
    // Configure UCA1
    UCA1CTLW0 = UCSSEL0; // use ACLK as clock source, User Guide page 495
    UCA1CTLW0 &= ~(UCPEN); // disable parity (default), User Guide page 495

    // set baud rate to 9600 @ 8MHz, User Guide page 490
    UCA1BRW = 52; // UCBRx is 16bits so we can set the whole word // User Guide page 497
    UCA1MCTLW = 0x4900 + UCOS16 + UCBRF0; // 49 modifies UCBRSx, UCBRF0 is 0th bit of UCBRFx (= 1)
}

void configureTimer(void)
{
    // set P2.1 to TB2.1
    P2DIR  |= BIT1;
    P2SEL0 |= BIT1;
    P2SEL1 &= ~(BIT1);

    // set P1.4, P1.5 to TB0.1, TB0.2
    P1DIR  |= BIT4 + BIT5;
    P1SEL0 |= BIT4 + BIT5;
    P1SEL1 &= ~(BIT4 + BIT5);

    // set P3.4, P3.5 to TB1.1, TB1.2
    P3DIR  |= BIT4 + BIT5;
    P3SEL0 |= BIT4 + BIT5;
    P3SEL1 &= ~(BIT4 + BIT5);

    TB2CCR0 = 100000; // Timer overflow value
    TB2CCR1 = 75000;
    TB2CCTL1 = OUTMOD_3;
    TB2CTL = TBSSEL_2 + MC_1; // SMCLK, UP mode

    // Configure timers that are internally connected to stepper input pins
    TB0CCR0 = 2000;
    TB0CCR1 = 1500;
    TB0CCR2 = 1500;
    TB0CCTL1 = OUTMOD_3;
    TB0CCTL2 = OUTMOD_3;
    TB0CTL = TBSSEL_2 + MC_1;

    TB1CCR0 = 2000;
    TB1CCR1 = 1500;
    TB1CCR2 = 1500;
    TB1CCTL1 = OUTMOD_3;
    TB1CCTL2 = OUTMOD_3;
    TB1CTL = TBSSEL_2 + MC_1;
}

void configureMiscPins(void)
{
    // reserved
}

void enableInterrupts(void)
{
    TB2CCTL0 |= CCIE;
    UCA1IE |= UCRXIE; // enable Receive Interrupt, User Guide page 502
    _EINT(); // global interrupt enable
}

unsigned char dequeueBuffer(void)
{
    if (buffSize > 0)
    {
        unsigned char res;
        res = buffer[ixFront];
        ixFront = (ixFront+1) % 50;
        buffSize--;

        return res;
    }
    else
    {
        return 0;
    }
}

void enqueueBuffer(unsigned char val)
{
    buffer[ixBack] = val;
    ixBack = (ixBack+1) % 50;
    buffSize++;
}

void processPacket(void)
{
    StartByte = dequeueBuffer();
    CmdByte = dequeueBuffer();
    UpperDataByte = dequeueBuffer();
    LowerDataByte = dequeueBuffer();
    EscByte = dequeueBuffer();

    if (EscByte == 1)
    {
        LowerDataByte = 255;
    }
    else if (EscByte == 2)
    {
        UpperDataByte = 255;
    }
    else if (EscByte == 3)
    {
        LowerDataByte = 255;
        UpperDataByte = 255;
    }

    fullData = combineBytes(LowerDataByte, UpperDataByte);

    if (CmdByte == 0)
    {
        processSpeed(fullData);
    }
    if (CmdByte == 1)
    {
        TB2CTL |= MC_1;
        forwardStepDir = 1;
    }
    if (CmdByte == 2)
    {
        TB2CTL |= MC_1;
        forwardStepDir = 0;
    }
    if (CmdByte == 3)
    {
        TB2CTL &= ~(MC_1);
        forwardStepDir = 1;
        advanceStep();
    }
    if (CmdByte == 4)
    {
        TB2CTL &= ~(MC_1);
        forwardStepDir = 0;
        advanceStep();
    }
}

unsigned int combineBytes(unsigned char lower, unsigned char upper)
{
    unsigned int res;
    l = (unsigned int)lower;
    u = (unsigned int)upper << 8;
    res = l + u;
    return res;
}

void processSpeed(unsigned int data)
{
    if (data == 0)
    {
        TB2CTL &= ~(MC_1);
    }
    else
    {
        temp = 1000000 / data;
        TB2CCR0 = temp;
    }
}

void delayNOP(unsigned int cycles)
{
    for (i = 0; i < cycles; i++)
    {
        _NOP();
    }
}

void encoderCalc(void)
{

}

#pragma vector = USCI_A1_VECTOR
__interrupt void USCI_A1_ISR(void)
{
    RxByte = UCA1RXBUF; // transfer from buffer to memory

    if (packetIndex == 0)
    {
        if (RxByte == (unsigned char)255)
        {
            if (buffSize < 50)
            {
                enqueueBuffer(RxByte);
                packetIndex++;
            }
        }
    }
    else if (packetIndex == 4)
    {
        if (buffSize < 50)
        {
            enqueueBuffer(RxByte);
            processPacket();
            packetIndex = 0;
        }
    }
    else
    {
        if (buffSize < 50)
        {
            enqueueBuffer(RxByte);
            packetIndex++;
        }
    }
}

// TB2.0 ISR - CCIFG automatically reset when request is serviced
#pragma vector = TIMER2_B0_VECTOR
__interrupt void Timer_B2_0 (void)
{
    encoder0Calc();
    encoder1Clac();
}
//TB


// PACKET STRUCTURE
// ----------------
// Byte Order
// ----------
// [StartByte][CmdByte][DataByte1][DataByte2][EscByte]
//
// Byte Details
// ------------
// StartByte -> always 255
// CmdByte
//      0 = changeSpeed
//      1 = dir clockwise
//      2 = dir counterclockwise
// DataByte1 -> upper 8 bits of 16bit speed value
// DataByte2 -> lower 8 bits of 16bit speed value
// EscByte
//      0 = no DataByte change
//      1 = DataByte1   -> 255
//      2 = DataByte2   -> 255
//      3 = DataByte1,2 -> 255

// Slider Packet
//      direction packet: {255, 1 or 2, 0, 0, 0}
//      speed packet:     {255, 0, UpperSpeed, LowerSpeed, SpeedEsc}

// Single Step Packet
//      step packet:      {255, 3 or 4, 0, 0, 0}
