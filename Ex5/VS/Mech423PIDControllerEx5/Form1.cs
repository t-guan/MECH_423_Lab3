using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Mech423PIDControllerEx5
{
    public partial class Form1 : Form
    {
        //Collect UART data
        ConcurrentQueue<Int32> databyte = new ConcurrentQueue<Int32>();
        //Encoder Count storage
        ConcurrentQueue<Int32> encoderUpCounts = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> encoderDownCounts = new ConcurrentQueue<Int32>();
        //Direction and PWM Bytes for DC motor Control
        ConcurrentQueue<Int32> PositionByte = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> PWMByte = new ConcurrentQueue<Int32>();
        int x = 0;
        int bytesToRead = 0;
        int is255 = 0;
        double position = 0.0;
        private static int pwmval;
        private static int sliderticks = 8;

        //The divisor for velocity
        double timeDiff = 0.6;
        Series posdata = new Series();
        Series veldata = new Series();
        Series pwmdata = new Series();
        //Variable File Builder
        string path = @"C:\Users\Thomas\MECH423Lab3\MECH-423-Lab3-\Ex5\VS\pidvalues.csv";
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
            //Serial Port Init
            serialPort1.PortName = "COM7";
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }
        private void ConBut_MouseClick(object sender, MouseEventArgs e)
        {

            if (serialPort1.IsOpen == false)
            {
                serialPort1.Open();
                ConBut.Text = "Disconnect";
            }
            else if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                ConBut.Text = "Connect";
            }
        }
        private void SumConverter(int upc, int doc)
        {
            // velocity, position calculations
            double velocityCPS = ((double)upc - (double)doc) / timeDiff;
            VelCountBox.Text = velocityCPS.ToString();
            double velocityRPM = (velocityCPS * 60.0 / (20.4 * 12.0));
            position = position + ((velocityRPM * 8 * 3.14) / 60) * timeDiff;
            //Store values into CSV
            File.AppendAllText(path, x.ToString() + delim + position.ToString() + '\n');
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
            x++;
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
        //Only collects data from UART, calls Converter, currently collecting only 3 data bytes as opposed to 5
        //Can modify as required
        private void timer1_Tick(object sender, EventArgs e)
        {
            int counter = 0;
            int usum = 0, dsum = 0;

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
                            is255 = 2;
                            break;
                        case 2:
                            encoderDownCounts.Enqueue(valfromq);
                            dsum = valfromq;
                            is255 = 0;
                            counter++; // increments here, so 1 counter increment == 1 full packet
                            SumConverter(usum, dsum);
                            break;
                    }
                }
            }
        }
        private void PreparePackets(int pos, int pwm)
        {
            PreparePosPacket(pos);
            PreparePWMPacket(pwm);
                if (PositionByte.Count == 5 && PWMByte.Count == 5)
                {
                    SendPackets();
                }
                else
                {
                    DestroyPackets();
                }
        }
        private void PreparePosPacket(int position)
        {
            //Enqueue first byte
            PositionByte.Enqueue(255);
            //Enqueue direction byte
            PositionByte.Enqueue(4);
            //Fill rest with Zeros
            if (position > 150)
            {
                position = 150;
            }
            else if (position < 0)
            {
                position = 0;
            }

            ushort pwmnum16 = Convert.ToUInt16(position);
            byte upperpwm = (byte)(pwmnum16 >> 8);
            byte lowerpwm = (byte)(pwmnum16 & 0xff);
            byte esc = 0;
            PositionByte.Enqueue(upperpwm);
            PositionByte.Enqueue(lowerpwm);
            PositionByte.Enqueue(esc);
        }
        private void DestroyPackets()
        {
            int garbage;
            while (PositionByte.Count > 0)
            {
                PositionByte.TryDequeue(out garbage);
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
                pwm = 100;
            }
            else if (pwm < 0)
            {
                pwm = 0;
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
            //Enqeue 3 for pwm, so the direction is not chagned
            PWMByte.Enqueue(3);
            //Enqueue the Upper Byte first
            PWMByte.Enqueue(upperpwm);
            //Enqueue the lower byte
            PWMByte.Enqueue(lowerpwm);
            // Enqueue escape byte
            PWMByte.Enqueue(esc);
        }
        private void SendPackets()
        {
            byte[] Pos_Byte = { 0, 0, 0, 0, 0 };
            byte[] PWM_Byte = { 0, 0, 0, 0, 0 };
            int value;
            for (int i = 0; i < 5; i++)
            {
                PositionByte.TryDequeue(out value);
                Pos_Byte[i] = (byte)value;
                PWMByte.TryDequeue(out value);
                PWM_Byte[i] = (byte)value;
            }
            serialPort1.Write(Pos_Byte, 0, 5);
            serialPort1.Write(PWM_Byte, 0, 5);

        }
        private void sendCtrlbut_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetPositionbox.TextLength == 0 || targetPWMbox.TextLength == 0)
            {
                MessageBox.Show("Incomplete Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                PreparePackets(Convert.ToInt32(targetPositionbox.Text), Convert.ToInt32(targetPWMbox.Text));
            }
        }
    }
}
