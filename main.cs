using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Configuration;
//using LabelManager2;
using System.Data.OleDb;


namespace iplant_BarCodePrint
{
    public partial class mainUI : Form
    {
        int logCount = 0;
        int unFinishLabCnt = 0;
        int finishLabCnt = 0;
        string printerName = "ZDesigner GK888t (EPL)";
        byte[] result;
        String logDirPath;
        public Socket socketClient;
        private Thread threadTCP;
        private Thread threadPrint;
        LabelManager2.Document doc = null;
        private string labName="labelname";
        private string confPath;
        private LabelManager2.ApplicationClass appClass;
        public mainUI()
        {
            InitializeComponent();
            initVal();
            readConf();
            initThread();
            updateUI();
        }
        private void initThread()


        {
            threadPrint = new Thread(new ThreadStart(doPrint));
            threadPrint.Start();
        }

        private void doPrint()
        {
            ConnectSocket(this.textBox_adder.Text, int.Parse(textBox_port.Text));
        }

        public bool ConnectSocket(string ip, int port)
        {
            IPAddress ip_ = IPAddress.Parse(ip);
            try
            {
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketClient.Connect(new IPEndPoint(ip_, port));
                threadTCP = new Thread(new ParameterizedThreadStart(recvMsg));
                threadTCP.Start(socketClient);
                this.addListItemText("服务器链接成功");
                this.bt_connectServer.Text = "断开";
            }
            catch
            {
                addListItemText("连接服务器失败！");
                this.bt_connectServer.Text = "连接";
            }
            return true;
        }
        //监听服务器传送的数据
        private void recvMsg(object obj)
        {
            Socket soket_ = (Socket)obj;
            bool b = soket_.Connected;
            CheckForIllegalCrossThreadCalls = false;
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                result = new byte[1024 * 1024 * 2];
                try
                {
                    int receiveLength = soket_.Receive(result);
                    if (receiveLength != 0)
                    {
                        string msg = System.Text.Encoding.UTF8.GetString(result, 0, receiveLength);
                        //如果socket一次性接收的数据大于缓冲区，程序会自动分割数据，故而需要将已经分割的数据重新拼接
                        if (String.Equals(System.Text.Encoding.UTF8.GetString(result, receiveLength - 5, receiveLength).TrimEnd('\0').Trim(), "}]}"))
                        {
                            builder.Append(msg);
                            addListItemText("接收到打印请求：" + builder);

                            Thread testThread = new Thread(new ParameterizedThreadStart(BeforePirnt));
                            testThread.Start(builder.ToString());
                            //BeforePirnt(builder.ToString());
                            builder.Clear();
                        }
                        else
                        {
                            builder.Append(msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    addListItemText(ex.ToString());
                }
            }
        }
        //打印之前的数据转化
        private void BeforePirnt(object data)
        {
            try
            {
                JObject obj = JObject.Parse(data.ToString().TrimEnd('\0'));
                labName = (string)obj["labName"];
                JArray array = (JArray)obj["barCodeList"];
                this.unFinishLabCnt = array.Count;
                updateUI();
                PrintBarcode(array);
            }
            catch (Exception e)
            {
                addListItemText(e.ToString());
            }
        }
        #region 打印条码 
        private void PrintBarcode(JArray array)
        {
            string strRptFile = confPath + "\\" + labName;
            try
            {
                if (appClass == null)
                {
                    appClass = new LabelManager2.ApplicationClass();
                }
                LabelManager2.Document docRpt = appClass.Documents.Open(strRptFile, false);
                doc = appClass.ActiveDocument;

                if (doc == null)
                {
                    MessageBox.Show(string.Format("{0}\r\n模板文件不存在!", strRptFile));
                    return;
                }

                foreach (JObject jobj in array)
                {
                    foreach (var item in jobj)
                    {
                        doc.Variables.Item(item.Key).Value = item.Value.ToString(); 
                    }
                    //doc.PrintLabel(1);
                    doc.Printer.SwitchTo(printerName);//条码信息发送至打印机
                    doc.PrintDocument(1);

                    unFinishLabCnt--;
                    finishLabCnt++;
                    updateUI();
                }
                docRpt.FormFeed();//开始打印，打印完自动结束
            }
            catch (Exception err)
            {
                //this.Invoke(new PrintMsg(showthemsg), "doPrintTest错误信息：" + err.ToString());
                addListItemText("打印条码出错：" + err.ToString());
            }
            finally
            {
                appClass.Documents.CloseAll(true);
                appClass.Quit();//退出
                appClass = null;
                //doc.Close(false);
                doc = null;
                GC.Collect(0);
            }
        }
        #endregion
        //连接按钮
        private void bt_connectServer_Click(object sender, EventArgs e)
        {
            
            if (this.bt_connectServer.Text == "断开")
            {
                try
                {
                    socketClient.Close();
                    threadTCP.Abort();
                }
                catch {
                    Console.Write("socket close failed!!!!");
                }
                this.bt_connectServer.Text = "连接";
                //addListItemText("已断开服务器！");
            }
            else if (this.bt_connectServer.Text == "连接")
            {
                addListItemText("连接中……");
                
                IPAddress ipRemote;
                try
                {
                    ipRemote = IPAddress.Parse(textBox_adder.Text);
                }
                catch
                {
                    MessageBox.Show("输入的IP地址不合法！", "提示错误！");
                    return;
                }
                ConnectSocket(textBox_adder.Text, int.Parse(textBox_port.Text));
            }
        }
        //消息提示框添加信息
        public void addListItemText(string msg)
        {
            logCount++;
            WriteLog(logCount.ToString()+ " "+msg);
            //Console.Write("{0}\n",msg);
            System.DateTime now = System.DateTime.Now;
            this.log.AppendText(logCount.ToString()+" " +now.ToLocalTime().ToString() + ":  " + msg + "\n");
        }
        //设置初始值
        private void initVal()
        {
            CheckForIllegalCrossThreadCalls = false;
            //logDirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\iplant\\codeprint\\Log";
            logDirPath = System.Windows.Forms.Application.StartupPath + "\\Log";
            confPath = System.Windows.Forms.Application.StartupPath + "\\conf";  
        }
        private void closeingWindow(object sender, FormClosingEventArgs e)
        {
            if (this.bt_connectServer.Text == "断开")
            {
                try
                {
                    socketClient.Close();
                    threadTCP.Abort();
                }
                catch
                {
                    Console.Write("socket close failed!!!!");
                }
                this.bt_connectServer.Text = "连接";
            }
            if (appClass != null)
            {
                try
                {
                    if (appClass.Documents.Count > 0)
                        appClass.Documents.CloseAll(true);
                    appClass.Quit();
                    appClass = null;
                }
                catch (Exception err)
                {
                    throw;
                }
            }
            Environment.Exit(0);
        }
        //
        public void WriteLog(string msg)
        {
            string filePath = logDirPath;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            System.DateTime now = System.DateTime.Now;

            String logPath = logDirPath + "\\" + now.ToString("yyyy-MM-dd") + ".txt";
            try
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    
                    sw.WriteLine(logCount.ToString()+now.ToString("HH:mm:ss：") + msg);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException e)
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine( now.ToString("yyy-MM-dd HH:mm:ss") + e.Message);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        //读取配置信息
        private void readConf()
        {
            string filePath = confPath;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            String confFilePath = confPath + "\\printconf.ini";
            if (!File.Exists(confFilePath))
            {
                using (StreamWriter sw = File.CreateText(confFilePath))
                {
                    sw.WriteLine("ServerIP=192.168.123.117");
                    sw.WriteLine("ServerPort=8899");
                    sw.WriteLine("PrintRate=500");
                    sw.WriteLine("PrinterName=ZDesigner GK888t (EPL)");
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                return;
            }
            using (StreamReader read = new StreamReader(confFilePath))
            {
                string buf;

                while (read.Peek() > -1)
                {
                    buf = read.ReadLine();
                    List<string> list = new List<string>(buf.Split('='));
                    if ("ServerIP" == list.ElementAt(0))
                    {
                        this.textBox_adder.Text = list.ElementAt(1);
                    }
                    else if ("ServerPort" == list.ElementAt(0))
                    {
                        this.textBox_port.Text = list.ElementAt(1);
                    }
                    else if ("PrintRate" == list.ElementAt(0))
                    {
                        string tmp = list.ElementAt(1);

                    }
                    else if ("PrinterName" == list.ElementAt(0))
                    {
                        string tmp = list.ElementAt(1);
                        printerName = tmp;

                    }
                }
                read.Close();
                if (this.textBox_adder.Text == "")
                {
                    this.textBox_adder.Text = "192.168.123.117";
                }
                if (this.textBox_port.Text == "")
                {
                    this.textBox_port.Text = "8899";
                }

            }
        }
        private void writeConf()
        {
            string filePath = confPath;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            String logPath = confPath + "printconf.ini";
            using (StreamWriter sw = File.AppendText(logPath))
            {
                sw.WriteLine("ServerIP=" + this.textBox_adder.Text);
                sw.WriteLine("ServerPort=" + this.textBox_port.Text);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }

        }
        //更新信息
        private void updateUI()
        {
            this.lab_UnFinishLabCnt.Text = unFinishLabCnt.ToString();
            this.lab_FinishLabCnt.Text = finishLabCnt.ToString();
            this.lab_FileName.Text = labName;
        }

        private void btnPrintTest_Click(object sender, EventArgs e)
        {
            JArray jarray = new JArray();
            JObject data=new JObject();
            JObject obj = new JObject();

            //for (int i = 1; i < 26; i++)
            //{
            //    obj.Add("SN" + i, "38AS201909" + (i < 10 ? "0" + i : i.ToString()));
            //    obj.Add("MAC" + i, "38AS201888" + (i < 10 ? "0" + i : i.ToString()));
            //}
            //obj.Add("ProductName", "End Subscriber Equipment");
            //obj.Add("ProductModel", "OCN-1001G-SC/APC"); 
            //obj.Add("Qty", "25PCS");
            //obj.Add("CartonNo", "001");

            //obj.Add("SN","201903010722");
            //obj.Add("MAC","2057AF1380AC");
            //obj.Add("ProName", "EPON ONU");
            //obj.Add("ProModel", "FHR1100");
            //obj.Add("ProVoltage","12V=0.5A");

            for (int i = 1; i < 5; i++)
            {
                obj.Add("EN" + i, "ENNC2019090000000" + (i < 10 ? "0" + i : i.ToString()));
                obj.Add("MAC" + i, "MACC201888" + (i < 10 ? "0" + i : i.ToString()));
            }
            obj.Add("ProName", "EPON ONU");
            obj.Add("ProModel", "FHR1100GZB");
            obj.Add("Qty", "50PCS");
            obj.Add("ProBatchNo", "2019030001");
            obj.Add("SerialNo", "04");
            obj.Add("Total", "40Box");
            obj.Add("ProDate", "2019/03");

            jarray.Add(obj);

            //data.Add("labName","sn_mac_single.lab");
            data.Add("labName", "en_mac01.lab");
            data.Add("barCodeList",jarray);
            addListItemText("开始测试打印："+data.ToString());
            Thread testThread = new Thread(new ParameterizedThreadStart(BeforePirnt));
            testThread.Start(data.ToString());
        }
        //清空信息
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            log.Clear();
        }

    }
}
