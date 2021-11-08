using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



namespace Mech423Lab3Ex5
{
    public partial class Form1 : Form
    {
        // ----- CONCURRENT QUEUES -----

        //Collect UART data
        ConcurrentQueue<Int32> databyte = new ConcurrentQueue<Int32>();

        //Encoder Count storage
        ConcurrentQueue<Int32> encoderUpCounts = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> encoderDownCounts = new ConcurrentQueue<Int32>();

        //Direction and PWM Bytes for DC motor Control
        ConcurrentQueue<Int32> DirectionByte = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> PWMByte = new ConcurrentQueue<Int32>();

        // ----- ADJUSTABLE GLOBAL VARIABLES -----
        int avg = 45; // for smoothing data (higher -> smoother, but updates less frequently)

        // ----- 'ACTIVE' GLOBAL VARIABLES -----
        int is255 = 0; // state variable for packets
        int x = 0; // increments for plotting data
        double position = 0.0; // current gantry position
        int prevsliderpos = 999; // records prev slider position.. used to prevent sending too many packets
        int dirval = 0; // sent to packet
        int pwmval = 0; // sent to packet, also used for plotting

        // ----- MISC GLOBAL VARIABLES -----
        int halfticks; // 1/2 of slider ticks - used to zero slider posiion @ middle
        int pwmscale; // scaling factor from slider ticks to pwm

        double circ = 0.523; // circumference for rotation/displacement conversion
        double timeDiff = 0.6; //The divisor for velocity

        // ----- SERIES FOR DATA PLOTTING -----
        Series posdata = new Series();
        Series veldata = new Series();
        Series pwmdata = new Series();

        // ----- FOR WRITINNG TO CSV -----
        string path = @"C:\Users\Thomas\MECH423Lab3\MECH-423-Lab3-\Ex5\VS\values.csv";
        string delim = ",";
        StringBuilder csvout = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Position Chart Init
            PosChart.Series.Add(posdata);
            posdata.Name = "Position Data";
            posdata.ChartType = SeriesChartType.Line;
            posdata.BorderWidth = 2;
            posdata.Color = Color.Purple;
            posdata.XValueType = ChartValueType.Double;
            posdata.YValueType = ChartValueType.Double;
            //Velocity Chart Init
            VelChart.Series.Add(veldata);
            veldata.Name = "Velocity Data";
            veldata.ChartType = SeriesChartType.Line;
            veldata.BorderWidth = 2;
            veldata.Color = Color.Purple;
            veldata.XValueType = ChartValueType.Double;
            veldata.YValueType = ChartValueType.Double;
            //PWM RPM Chart Init
            pwmrotchart.Series.Add(pwmdata);
            pwmdata.Name = "PWM VS Rotation";
            pwmdata.ChartType = SeriesChartType.Point;
            pwmdata.BorderWidth = 2;
            pwmdata.Color = Color.Purple;
            pwmdata.XValueType = ChartValueType.Int32;
            pwmdata.YValueType = ChartValueType.Double;
            //Timer Initialization
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
            timer2.Interval = 1000;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Enabled = true;
            //Serial Port Init
            serialPort1.PortName = "COM7";
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            //serialPort1.Open();
            //Variable Inits
            halfticks = SliCon.Maximum / 2;
            pwmscale = (int)((double)100 / halfticks);
        }

        private void ConBut_MouseClick(object sender, MouseEventArgs e)
        {
            serialPort1.Open();

/*            if (serialPort1.IsOpen == false)
            {
                serialPort1.Open();
                ConBut.Text = "Disconnect";
            }
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                ConBut.Text = "Connect";
            }*/
        }

        private void SumConverter(int upc, int doc)
        {
            // velocity, position calculations
            double velocityCPS = ((double)upc - (double)doc) / timeDiff;
            VelCountBox.Text = velocityCPS.ToString();
            double velocityRPM = (velocityCPS * 60.0 / (20.4 * 12.0));
            position = position + ((velocityRPM * 8 * 3.14) / 60) * timeDiff;
            //Store values into CSV
            csvout.AppendLine(x.ToString() + delim + position.ToString());
            File.WriteAllText(path, csvout.ToString());
            File.AppendAllText(path, csvout.ToString());
            // only plot 100 datapoints
            if (posdata.Points.Count() > 1000) posdata.Points.RemoveAt(0);
            if (veldata.Points.Count() > 1000) veldata.Points.RemoveAt(0);
            if (pwmdata.Points.Count() > 1000) pwmdata.Points.RemoveAt(0);

            // actual plotting
            posBox.Text = position.ToString();
            velBox.Text = velocityRPM.ToString();
            posdata.Points.AddXY(x, position);
            veldata.Points.AddXY(x, velocityRPM);   
            pwmdata.Points.AddXY(pwmval, velocityCPS);
            PosChart.ResetAutoValues();
            VelChart.ResetAutoValues();
            pwmrotchart.ResetAutoValues();
            x++;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = serialPort1.BytesToRead;
            while (bytesToRead != 0)
            {
                int newByte = serialPort1.ReadByte();
                databyte.Enqueue(newByte);
                bytesToRead = serialPort1.BytesToRead;
            }
        }

