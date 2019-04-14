using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace iplant_BarCodePrint
{
    public class StateObject
    {
        public Socket workSocket = null;
        public byte[] buff = new byte[1024];
        public List<byte> data = new List<byte>();
    }


    class SocketClient2
    {
        private Encoding encode = Encoding.Default;
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);

        public delegate void SendCompletedEvnetHandler(string message);
        public event SendCompletedEvnetHandler SendCompleted;
        private void OnSendCompleted(string message)
        {
            if(SendCompleted != null)
            {
                SendCompleted(message);
            }
        }

        public void initConnect(string ip, int port)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.BeginConnect(ip, port, EndConnect, clientSocket);

            StateObject state = new StateObject();
            state.workSocket = clientSocket;
            clientSocket.BeginReceive(state.buff, 0, state.buff.Length, SocketFlags.None, EndReceive, state);
            receiveDone.WaitOne();
            
        }

        private void EndConnect(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            connectDone.Set();
        }

        private void EndSend(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);

            sendDone.Set();
        }

        private void EndReceive(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket socket = state.workSocket;

            int length = socket.EndReceive(ar);
            for (int j =0 ; j < length; j++)
            {
                state.data.Add(state.buff[j]);
            }
            if(state.data.Count >0)
            {
                string receiveData = encode.GetString(state.data.ToArray(), 0, state.data.Count);
                OnSendCompleted(receiveData);
            }
            receiveDone.Set();
        }

        private void DestroySocket(Socket socket)
        {
            if(socket != null)
            {
                if(socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                socket.Close();
            }
        }
    }
}
