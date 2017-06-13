using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.Threading;

namespace HFWifi
{
    public partial class MainForm : Form
    {
        int iState = 0, iLengthHaveSaved = 0;
        int num_a_pack = 21;
        byte[] bData;
        int flag_data = 0;
        int count = 0;
        double time_c = 0, time_interval = 0.05;
        double freq = 20.0;
        double _XScaleMax = 5.0;
        bool is_show = false;
        public RollingPointPairList[] voltLine = new RollingPointPairList[8];
        public LineItem[] voltCurve = new LineItem[8];
        int channel = 0;

        delegate void HandleInterfaceUpdateDelegate(byte[] buf, int allLength);
        HandleInterfaceUpdateDelegate interfaceUpdateHandle; //用于更新label上的重量数据 

        public MainForm()
        {
            InitializeComponent();
        }
        public string Baud;
        private void 串口设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialSetting FSerialSetting = new SerialSetting();
            FSerialSetting.ShowDialog();
            if (FSerialSetting.DialogResult == DialogResult.OK)
            {
                this.serialPort1.BaudRate = FSerialSetting.Baud;
                this.serialPort1.PortName = FSerialSetting.Port;
                interfaceUpdateHandle = new HandleInterfaceUpdateDelegate(UpdateLabel);  //实例化委托对象 
                FSerialSetting.Close();
                this.serialPort1.Open();
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int n = serialPort1.BytesToRead;
                byte[] bufRecv = new byte[n];
                serialPort1.Read(bufRecv, 0, n);
                this.Invoke(interfaceUpdateHandle, new object[] { bufRecv ,n}); //指定interfaceUpdateHandle委托更新并处理数据
            }
            catch(Exception expt)
            {
                MessageBox.Show(expt.Message);
            }
        
        }
        private Color GetColor(int i)
        {
            Color colour;
            switch (i)
            {
                case 0:
                    {
                        colour = Color.GreenYellow;
                        break;
                    }
                case 1:
                    {
                        colour = Color.Pink;
                        break;
                    }
                case 2:
                    {
                        colour = Color.DeepSkyBlue;
                        break;
                    }
                case 3:
                    {
                        colour = Color.SeaGreen;
                        break;
                    }
                case 4:
                    {
                        colour = Color.RosyBrown;
                        break;
                    }
                case 5:
                    {
                        colour = Color.Purple;
                        break;
                    }
                case 6:
                    {
                        colour = Color.Olive;
                        break;
                    }
                case 7:
                    {
                        colour = Color.Silver;
                        break;
                    }
                default:
                    {
                        colour = Color.Purple;
                        break;
                    }
            }
            return colour;
        }

        private void zedGraphVibShow_Realtime()
        {
            int j;
            Color colour;
            GraphPane pane1 = graphShow1.GraphPane;

            pane1.Title.Text = "应变数据";
            pane1.YAxis.Title.Text = "应变(ue)";
            pane1.XAxis.Title.Text = "时间(s)";
            pane1.XAxis.Scale.Min = 0;        //X轴最小值0
            pane1.XAxis.Scale.Max = _XScaleMax; 
            //pane1.XAxis.Scale.MinorStep = _XScaleMax / 100;//X轴小步长1,也就是小间隔
            pane1.XAxis.Scale.MajorStep = _XScaleMax / 20;//X轴大步长为5，也就是显示文字的大间隔
            pane1.YAxis.MajorGrid.DashOff = 0;
            pane1.YAxis.MinorGrid.DashOff = 0;
            pane1.YAxis.MajorGrid.IsZeroLine = false;
            graphShow1.IsShowPointValues = false;
            graphShow1.IsShowHScrollBar = false;
            graphShow1.IsAutoScrollRange = false;
            graphShow1.IsShowPointValues = true;
            graphShow1.AxisChange();
            graphShow1.Refresh();

            for (j = 0; j < 8; j++)
            {
                colour = GetColor(j);
                voltLine[j] = new RollingPointPairList(1000);
                voltCurve[j] = pane1.AddCurve("电压" + j.ToString(), voltLine[j], colour, SymbolType.Diamond);
            }

            graphShow1.AxisChange();
            graphShow1.Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bData = new byte[num_a_pack];
            time_interval = 1.0 /freq;
            zedGraphVibShow_Realtime();
        }

