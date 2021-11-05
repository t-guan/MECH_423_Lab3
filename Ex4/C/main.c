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
void processPWM(unsigned int data);

// other functions
void delayNOP(unsigned int cycles);

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

    while(1) {
        // FOR UART DEBUG
        UCA1TXBUF = 43; // periodically transmit 'a' to serial port
        while ((UCA1IFG & UCTXIFG) == 0);
        delayNOP(2000);
    }
}

void configureCS(void)
{
    // User Guide pg 80
    CSCTL0_H = 0xA5; // this is the password when writing in word mode

    // User Guide page 81-83
    CSCTL1 |= DCOFSEL0 + DCOFSEL1; // DCO setting -> configured to 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = ACLK = SMCLK = DCO
    //CSCTL2 = SELS_3; // SMCLK source is set to 011b = DCOCLK
    //CSCTL3 = DIVS_5; // SMCLK source divider is set to 101b -> divider of 32
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
    //P2OUT = 0;
    P2DIR  |= BIT1;
    P2SEL0 |= BIT1;
    P2SEL1 &= ~(BIT1);

    TB2CCR0 = 2000; // Timer overflow value
    TB2CCR1 = 250;
    TB2CCTL1 = OUTMOD_3;
    TB2CTL = TBSSEL_2 + MC_1; // SMCLK, UP mode
}

void configureMiscPins(void)
{
    // set P3.6, P3.7 to outputs that we toggle for AIN1, AIN2
    P3OUT = 0;
    P3DIR |= (BIT6 + BIT7);
    P3SEL0 &= ~(BIT6 + BIT7);
    P3SEL1 &= ~(BIT6 + BIT7);

    // force one direction at start
    P3OUT |= BIT6;
    P3OUT &= ~(BIT7);
}

void enableInterrupts(void)
{
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
        processPWM(fullData); // changes PWM
    }
    if (CmdByte == 1)
    {
        // change dir to ccw
        P3OUT |= BIT7;
        P3OUT &= ~(BIT6);
    }
    if (CmdByte == 2)
    {
        // change dir to cw
        P3OUT |= BIT6;
        P3OUT &= ~(BIT7);
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

void processPWM(unsigned int data)
{
    max = 65535;
    maxTimerVal = TB2CCR0;

    duty = (double)data / (double)max;
    newTimerVal = (double)maxTimerVal * duty;

    // change timer value to duty cycle
    TB2CCR1 = (unsigned int)newTimerVal;
}

void delayNOP(unsigned int cycles)
{
    for (i = 0; i < cycles; i++)
    {
        _NOP();
    }
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

// CONNECTIONS
// -----------
// see page 18 on Lab 3 manual, requires external Polulu motor driver

// PSEUDOCODE
// ----------
//  set up timer connected to P2.1 in continuous mode with 50% pwm to start
//  if (packet received from C# program)
//      extract pwm and dir information from it
//      change dir (turn on P3.7, turn off P3.6)
//      change pwm (on timer connected to P2.1)

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
//      0 = changePWM
//      1 = dir clockwise
//      2 = dir counterclockwise
// DataByte1 -> upper 8 bits of 16bit PWM value
// DataByte2 -> lower 8 bits of 16bit PWM value
// EscByte
//      0 = no PWM change
//      1 = DataByte1   -> 255
//      2 = DataByte2   -> 255
//      3 = DataByte1,2 -> 255
//
// NOTE ABOUT PWM DataBytes:
// 0% PWM at {DataByte1, DataByte2} = 0
// 100% PWM at {DataByte1, DataByte2} = 0xFFFF = 65535
