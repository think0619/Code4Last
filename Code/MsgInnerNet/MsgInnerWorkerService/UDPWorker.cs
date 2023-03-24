using Entities;
using Entities.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 
using MsgInnerWorkerService.Common;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MsgInnerWorkerService
{
    public class UDPWorker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

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
        /// ≤‚ ‘¡¨Õ®–‘
        /// </summary>
        public static string TestConnectionString = "TestConnection";

        public static List<IPConfig> IPConfigList = null; //IP


        public UDPWorker(ILogger<Worker> logger)
        {
            _logger = logger;

           
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("program start");
            ServerMQUrl = ConfigurationHelper.AppSetting["SystemCfg:ServerMQUrl"].ToString();

            string ipDataConfigFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data",@"msgip.json");
            if (File.Exists(ipDataConfigFile))
            {
                try
                {
                    string ipdata = System.IO.File.ReadAllText(ipDataConfigFile);
                    List<IPConfig> ipdataList = Entities.JsonHelper.DeserializeJsonToList<IPConfig>(ipdata);
                    IPConfigList = ipdataList; 
                }
                catch (Exception ex)
                { 
                    _logger.LogError(ex.Message);
                }
            }
            else
            {
                _logger.LogError("IP config data does not exist.");
            } 
            await base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            { 
                var task1 = RunMsgTransfer(stoppingToken);  
                await Task.WhenAll(task1 );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
               
            }
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }

        protected Task RunMsgTransfer(CancellationToken stoppingToken)
        {
            var task = Task.Run(() =>
            { 
                try
                {
                    //while (!stoppingToken.IsCancellationRequested)
                    //{
                        if (IPConfigList?.Count > 0)
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
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                        Thread.Sleep(1000);
                   // }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    
                }
            }, stoppingToken);
            return task;
        }

        public void InitNetMQ()
        {
            //serviceLog.Info("service init");
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
                                _logger.LogInformation($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {cmdmsg}. "); 

                                if (!TestConnectionString.Equals(cmdmsg) && !String.IsNullOrWhiteSpace(cmdmsg))
                                {
                                    List<string> msgList = Entities.JsonHelper.DeserializeJsonToList<string>(cmdmsg);
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
                        _logger.LogError(log); 
                    }
                });
            }
            else
            {
                string log = String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:mq host is empty.\r\n");
                _logger.LogError(log);
                //serviceLog.Error(log);
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
                
                string url = ConfigurationHelper.AppSetting["SystemCfg:ServerWebUrl"].ToString();  
                url = String.Format($"http://{url}/api/HandleMsg/ReceiveMsgFromUser");
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 3 * 1000;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Entities.JsonHelper.SerializeObject(testmsg));
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
