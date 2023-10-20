using CtrlVideoPlayerCore.Model;
using LibVLCSharp.Shared;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using LibVLCSharp.WinForms;
using CtrlVideoPlayerCore.Helper;
using static CtrlVideoPlayerCore.Helper.ScreenHelper;
using MQTTnet.Client;
using MQTTnet;

namespace CtrlVideoCore
{
    public partial class MainFrm : Form
    {
        LibVLC libvlc;
        //视频文件数组
        internal string[] videoFilePath = new string[20]; 
        //当前视频播放索引
        int videoCurrentIndex = 0;
        int videoCurrentPlayCount = 0;
        int videoValidCount = 0;
        //视频循环播放模式,default is list cycle
        VideoPlayMode videoPlayMode = VideoPlayMode.ListCycle;

        //设备标识码
        string GUID = "ABCDE";
        int UdpListenPort = 47147;

        DEVMODE dm;

        MqttFactory mqttFactory;
        IMqttClient _mqttClient;
        const string MQTTTopic = "ExhibitionPC";

        public MainFrm()
        { 
            libvlc = new LibVLC();
            mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            InitializeComponent();

            this.Load += new EventHandler(LoadFormLocationSize);
            this.Load += (obj, e) =>
            { 
                Core.Initialize();
                GetVideoFiles(); 

                string _GUID = ConfigurationManager.AppSettings["GUID"].ToString();
                if (!String.IsNullOrWhiteSpace(_GUID))
                {
                    GUID = _GUID;
                }
                ConnectMQTTTask();
                // Task.Factory.StartNew(ReceiveUdpMessage);
            };

            this.Shown += (obj, e) =>
            {
                InitVideoPlayer();
                InitStartPlayer();
            };
        }

