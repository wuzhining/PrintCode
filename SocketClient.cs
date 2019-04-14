using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

using LabelManager2;
using System.Data.OleDb;

namespace iplant_BarCodePrint
{
    public class MyObject
    {
        private Socket _Socket;
        private string _MyName = "";
        private byte[] _Buffer = new byte[1024];
        private List<byte> data = new List<byte>();

        public Socket Socket
        {
            get { return _Socket; }
            set { _Socket = value; }
        }

        public byte[] Buffer
        {
            get { return _Buffer; }
            set { _Buffer = value; }
        }
        public string MyName
        {
            get { return _MyName; }
            set { _MyName = value; }
        }

        public MyObject(Socket socket, string myName)
        {
            _Socket = socket;
            _MyName = myName;
        }

        public MyObject(Socket socket, string myName, byte[] buffer)
        {
            _Socket = socket;
            _MyName = myName;
            _Buffer = buffer;
        }
    }

    [DataContract]
    class MsgData
    {
        [DataMember(Order = 0)]
        public int type { get; set; }
        [DataMember(Order = 1)]
        public Msg[] barCodeList { get; set; }
    }

    [DataContract]
    class Msg
    {
        [DataMember(Order = 0)]
        public string SN { get; set; }
        [DataMember(Order = 1)]
        public string ProductNo { get; set; }
        [DataMember(Order = 2)]
        public string SUPNO { get; set; }
        [DataMember(Order = 3)]
        public string RN { get; set; }
        [DataMember(Order = 4)]
        public string PTime { get; set; }
        [DataMember(Order = 5)]
        public string LOTNO { get; set; }
        [DataMember(Order = 6)]
        public string NUM { get; set; }
    }

    class SocketClient 
    {
        private Form1 maiForm;
        private Queue<MsgData> listQueue = new Queue<MsgData>();

       
        public SocketClient()
        {
            maiForm = new Form1();
            maiForm.Show();

        }
        public void initSocket(string ip, int port)
        {
            maiForm.addListItemText("正在链接服务器ip:" + ip + ",port:" + port + "...");
            Socket clientSk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5670);
            clientSk.BeginConnect(remoteEP, new AsyncCallback(ConnectComplete), new MyObject(clientSk, "开始连接了"));
            
            //clientSk.BeginConnect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5670), new AsyncCallback(ConnectComplete), new MyObject(clientSk, "开始连接了"));

        }

        public void ConnectComplete(IAsyncResult ar)
        {
            MyObject myObj = ar.AsyncState as MyObject;
            Socket _client = myObj.Socket;
            _client.EndConnect(ar); 

            if (_client.Connected == true)
            {
                maiForm.addListItemText("链接服务器成功！！！");
                byte[] datas = System.Text.Encoding.UTF8.GetBytes("abc++++");
                _client.BeginSend(datas, 0, datas.Length, SocketFlags.None, new AsyncCallback(SendComplete), new MyObject(_client, "开始发送了", datas));

                byte[] buff = new byte[2048];
                _client.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(RecievedComplete), new MyObject(_client, "开始接收了", buff));
                Start();
            }
        }

        public void SendComplete(IAsyncResult ar)
        {
            MyObject myObj = ar.AsyncState as MyObject;
            Socket sk = myObj.Socket;

            int sended = sk.EndSend(ar);

            maiForm.addListItemText(System.Text.Encoding.UTF8.GetString(myObj.Buffer));
        }

        public void RecievedComplete(IAsyncResult ar)
        {
            MyObject myObj = ar.AsyncState as MyObject;
            Socket sk = myObj.Socket;

            int recieved = sk.EndReceive(ar);

            for(int j = 0; j < recieved; j++)
            {

            }

            string strBuff = System.Text.Encoding.UTF8.GetString(myObj.Buffer);
            //readObject(strBuff);
            maiForm.addListItemText(System.Text.Encoding.UTF8.GetString(myObj.Buffer));
            var data_ = (MsgData)Newtonsoft.Json.JsonConvert.DeserializeObject(strBuff.TrimEnd('\0'), typeof(MsgData));

            lock (listQueue) 
            {
                listQueue.Enqueue(data_);
            } 

        }

        public void Start()//启动  
        {
            Thread thread = new Thread(readObject);
            thread.IsBackground = true;
            thread.Start();
        }  


        public void readObject()
        {
            while (true)
            {
                while(listQueue.Count != 0)
                {
                    MsgData data_;
                    lock (listQueue)
                    {
                        data_ = listQueue.Dequeue();
                    }

                    switch (data_.type)
                    {
                        case 1:
                            //Msg[] msg_ = data_.barCodeList;
                            printBrCode(data_.barCodeList);
                            break;
                        case 2:
                            break;
                        default:
                            break;
                    }
                }
                if(listQueue.Count == 0)
                    Thread.Sleep(3000);  
               
            }
        }


        private bool printBrCode(Msg[] msgArray)
        {
            //      SocketClient skClient = new SocketClient();

            //        skClient.initSocket(ip, port);


            ApplicationClass lbl = new ApplicationClass();
            int index = 0;

            try
            {
                string a1 = System.Windows.Forms.Application.StartupPath + "\\mes_barcode.lab";
                lbl.Documents.Open(@a1, false);
                lbl.Documents.Open(@"c:\\mes_barcode.lab", false);// 调用设计好的label文件

                //lbl.Dialogs.Item(LabelManager2.enumDialogType.lppxPrinterSelectDialog).Show();

                Document doc = lbl.ActiveDocument;

                foreach (Msg msg_ in msgArray)
                {
                    //给参数传值
                    doc.Variables.Item("SN").Value = msg_.SN;   //唯一码
                    doc.Variables.Item("ProductNo").Value = msg_.ProductNo;     //物料编码
                    doc.Variables.Item("SUPNO").Value = msg_.SUPNO;    //供应商
                    doc.Variables.Item("RN").Value = msg_.RN;          //流水号
                    doc.Variables.Item("PTime").Value = msg_.PTime;           //制造日期
                    doc.Variables.Item("LOTNO").Value = msg_.LOTNO;            //批次号
                    doc.Variables.Item("NUM").Value = msg_.NUM;    //数量


                    //doc.Printer.Name = textBox3.Text;
                    //int Num = Convert.ToInt32(Convert.ToInt32(textBox1.Text.ToString()));        //打印数量
                    int Num = 1;
                    //doc.PrintDocument(Num);    //打印
                    maiForm.addListItemText(index++.ToString());
                }
               
            }
            catch (Exception ex)
            {
                maiForm.addListItemText(ex.Message);
            }
            finally
            {
                lbl.Quit();                                         //退出
            }

            return true;
        }

    }
}
