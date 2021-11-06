using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;

namespace Mech423Lab3Ex4
{
    public partial class Form1 : Form
    {
        ConcurrentQueue<double> Posvalues = new ConcurrentQueue<double>();
        ConcurrentQueue<double> Velvalues = new ConcurrentQueue<double>();
        ConcurrentQueue<Int32> databyte = new ConcurrentQueue<Int32>();
        //Encoder Count storage
        ConcurrentQueue<Int32> encoderUpCounts = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> encoderDownCounts = new ConcurrentQueue<Int32>();
        //Direction and PWM Bytes for DC motor Control
        ConcurrentQueue<Int32> DirectionByte = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> PWMByte = new ConcurrentQueue<Int32>();
        int x = 0;
        double pval = 0;
        double vval = 0;
        int freq = 1000;
        double circ = 0.523;
        private static int dirval;
        private static int pwmval;
        private static int sliderticks = 8;
        private int prevsliderpos = 999;
        int halfticks;
        int bytesToRead = 0;
        int is255 = 0;
        //The divisor for velocity
        double timeDiff = 1;
        Series posdata = new Series();
        Series veldata = new Series();
        Random rnd = new Random();
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
            //Timer Initialization
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(timer1_Tick);
            //timer1.Enabled = true;
            timer2.Interval = 100;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Enabled = true;
            timer3.Interval = 100;
            timer3.Tick += new EventHandler(timer3_Tick);
            //Serial Port Init
            serialPort1.PortName = "COM5";
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            //Variable Inits
            sliderticks = SliCon.Maximum;
            halfticks = sliderticks / 2;


        }
        private void ConBut_MouseClick(object sender, MouseEventArgs e)
        {
            serialPort1.Open();
        }
        private void UpdateSeries()
        {
            if (x < Posvalues.Count)
            {
                Posvalues.TryDequeue(out pval);
                Velvalues.TryDequeue(out vval);
                if (posdata.Points.Count() > 100) posdata.Points.RemoveAt(0);
                if (veldata.Points.Count() > 100) veldata.Points.RemoveAt(0);
                posBox.Text = pval.ToString();
                velBox.Text = vval.ToString();
                posdata.Points.AddXY(x, pval);
                veldata.Points.AddXY(x, vval);
                PosChart.ResetAutoValues();
                VelChart.ResetAutoValues();
                x++;
            }
            else
            {
                timer3.Enabled = false;
                MessageBox.Show("No More Data", "Error");
            }
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int newByte = 0;
            bytesToRead = serialPort1.BytesToRead;
            while (bytesToRead != 0)
            {
                newByte = serialPort1.ReadByte();
                databyte.Enqueue(newByte);
                bytesToRead = serialPort1.BytesToRead;
            }
        }
        //Converter converts values, performs math, calls UpdateSeries
        private void Converter()
        {
            int upcount;
            int downcount;
            double position=0.0;
            double velocityCPS;
            double velocityRPM;
            encoderUpCounts.TryDequeue(out upcount);
            encoderDownCounts.TryDequeue(out downcount);
            //TODO: Perform RPM conversion
            velocityCPS = ((double)upcount - (double)downcount)/timeDiff;
            VelCountBox.Text = velocityCPS.ToString();
            velocityRPM = (velocityCPS * 60.0 / (20.4 * 48.0));
            position = (position + ((velocityCPS * 0.2 * 4) / (20.4 * 48.0)));
            Posvalues.Enqueue(position);
            Velvalues.Enqueue(velocityRPM);
            UpdateSeries();

        }
        //Only collects data from UART, calls Converter, currently collecting only 3 data bytes as opposed to 5
        //Can modify as required
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                int valfromq;
                bool queuenotempty;
                queuenotempty = databyte.TryDequeue(out valfromq);
                while (queuenotempty)
                {
                    switch (is255)
                    {
                        case 1:
                            encoderUpCounts.Enqueue(valfromq);
                            is255++;
                            break;
                        case 2:
                            encoderDownCounts.Enqueue(valfromq);
                            is255 = 0;
                            break;
                    }
                    if (valfromq == 255)
                    {
                        is255 = 0;
                        is255++;

                    }
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
            int garbage;
            while (DirectionByte.Count > 0)
            {
                DirectionByte.TryDequeue(out garbage);
            }
            while (PWMByte.Count > 0)
            {
                PWMByte.TryDequeue(out garbage);
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
                pwm = (int)((1 - (double)pwm / 100) * 65535);
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
            int pwmscale = (int)((double)100 / halfticks);
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
                    pwmval = (halfticks - sliderpos)*pwmscale;
                    dirval = 2;
                    
                }
                else if (sliderpos > halfticks)
                {
                    pwmval = (sliderpos - halfticks) * pwmscale;
                    dirval = 1;
                }
                
                //Send the read values to PreparePackets
                if ((sliderpos != prevsliderpos) && (prevsliderpos != 999))
                {
                    PreparePackets(dirval, pwmval);
                }
                else if (prevsliderpos == -999)
                {
                    PreparePackets(dirval, pwmval);
                }
                prevsliderpos = sliderpos;
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            Converter();
        }

        private void PlotBut_MouseClick(object sender, MouseEventArgs e)
        {
            timer3.Enabled = true;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