        /// <summary>
        /// hand udp message
        /// </summary>
        public void ReceiveUdpMessage()
        {
            UdpClient receivingUdpClient = new UdpClient(UdpListenPort);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    string receiveMsg = returnData.ToString();
                    if (receiveMsg?.ToUpper().IndexOf(GUID) == 0)
                    {
                        //cmd$ParkModel${GUID}_restartapp

                        //cmd$ParkModel${GUID}_video_pv01
                        //cmd$ParkModel${GUID}_video_pause
                        //cmd$ParkModel${GUID}_video_start
                        //cmd$ParkModel${GUID}_video_single
                        //cmd$ParkModel${GUID}_video_list 
                        //cmd$ParkModel${GUID}_video_next 
                        //cmd$ParkModel${GUID}_video_previous

                        string cmdmsg = receiveMsg.Replace($"{GUID}_", "");
                        if (!String.IsNullOrWhiteSpace(cmdmsg))
                        {
                            cmdmsg = cmdmsg.ToLower();

                            #region video cmd
                            if (cmdmsg.IndexOf("video") == 0)
                            {
                                if (this.videoView1.MediaPlayer != null)
                                {
                                    cmdmsg = cmdmsg.Replace("video_", "");
                                    if ("start".Equals(cmdmsg))
                                    {
                                        //video start
                                        StartVideoPlayer();
                                    }
                                    else if ("pause".Equals(cmdmsg))
                                    {
                                        //video pause
                                        PauseVideoPlayer();
                                    }
                                    else if ("single".Equals(cmdmsg))
                                    {
                                       ChangePlayMode(VideoPlayMode.SingleCycle);
                                    }
                                    else if ("list".Equals(cmdmsg))
                                    {
                                       ChangePlayMode(VideoPlayMode.ListCycle);
                                    }
                                    else if ("previous".Equals(cmdmsg))
                                    {
                                       SwitchPrevioustItem();
                                    }
                                    else if ("next".Equals(cmdmsg))
                                    {
                                        SwitchNextItem();
                                    }
                                    else
                                    {
                                        if (cmdmsg.IndexOf("pv") == 0)
                                        {
                                            //video change
                                            //cmd$ParkModel${GUID}_pv01
                                            int msgIndex = -1;
                                            string _msgcmd = cmdmsg.Replace("pv", "");
                                            if (Int32.TryParse(_msgcmd, out msgIndex))
                                            {
                                                SwitchVideoPlayerSrc(msgIndex);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                          
                            else
                            {
                                if ("restartapp".Equals(cmdmsg))
                                { 
                                    Application.Restart();
                                    Environment.Exit(0);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// get video files
        /// </summary>
        public void GetVideoFiles()
        {
            //video folder
            string folderPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Videos");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //child video folder
            for (int i = 0; i < videoFilePath.Length; i++)
            {
                string childFolderPath = System.IO.Path.Combine(folderPath, (i + 1).ToString().PadLeft(2, '0'));
                if (!Directory.Exists(childFolderPath))
                {
                    Directory.CreateDirectory(childFolderPath);
                }
                string singleVideoFilePath = Directory.EnumerateFiles(childFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                                                     .Where(s => s.EndsWith(".mp4", StringComparison.InvariantCultureIgnoreCase)/*|| s.EndsWith(".jpg")*/)
                                                     .FirstOrDefault();
                videoFilePath[i] = String.IsNullOrWhiteSpace(singleVideoFilePath) ? "" : singleVideoFilePath;
                if (singleVideoFilePath != null && File.Exists(singleVideoFilePath))
                {
                    videoValidCount++;
                }
            }
        }

        /// <summary>
        /// initialize video player
        /// </summary>
        public void InitVideoPlayer()
        {
            videoView1.MediaPlayer = new MediaPlayer(libvlc);
            //videoView1.MediaPlayer.AspectRatio="32:9";


            //string ScreenNo = ConfigurationManager.AppSettings["ScreenNoX"].ToString();
            //int index = 1;
            //if (!String.IsNullOrWhiteSpace(ScreenNo))
            //{
            //    Int32.TryParse(ScreenNo, out index);
            //}
            //Screen[] sc = Screen.AllScreens;
            //if ((sc.Length - index) >= 1)
            //{
            //    this.StartPosition = FormStartPosition.Manual;
            //    this.Location = Screen.AllScreens[index].WorkingArea.Location;
            //    videoView1.MediaPlayer.AspectRatio=String.Format($"{Screen.AllScreens[index].Bounds.Width}:{Screen.AllScreens[index].Bounds.Height}");
            //}
            //else
            //{
            //    videoView1.MediaPlayer.AspectRatio=String.Format($"{this.Width}:{this.Height}"); 
            //}
            videoView1.MediaPlayer.AspectRatio=String.Format($"{this.Width}:{this.Height}");
            //videoView1.MediaPlayer.AspectRatio=String.Format($"{dm.dmPelsWidth}:{dm.dmPelsHeight}");

            videoView1.Dock = DockStyle.Fill;
           
            videoView1.MediaPlayer.EndReached += (s, e1) =>
            {
                //单个循环时 超过设定时常 改为列表循环
                int setSeconds = 120;
                string _SingleVideoCycleMaxSecs = ConfigurationManager.AppSettings["SingleVideoCycleMaxSecs"].ToString();
                Int32.TryParse(_SingleVideoCycleMaxSecs, out setSeconds);
                if (videoPlayMode == VideoPlayMode.SingleCycle)
                {
                    var playDuration = (videoView1.MediaPlayer.Media.Duration / 1000) * videoCurrentPlayCount;
                    if (setSeconds <= playDuration)
                    {
                        videoPlayMode = VideoPlayMode.ListCycle;
                    }
                }

                //列表循环
                if (videoPlayMode == VideoPlayMode.ListCycle)
                {
                    while (true)
                    {
                        videoCurrentIndex = (++videoCurrentIndex) % videoFilePath.Length;
                        videoCurrentPlayCount = 1;
                        if (File.Exists(videoFilePath[videoCurrentIndex]))
                        {
                            break;
                        }
                    }
                }
                else 
                {
                    videoCurrentPlayCount++;
                }
               
                //单个循环
                var _media = new Media(libvlc, new Uri(videoFilePath[videoCurrentIndex]));
                ThreadPool.QueueUserWorkItem(_ => this.videoView1.MediaPlayer.Play(_media));
            };
        }

        public void InitStartPlayer()
        {
            //play first video
            while (true)
            {
                videoCurrentIndex = (videoCurrentIndex) % videoFilePath.Length;
                videoCurrentPlayCount = 1;
                if (File.Exists(videoFilePath[videoCurrentIndex]))
                {
                    break;
                }
                videoCurrentIndex++;
            }
            var firstMedia = new Media(libvlc, new Uri(videoFilePath[videoCurrentIndex])); 
            this.videoView1.MediaPlayer?.Play(firstMedia);
        } 

        //start video
        public void StartVideoPlayer()
        {
            //video start
            this.videoView1.MediaPlayer?.Play();
        }

        //Pause
        public void PauseVideoPlayer()
        {
            this.videoView1.MediaPlayer?.Pause(); 
        }

        /// <summary>
        /// 切换视频
        /// </summary>
        /// <param name="switchIndex"></param>
        public void SwitchVideoPlayerSrc(int switchIndex)
        {

            if (switchIndex <= videoFilePath.Length)
            {
                if (switchIndex > 0 && File.Exists(videoFilePath[switchIndex - 1]))
                {
                    videoCurrentIndex = switchIndex - 1;

                    var _media = new Media(libvlc, new Uri(videoFilePath[videoCurrentIndex]));
                    this.videoView1.MediaPlayer?.Play(_media);
                    videoCurrentPlayCount = 1;
                }
            }
        }

        public void SwitchNextItem()
        {
            while (true)
            {
                videoCurrentIndex = (++videoCurrentIndex) % videoFilePath.Length;
                if (File.Exists(videoFilePath[videoCurrentIndex]))
                {
                    break;
                }
            }
            SwitchVideoPlayerSrc(videoCurrentIndex + 1);
        }

        public void SwitchPrevioustItem()
        {
            while (true)
            {
                videoCurrentIndex = ((--videoCurrentIndex) + videoFilePath.Length) % videoFilePath.Length;
                if (File.Exists(videoFilePath[videoCurrentIndex]))
                {
                    break;
                }
            }
            SwitchVideoPlayerSrc(videoCurrentIndex + 1);
        }

        //change 
        public void ChangePlayMode(VideoPlayMode _playmode)
        {
            this.videoPlayMode = _playmode;
        } 
        private void LoadFormLocationSize(object sender, EventArgs e)
        {  
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.None; 
            //this.Size=new Size(dm.dmPelsWidth, dm.dmPelsHeight);
            //this.Location =new Point(dm.dmPositionX, dm.dmPositionY);

             this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        public async void ConnectMQTT()
        { 
            var mqttClientOptions = new MqttClientOptionsBuilder()
                                            .WithClientId("ExhibitionVideoPC")
                                            .WithTcpServer("hw.hellolinux.cn", 1883)
                                            .WithCredentials("homediskrelay", "@Xiongsen1994!+")
                                            .Build();

            _mqttClient.ApplicationMessageReceivedAsync += arg =>
            {
                var topic = arg?.ApplicationMessage?.Topic;
                if (MQTTTopic.Equals(topic))
                {
                    var payloadText = Encoding.UTF8.GetString(arg?.ApplicationMessage?.Payload ?? Array.Empty<byte>());
                    if (!String.IsNullOrWhiteSpace(payloadText) && payloadText.StartsWith("cmd",StringComparison.CurrentCulture)) 
                    {
                        //cmd$Beijing${GUID}_restartapp 
                        //cmd$Beijing${GUID}_video_pv01
                        //cmd$Beijing${GUID}_video_pause
                        //cmd$Beijing${GUID}_video_start
                        //cmd$Beijing${GUID}_video_replay
                        //cmd$Beijing${GUID}_video_single
                        //cmd$Beijing${GUID}_video_list 
                        //cmd$Beijing${GUID}_video_next 
                        //cmd$Beijing${GUID}_video_previous
                        string[] msgArrays = payloadText.Split('$');
                        if (msgArrays.Length > 2)
                        {
                            string receiveMsg = msgArrays[2];
                            string cmdmsg = receiveMsg.Replace($"{GUID}_", "");
                            if (!String.IsNullOrWhiteSpace(cmdmsg))
                            {
                                cmdmsg = cmdmsg.ToLower();

                                #region video cmd
                                if (cmdmsg.IndexOf("video") == 0)
                                {
                                    if (this.videoView1.MediaPlayer != null)
                                    {
                                        cmdmsg = cmdmsg.Replace("video_", "");
                                        if ("start".Equals(cmdmsg))
                                        {
                                            //video start
                                            StartVideoPlayer();
                                        }
                                        else if ("pause".Equals(cmdmsg))
                                        {
                                            //video pause
                                            PauseVideoPlayer();
                                        }
                                        else if ("single".Equals(cmdmsg))
                                        {
                                            ChangePlayMode(VideoPlayMode.SingleCycle);
                                        }
                                        else if ("list".Equals(cmdmsg))
                                        {
                                            ChangePlayMode(VideoPlayMode.ListCycle);
                                        }
                                        else if ("previous".Equals(cmdmsg))
                                        {
                                            SwitchPrevioustItem();
                                        }
                                        else if ("next".Equals(cmdmsg))
                                        {
                                            SwitchNextItem();
                                        }
                                        else if ("replay".Equals(cmdmsg))
                                        {
                                            SwitchVideoPlayerSrc(1);
                                        }
                                        else
                                        {
                                            if (cmdmsg.IndexOf("pv") == 0)
                                            {
                                                //video change
                                                //cmd$ParkModel${GUID}_pv01
                                                int msgIndex = -1;
                                                string _msgcmd = cmdmsg.Replace("pv", "");
                                                if (Int32.TryParse(_msgcmd, out msgIndex))
                                                {
                                                    SwitchVideoPlayerSrc(msgIndex);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                else
                                {
                                    if ("restartapp".Equals(cmdmsg))
                                    {
                                        Application.Restart();
                                        Environment.Exit(0);
                                    }
                                }
                            }
                        }
                    }

                }
                return Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                if (e.ClientWasConnected)
                {
                    await _mqttClient.ConnectAsync(_mqttClient.Options);
                }
            };
            try
            {
                await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => { f.WithTopic(MQTTTopic); })
                    // .WithTopicFilter(f => { f.WithTopic("ShowClockTime"); })
                    .Build();

                await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            }
            catch (Exception ex) { }
        }

        private void ConnectMQTTTask()
        {
            ConnectMQTT();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(10000);
                    if (_mqttClient == null || !_mqttClient.IsConnected)
                    {
                         ConnectMQTT();
                    }
                }
            });
        }

    }
}