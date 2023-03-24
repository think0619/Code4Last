using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MsgInnerNet.Common
{
    public class UDPHelper /*: IDisposable*/
    {
        //// Flag: Has Dispose already been called?
        //bool disposed = false;
        //// Instantiate a SafeHandle instance.
        //SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        //// Public implementation of Dispose pattern callable by consumers.
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //// Protected implementation of Dispose pattern.
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposed)
        //        return;

        //    if (disposing)
        //    {
        //        handle.Dispose();
        //        // Free any other managed objects here.
        //        //
        //    }

        //    disposed = true;
        //}
        public string RemoteIPAdd { get; set; }
        public int RemotePort { get; set; }
        public UdpClient udpClient { get; set; }
        public Task udpSendThread { get; set; }
        public CancellationTokenSource taskController { get; set; }
        public ConcurrentQueue<string> msgQueue { get; set; }

        public UDPHelper(string _RemoteIPAdd, int _RemotePort)
        {
            RemoteIPAdd = _RemoteIPAdd;
            RemotePort = _RemotePort;
            udpClient = null;
            udpSendThread = null;
            taskController = new CancellationTokenSource();
            msgQueue = new ConcurrentQueue<string>();
        }

        public void OpenUDPSendQueue()
        {
          //  int localPort = 20000;
            while (udpClient == null)
            {
                try
                {
                    udpClient = new UdpClient();
                }
                catch (SocketException)
                {
                    
                }
            }
            udpClient.Connect(RemoteIPAdd, RemotePort);
            udpClient.Client.SendTimeout = 300;
            udpClient.Client.ReceiveTimeout = 300;

            taskController = new CancellationTokenSource();
            var token = taskController.Token; 
            udpSendThread = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (udpClient != null && (!msgQueue.IsEmpty))
                    {
                        string message = string.Empty;
                        if (msgQueue.TryDequeue(out message))
                        {
                            Debug.WriteLine(message);
                            Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                            try
                            {
                                udpClient.Send(sendBytes, sendBytes.Length);
                            }
                            catch { }
                        }
                    }
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }, token);
        }

        public void CloseUDPSendQueue()
        { 
            if (udpClient != null)
            {
                udpClient.Close();
            }
            try 
            { 
                taskController.Cancel();
                taskController.Dispose();
                udpClient = null;
                udpSendThread = null;
                msgQueue = new ConcurrentQueue<string>();
            }
            catch { }
        }

        public void AddUDPMsg(string msg)
        {
            if (udpClient != null)
            {
                string enqueueTxt = String.IsNullOrEmpty(msg) ? string.Empty : msg.Trim();
                msgQueue.Enqueue(enqueueTxt);
            }
        }

        /// <summary>
        /// 发送UDP消息
        ///  ///其实此项目中，发送后完全不用处理接收后的消息，
        /// </summary>
        /// <param name="remoteip"></param>
        /// <param name="remoteport"></param>
        /// <param name="sendmessage"></param>
        public static void SendUDPMsg(string remoteip, int remoteport, string sendMessage,int localPort=-1)
        {
            if (ValidateIPv4(remoteip) && (!String.IsNullOrWhiteSpace(sendMessage)))
            {
                Task.Factory.StartNew(() =>
                {
                    UdpClient udpClient;
                    if (localPort == -1)
                    {
                        udpClient = new UdpClient();
                    }
                    else
                    {
                        udpClient = new UdpClient(localPort);
                    } 

                     Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(String.Format($@"{sendMessage.Trim()}"));
                     udpClient.Connect(remoteip, remoteport);
                     udpClient.Send(sendBytes, sendBytes.Length);
                     udpClient.Client.SendTimeout = 500;

                     udpClient.Client.ReceiveTimeout = 500;
                     IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(remoteip), 0);
                     //Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                     //string returnData = Encoding.ASCII.GetString(receiveBytes);  

                     var timeToWait = TimeSpan.FromMilliseconds(1000);
                     var asyncResult = udpClient.BeginReceive(null, null);
                     asyncResult.AsyncWaitHandle.WaitOne(timeToWait);
                     if (asyncResult.IsCompleted)
                     {
                         try
                         {
                             IPEndPoint remoteEP = null;
                             byte[] receivedData = udpClient.EndReceive(asyncResult, ref remoteEP);
                             // EndReceive worked and we have received data and remote endpoint
                         }
                         catch (Exception ex)
                         {
                             // EndReceive failed and we ended up here
                         }
                         finally
                         {
                            udpClient.Client.Shutdown(SocketShutdown.Both);
                            udpClient.Client.Close();
                            //udpClient.Close();
                         }
                     }
                     else
                     {
                         // The operation wasn't completed before the timeout and we're off the hook
                     }
                     udpClient.Close();
                 });
            }
        }


        public static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }
            byte tempForParsing;
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}