        private double Data_V(int num)
        {
            Int16 iTemp;
            UInt16 uChannel, uTemp;
            double dTemp;

            //uTemp = System.BitConverter.ToUInt16(bData, num * 2 + 1);
            iTemp = (Int16)(bData[num * 2] * 256 + bData[num * 2 + 1]);

            //dTemp = (double)iTemp * 0.117839 + 86.431880; //ch1 <1000
            //dTemp = (double)iTemp * 0.118036 + 86.554680; //ch1 <2500
            //dTemp = (double)iTemp * 0.117582 + 86.118768; //ch6 <2500
            //dTemp = (double)iTemp;

            //dTemp = (double)iTemp * 0.118036 + 86.554680 + 1 - 166; //ch1 <2500 二次
            //dTemp = (double)iTemp * 0.117582 + 86.118768 + 1; //ch6 <2500 二次

            dTemp = (double)iTemp;

            return (Math.Round(dTemp, 0));
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (is_show == true)
            {
                byte[] send1 = { 0x53, 0xff, 0x11, 0x12, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x45 };
                this.serialPort1.Write(send1, 0, 17);
                button_start.Text = "开始显示";
                is_show = false;
            }
            else if (is_show == false)
            {
                byte[] send1 = { 0x53, 0xff, 0x05, 0x06, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x45 };
                this.serialPort1.Write(send1, 0, 17);
                Thread.Sleep(500);
                if (radioButton1.Checked)
                {
                    channel = 0;
                    voltCurve[0].IsVisible = true;
                }
                else voltCurve[0].IsVisible = false;

                if (radioButton2.Checked)
                {
                    channel = 1;
                    voltCurve[1].IsVisible = true;
                }
                else voltCurve[1].IsVisible = false;

                if (radioButton3.Checked)
                {
                    channel = 2;
                    voltCurve[2].IsVisible = true;
                }
                else voltCurve[2].IsVisible = false;

                if (radioButton4.Checked)
                {
                    channel = 3;
                    voltCurve[3].IsVisible = true;
                }
                else voltCurve[3].IsVisible = false;

                if (radioButton5.Checked)
                {
                    channel = 4;
                    voltCurve[4].IsVisible = true;
                }
                else voltCurve[4].IsVisible = false;

                if (radioButton6.Checked)
                {
                    channel = 5;
                    voltCurve[5].IsVisible = true;
                }
                else voltCurve[5].IsVisible = false;

                if (radioButton7.Checked)
                {
                    channel = 6;
                    voltCurve[6].IsVisible = true;
                }
                else voltCurve[6].IsVisible = false;

                if (radioButton8.Checked)
                {
                    channel = 7;
                    voltCurve[7].IsVisible = true;
                }
                else voltCurve[7].IsVisible = false;

                button_start.Text = "暂停显示";
                byte[] send2 = { 0x53, 0xff, 0x01, 0x02, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x45 };
                this.serialPort1.Write(send2, 0, 17);
                is_show = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double time_min = time_c;
            Scale xScale = graphShow1.GraphPane.XAxis.Scale;
            if (time_min > xScale.Max - _XScaleMax / 100.0)
            {
                xScale.Min = xScale.Max;
                xScale.Max = time_min + _XScaleMax;
            }

            try
            {
                graphShow1.AxisChange();
                graphShow1.Refresh();
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private void CurveAddPoint_V(double data, double time, int num)
        {
            voltLine[num].Add(time, data);// - vibzero[0, num]);
        }

        private void UpdateLabel(byte[] strRecv, int allLength)
        {
            int index = 0;
            int iLengthToSaveOrg, iLengthToSave;
            int i;
            double MO;
            if (is_show == false) return;
            while (index < allLength)
            {
                switch (iState)
                {
                    case 0:
                        {
                            if (strRecv[index] == 0x53)
                            {
                                index++;
                                iState = 1;
                            }
                            else
                            {
                                if (flag_data == 1)//丢弃待验证的包
                                {
                                    flag_data = 0;
                                }
                                index++;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (strRecv[index] == 0x20 || strRecv[index] == 0x21 || strRecv[index] == 0x22 || strRecv[index] == 0x23)
                            {
                                index++;
                                iState = 2;
                            }
                            else
                            {
                                if (flag_data == 1)//丢弃待验证的包
                                {
                                    flag_data = 0;
                                }
                                index++;
                                iState = 0;
                            }
                            break;
                        }
                    case 2:
                        {
                            if(flag_data == 1)
                            {
                                double temp = 0;
                                //上一包验证通过(验证方法是用下一帧的帧头作为这一帧的帧尾）此处加处理函数：
                                for (i = 0; i < 8; i++)
                                {
                                        MO = Data_V(i);
                                        CurveAddPoint_V(MO, time_c, i);
                                        if (i == channel) temp = MO;
                                }

                                time_c += time_interval;

                                graphShow1.AxisChange();
                                graphShow1.Refresh();

                                flag_data = 0;
                                count++;
                                textBox1.Text = count.ToString();
                                textBox2.Text = temp.ToString();
                            }
                            iLengthToSave =  num_a_pack - iLengthHaveSaved; //还剩下iLengthToSave填满一包
                            if (iLengthToSave + index <= allLength)//剩下的数据足够填满一包
                            {             
                                Array.Copy(strRecv, index, bData, iLengthHaveSaved, iLengthToSave); //填满一包
                                flag_data = 1;//一包待验证
                                iLengthHaveSaved = 0; //下一个数据从头开始存
                                index +=  iLengthToSave; //继续往下走
                                iState = 0;//检测下一帧帧头
                            }
                            else
                            {
                                iLengthToSaveOrg = allLength - index;//剩下数据不够填满一包
                                Array.Copy(strRecv, index, bData, iLengthHaveSaved, iLengthToSaveOrg);//剩下数据全填进去
                                iLengthHaveSaved += iLengthToSaveOrg;//填了 iLengthToSaveOrg个
                                index += iLengthToSaveOrg;//往下走
                            }
                            break;
                        }
                    default: break;
                }
            }
        }

    }
}
