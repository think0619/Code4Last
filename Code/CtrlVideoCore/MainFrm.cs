using CtrlVideoPlayerCore.Model;
using LibVLCSharp.Shared;
using System;

namespace CtrlVideoCore
{
    public partial class MainFrm : Form
    {
        LibVLC libvlc  ;
        //视频文件数组
        internal string[] videoFilePath = new string[10];
        //实际视频数量
        int videoValidCount = 0;
        //当前视频播放索引
        int videoCurrentIndex = 0;
        //视频循环播放模式,default is list cycle
        VideoPlayMode videoPlayMode = VideoPlayMode.ListCycle;


        public MainFrm()
        { 
            this.WindowState= FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle= FormBorderStyle.None;
            
            InitializeComponent();
            Core.Initialize();
            libvlc = new LibVLC();


            this.Load += (obj, e) =>
            {
                GetVideoFiles(); 
                InitVideoPlayer();
                StartPlayer();
            };
        }

        /// <summary>
        /// get video files
        /// </summary>
        public void GetVideoFiles()
        {
            //video folder
            string folderPath = System.IO.Path.Combine(Environment.CurrentDirectory,  "Videos");
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
            videoView1.Dock = DockStyle.Fill; 

            videoView1.MediaPlayer.EndReached += (s, e1) => 
            {
                if (videoPlayMode == VideoPlayMode.ListCycle)
                {
                    while (true)
                    {
                        videoCurrentIndex = (++videoCurrentIndex) % videoFilePath.Length;
                        if (File.Exists(videoFilePath[videoCurrentIndex]))
                        {
                            break;
                        }
                    }
                }

                var _media = new Media(libvlc, new Uri(videoFilePath[videoCurrentIndex]));
                ThreadPool.QueueUserWorkItem(_ => this.videoView1.MediaPlayer.Play(_media));
            }; 
        }

        public void StartPlayer() 
        {
            //play first video
            while (true)
            {
                videoCurrentIndex = (videoCurrentIndex) % videoFilePath.Length;
                if (File.Exists(videoFilePath[videoCurrentIndex]))
                {
                    break;
                }
                videoCurrentIndex++;
            }
            var firstMedia = new Media(libvlc, new Uri(videoFilePath[videoCurrentIndex]));
            this.videoView1.MediaPlayer.Play(firstMedia);
        }
    }
}