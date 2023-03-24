using log4net;
using MsgInnerNet.Common;
using MsgInnerService.Common;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MsgInnerService
{
    public partial class Service1 : ServiceBase
    {
        private static ILog serviceLog = LogManager.GetLogger("Service1");

        /// <summary>
        /// NetMQ Url
        /// </summary>
        public string ServerMQUrl = string.Empty;

        /// <summary>
        /// NetMQ Subscriber
        /// </summary>
        public SubscriberSocket subscriberSocket = null;

        public static string SubscriberString = "midmsg";

        /// <summary>
        /// 测试连通性
        /// </summary>
        public static string TestConnectionString = "TestConnection";

        public static List<IPConfig> IPConfigList = null; //IP

        public Service1()
        {
            serviceLog.Info("0");
            InitializeComponent();
            //get url from config
            ServerMQUrl = ConfigurationManager.AppSettings["ServerMQUrl"].ToString();

            string ipDataConfigFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\msgip.json");
            if (File.Exists(ipDataConfigFile))
            {
                try
                {
                    string ipdata = System.IO.File.ReadAllText(ipDataConfigFile);
                    List<IPConfig> ipdataList = JsonHelper.JsonHelper.DeserializeJsonToList<IPConfig>(ipdata);
                    IPConfigList = ipdataList;
                }
                catch (Exception ex)
                {
                    serviceLog.Error(ex.Message);
                }
            }
            else 
            {
                serviceLog.Error("IP config data does not exist.");
            } 
        }

        protected override void OnStart(string[] args)
        {
            if (IPConfigList?.Count>0) 
            {
                InitNetMQ();

                System.Timers.Timer timer = new System.Timers.Timer(1 * 60 * 1000);
                timer.Elapsed += async (obj, e) =>
                {
                    await ConnectServiceAsync();
                };
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Start();
            } 
        }

        protected override void OnStop()
        {
            if (subscriberSocket != null)
            {
                subscriberSocket.Close();
            } 
        }

        public void InitNetMQ()
        {
            serviceLog.Info("service init");
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
                                serviceLog.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {cmdmsg}.\r\n"); 

                                if (!TestConnectionString.Equals(cmdmsg) && !String.IsNullOrWhiteSpace(cmdmsg))
                                {
                                    List<string> msgList = JsonHelper.JsonHelper.DeserializeJsonToList<string>(cmdmsg);
                                    // udp
                                    // midmsg@cmd${key}${content}
                                    // midmsg@cmd$video$xxx 

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
                                                    msgs.Add(new TransferMsg()
                                                    {
                                                        Model = key.Trim(),
                                                        Content = content.Trim(),
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    if (msgs.Count > 0)
                                    {
                                        var models = msgs.Select(msg => msg.Model).Distinct().ToList();
                                        foreach (var model in models)
                                        {
                                            var thisModelIPs = IPConfigList.Where(x => x.FormatKey.Equals(model, StringComparison.OrdinalIgnoreCase))?.ToList();
                                            var thisModelMsgs = msgs.Where(x => x.Model.Equals(model, StringComparison.OrdinalIgnoreCase))?.ToList();

                                            if (thisModelIPs != null && thisModelMsgs != null)
                                            {
                                                foreach (IPConfig modelIP in thisModelIPs)
                                                {
                                                    if (UDPHelper.ValidateIPv4(modelIP.IPAddress) && modelIP.Port > 0 && modelIP.Port < 65536)
                                                    {
                                                       // Task.Factory.StartNew(() =>
                                                        {
                                                            var udpclient = new EchoClient(modelIP.IPAddress, modelIP.Port);
                                                            udpclient.Connect();
                                                            thisModelMsgs.ForEach(msg =>
                                                            {
                                                                udpclient.Send(msg.Content);
                                                            });
                                                            udpclient.DisconnectAndStop();
                                                        }
                                                       // );
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
                        serviceLog.Error(log);
                        //msglogBox.BeginInvoke(new MethodInvoker(delegate { msglogBox.AppendText(log); }));
                    }
                });
            }
            else
            {
                string log = String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:mq host is empty.\r\n");
                 serviceLog.Error(log);
                //this.msglogBox.AppendText(log);
                // msglogBox.BeginInvoke(new MethodInvoker(delegate { msglogBox.AppendText(log); }));
            }
        }


        /// <summary>
        /// send service test
        /// </summary>
        /// <returns></returns>
        public async Task<string> ConnectServiceAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var testmsg = new
                {
                    Msg = new List<string> { "...test connect msg..." },
                    Sender = "zhkjdev"
                };

                string url = ConfigurationManager.AppSettings["ServerWebUrl"].ToString();
                url = String.Format($"http://{url}/api/HandleMsg/ReceiveMsgFromUser");
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 3 * 1000;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(JsonHelper.JsonHelper.SerializeObject(testmsg));
                }
                try
                {
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
                catch
                {
                    return "";
                }
                // await request.GetResponseAsync();
            }
        }
    }
}