        //Only collects data from UART, calls SumConverter, currently collecting only 3 data bytes as opposed to 5
        //Can modify as required
        private void timer1_Tick(object sender, EventArgs e)
        {
            int counter = 0;
            int usum=0, dsum=0;

            if (serialPort1.IsOpen)
            {
                while (databyte.TryDequeue(out int valfromq))
                {
                    // state machine
                    switch (is255)
                    {
                        case 0:
                            if (valfromq == 255) { is255 = 1; }
                            break;
                        case 1:
                            encoderUpCounts.Enqueue(valfromq);
                            usum = valfromq;
                            textBox2.Text = valfromq.ToString();
                            is255 = 2;
                            break;
                        case 2:
                            encoderDownCounts.Enqueue(valfromq);
                            dsum = valfromq;
                            textBox3.Text = valfromq.ToString();
                            is255 = 0;
                            counter++; // increments here, so 1 counter increment == 1 full packet
                            break;
                    }
                    SumConverter(usum, dsum);

                    // once enough data points collected, send to SumConverter()
/*                    if (counter > avg)
                    {
                        usum = 0;
                        for (int i = 0; i < avg; i++)
                        {
                            if (encoderUpCounts.TryDequeue(out int value))
                            {
                                usum = usum + value;
                            }
                        }
                        usum = usum / avg;

                        dsum = 0;
                        for (int i = 0; i < avg; i++)
                        {
                            if (encoderDownCounts.TryDequeue(out int value))
                            {
                                dsum = dsum + value;
                            }
                        }
                        dsum = dsum / avg;

                        SumConverter(usum, dsum);
                        counter = 0; // reset counter
                    }*/
                }
            }
        }

        private void PreparePackets(int direction, int pwm)
        {
            if (direction == 0 || direction == 1 || direction == 2)
            {
                PrepareDirPacket(direction);
                PreparePWMPacket(pwm);
                if (DirectionByte.Count == 5 && PWMByte.Count == 5)
                {
                    SendPackets();
                }
                else
                {
                    DestroyPackets();
                }
            }
            else
            {
                MessageBox.Show("Invalid Direction, No Action Taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareDirPacket(int direction)
        {
            //Enqueue first byte
            DirectionByte.Enqueue(255);
            //Enqueue direction byte
            DirectionByte.Enqueue(direction);
            //Fill rest with Zeros
            for (int i = 0; i < 3; i++)
            {
                DirectionByte.Enqueue(0);
            }
        }

        private void DestroyPackets()
        {
            while (DirectionByte.Count > 0)
            {
                DirectionByte.TryDequeue(out _);
            }
            while (PWMByte.Count > 0)
            {
                PWMByte.TryDequeue(out _);
            }
        }

        private void PreparePWMPacket(int pwm)
        {
            if (pwm > 100)
            {
                pwm = 65535;
            }
            else if (pwm < 0)
            {
                pwm = 0;
            }
            else
            {
                pwm = (int)((double)pwm / 100 * 65535);
            }
            ushort pwmnum16 = Convert.ToUInt16(pwm);
            byte upperpwm = (byte)(pwmnum16 >> 8);
            byte lowerpwm = (byte)(pwmnum16 & 0xff);
            byte esc = 0;

            if (upperpwm == 255 && lowerpwm == 255)
            {
                upperpwm = 0;
                lowerpwm = 0;
                esc = 3;
            }
            else if (upperpwm == 255 && lowerpwm != 255)
            {
                upperpwm = 0;
                esc = 2;
            }
            else if (upperpwm != 255 && lowerpwm == 255)
            {
                lowerpwm = 0;
                esc = 1;
            }
            else
            {
                esc = 0;
            }

            //Enqueue First Byte
            PWMByte.Enqueue(255);
            //Enqeue 0 for direction, so the direction is not chagned
            PWMByte.Enqueue(0);
            //Enqueue the Upper Byte first
            PWMByte.Enqueue(upperpwm);
            //Enqueue the lower byte
            PWMByte.Enqueue(lowerpwm);
            // Enqueue escape byte
            PWMByte.Enqueue(esc);
        }

        private void SendPackets()
        {
            byte[] Dir_Byte = { 0, 0, 0, 0, 0 };
            byte[] PWM_Byte = { 0, 0, 0, 0, 0 };
            int value;
            for (int i = 0; i < 5; i++)
            {
                DirectionByte.TryDequeue(out value);
                Dir_Byte[i] = (byte)value;
                PWMByte.TryDequeue(out value);
                PWM_Byte[i] = (byte)value;
            }
            serialPort1.Write(Dir_Byte, 0, 5);
            serialPort1.Write(PWM_Byte, 0, 5);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int dirval, pwmval;
            if (SliConCheck.Checked)
            {
                int sliderpos = SliCon.Value;
                if (sliderpos == halfticks)
                {
                    dirval = 0;
                    pwmval = 0;
                }
                else if (sliderpos < halfticks)
                {
                    pwmval = (halfticks - sliderpos) * pwmscale;
                    dirval = 2;
                }
                else
                {
                    pwmval = (sliderpos - halfticks) * pwmscale;
                    dirval = 1;
                }

                //Send the read values to PreparePackets
                if ((sliderpos != prevsliderpos) && (prevsliderpos != 999))
                {
                    PreparePackets(dirval, pwmval);
                }
                else if (prevsliderpos == 999)
                {
                    PreparePackets(dirval, pwmval);
                }
                prevsliderpos = sliderpos;
            }
        }

        private void PWM_0_Button_MouseClick(object sender, MouseEventArgs e)
        {
            PreparePackets(0, 0);
        }

        private void PWM_25_Button_MouseClick(object sender, MouseEventArgs e)
        {
            PreparePackets(2, 25);
        }

        private void PWM_50_Button_MouseClick(object sender, MouseEventArgs e)
        {
            PreparePackets(2, 50);
        }

        private void PWM_100_Button_MouseClick(object sender, MouseEventArgs e)
        {
            PreparePackets(2, 99);
        }

        private void PWM_Custom_Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (Int32.TryParse(PWM_Custom_Box.Text, out int custompwm))
            {
                if ((custompwm >= 0) && (custompwm <= 100))
                {
                    PreparePackets(2, custompwm);
                }
                else
                {
                    MessageBox.Show("PWM outside of 0-100 range, No Action Taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid PWM input, No Action Taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
