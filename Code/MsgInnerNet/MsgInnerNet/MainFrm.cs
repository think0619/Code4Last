using MsgInnerNet.Common;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsgInnerNet
{
    public partial class MainFrm : Form
    {
        public string ServerMQUrl = string.Empty;
        public SubscriberSocket subscriberSocket = null;

        public static string SubscriberString = "ControlMsg";

        /// <summary>
        /// 测试连通性
        /// </summary>
        public static string TestConnectionString = "TestConnection";

        public MainFrm()
        {
            InitializeComponent();

            ServerMQUrl = ConfigurationManager.AppSettings["ServerMQUrl"].ToString();
            this.serveripTxt.Text = ServerMQUrl;

            InitNetMQ();

            System.Timers.Timer timer = new System.Timers.Timer(10 * 60 * 1000);
            timer.Elapsed += async (obj, e) =>
            {
                await ConnectServiceAsync();
            };
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

            this.Load += (a, b) =>
            {
                clearMsglogBox();
                Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(15 * 1000);
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Hide();
                    });
                });
            };

            #region add notifyIcon
            notifyIcon1.Icon = new System.Drawing.Icon(@"Res\data-transfer-64.ico");
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Data Transfer";
            notifyIcon1.Visible = true;
            this.notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.notifyIcon1.DoubleClick += (obj, ev) =>
            {
                this.ShowInTaskbar = true;
                this.Show();
            };
            this.notifyIcon1.ContextMenuStrip.Items.Add("Dashboard", null, (obj, e) =>
            {
                this.ShowInTaskbar = true;
                this.Show();
            });
            this.notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, (obj, e) =>
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Windows.Forms.Application.Exit();
            });
            #endregion 
        }

        private void clearbtn_Click(object sender, EventArgs e)
        {
            this.msglogBox.Text = String.Empty;
        }

        private void clearMsglogBox()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1 * 60 * 1000);
            timer.Elapsed +=   (obj, e) =>
            {
                if (this.msglogBox.Text.Length > 1000)
                {
                    msglogBox.BeginInvoke(new MethodInvoker(delegate
                    {
                        this.msglogBox.Text = String.Empty;
                    }));
                };
            };
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }


        public void InitNetMQ()
        {
            if (subscriberSocket != null)
            {
                subscriberSocket.Close();
            }

            if (!String.IsNullOrWhiteSpace(ServerMQUrl))
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        subscriberSocket = new SubscriberSocket();
                        subscriberSocket.Connect($"tcp://{ServerMQUrl}");
                        subscriberSocket.Subscribe(SubscriberString);

                        while (true)
                        {
                            string results = subscriberSocket.ReceiveFrameString();
                            string[] split = results.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                            if (split.Length > 1)
                            {
                                string cmdmsg = split[1];
                                msglogBox.BeginInvoke(new MethodInvoker(delegate { msglogBox.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {cmdmsg}.\r\n"); }));

                                if (!TestConnectionString.Equals(cmdmsg) && !String.IsNullOrWhiteSpace(cmdmsg))
                                {
                                    List<string> msgList = JsonHelper.JsonHelper.DeserializeJsonToList<string>(cmdmsg);
                                    // udp
                                    // `cmd$Model$m1_open01`
                                    // `cmd$PC$V4DES_Video_PV01`

                                    List<TransferMsg> msgs = new List<TransferMsg>();
                                    foreach (string msg in msgList)
                                    {
                                        if (msg.IndexOf("cmd") == 0)
                                        {
                                            string[] cmdSplit = msg.Split(new[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                            if (cmdSplit.Length > 2)
                                            {
                                                string key = cmdSplit[1];
                                                string content = cmdSplit[2];
                                                if (!String.IsNullOrWhiteSpace(key) && !String.IsNullOrWhiteSpace(content))
                                                {
                                                    msgs.Add(new TransferMsg(key.Trim(), content.Trim()));
                                                }
                                            }
                                        }
                                    }
                                    if (msgs.Count > 0)
                                    {
                                        var models = msgs.Select(msg => msg.Model).Distinct().ToList();
                                        foreach (var model in models)
                                        {
                                            // `PC$V4DES_Video_PV01`
                                            //广播
                                            if ("PC".Equals(model, StringComparison.OrdinalIgnoreCase))
                                            {
                                                var pcMsgs = msgs.Where(x => x.Model.Equals(model, StringComparison.OrdinalIgnoreCase))?.ToList();

                                                if (pcMsgs != null && pcMsgs != null)
                                                {
                                                    var thisPCIP = Program.IPConfigList.Where(x => x.FormatKey.Equals(model, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                                    if (thisPCIP != null)
                                                    {
                                                        if (UDPHelper.ValidateIPv4(thisPCIP.IPAddress) && thisPCIP.Port > 0 && thisPCIP.Port < 65536)
                                                        {
                                                            Task.Factory.StartNew(() =>
                                                            {
                                                                var udpclient = new EchoClient(thisPCIP.IPAddress, thisPCIP.Port);
                                                                udpclient.Connect();
                                                                pcMsgs.ForEach(msg =>
                                                                {
                                                                    udpclient.Send(msg.Content);
                                                                });
                                                                udpclient.DisconnectAndStop();
                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                            // `Model$m1_open01`
                                            //单播
                                            //模型控制器受实际情况限制，得设置静态IP
                                            else if ("Model".Equals(model, StringComparison.OrdinalIgnoreCase))
                                            {
                                                List<TransferMsg> modelMsgList = new List<TransferMsg>() { };
                                                var modelMsgs = msgs.Where(x => x.Model.Equals(model, StringComparison.OrdinalIgnoreCase))?.ToList();
                                                foreach (var _ModelMsg in modelMsgs)
                                                {
                                                    string[] modelCmds = _ModelMsg.Content?.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                                                    if (modelCmds.Length > 1)
                                                    {
                                                        modelMsgList.Add(new TransferMsg(modelCmds[0], modelCmds[1]));
                                                    }
                                                }
                                                if (modelMsgList?.Count > 0)
                                                {
                                                    var modelNames = modelMsgList.Select(msg => msg.Model).Distinct().ToList();
                                                    foreach (var modelName in modelNames)
                                                    {
                                                        var oneModelMsg = modelMsgList.Where(m => modelName.Equals(m.Model)).ToList();
                                                        var thisPCIP = Program.IPConfigList.Where(x => x.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                                        if (thisPCIP != null)
                                                        {
                                                            if (UDPHelper.ValidateIPv4(thisPCIP.IPAddress) && thisPCIP.Port > 0 && thisPCIP.Port < 65536)
                                                            {
                                                                Task.Factory.StartNew(() =>
                                                                {
                                                                    var udpclient = new EchoClient(thisPCIP.IPAddress, thisPCIP.Port);
                                                                    udpclient.Connect();
                                                                    oneModelMsg.ForEach(msg =>
                                                                    {
                                                                        udpclient.Send(msg.Content);
                                                                        Task.Delay(50).Wait();
                                                                    });
                                                                    udpclient.DisconnectAndStop();
                                                                }
                                                               );
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        string log = String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{ex.Message}.\r\n");
                        msglogBox.BeginInvoke(new MethodInvoker(delegate { msglogBox.AppendText(log); }));
                    }
                });
            }
            else
            {
                string log = String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:mq host is empty.\r\n");
                this.msglogBox.AppendText(log);
                // msglogBox.BeginInvoke(new MethodInvoker(delegate { msglogBox.AppendText(log); }));
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var result = await ConnectServiceAsync();
        }

        public async Task<string> ConnectServiceAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var testmsg = new
                {
                    Msg = new List<string> { "...test connection msg..." },
                    Sender = "zhkjdev"
                };

                string url = ConfigurationManager.AppSettings["ServerWebUrl"].ToString();
                url = String.Format($"http://{url}/api/HandleMsg/ReceiveMsgFromUser");
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 5 * 1000;
                try
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(JsonHelper.JsonHelper.SerializeObject(testmsg));
                    }

                    using (var response = await request.GetResponseAsync())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    return await reader.ReadToEndAsync();
                                }
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
    }
}
