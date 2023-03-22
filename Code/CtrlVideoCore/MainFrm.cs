using LibVLCSharp.Shared;

namespace CtrlVideoCore
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            this.WindowState= FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle= FormBorderStyle.None;
            
            InitializeComponent();
            Core.Initialize();

            
            this.Load += (obj, e) => 
            {
                var libvlc = new LibVLC();
                videoView1.MediaPlayer = new MediaPlayer(libvlc);
                videoView1.Dock= DockStyle.Fill;
                
                var uri = new Uri(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"Videos","00", "test.mp4")); 
                var media = new Media(libvlc, uri, ":input-repeat=65535");
                videoView1.MediaPlayer.Play(media);


            };
        }

       
    }
}