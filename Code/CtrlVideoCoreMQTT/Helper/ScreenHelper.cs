using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace CtrlVideoPlayerCore.Helper
{
    public class ScreenHelper
    {
       
        public static DEVMODE GetScreenSize(int screenIndex)
        {
            const int ENUM_CURRENT_SETTINGS = -1;

            var dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(Screen.AllScreens[screenIndex].DeviceName, ENUM_CURRENT_SETTINGS, ref dm);
            return dm;
            //foreach (Screen screen in Screen.AllScreens)
            //{
            //    var dm = new DEVMODE();
            //    dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            //    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);

            //    Console.WriteLine($"Device: {screen.DeviceName}");
            //    Console.WriteLine($"Real Resolution: {dm.dmPelsWidth}x{dm.dmPelsHeight}");
            //    Console.WriteLine($"Virtual Resolution: {screen.Bounds.Width}x{screen.Bounds.Height}");
            //    Console.WriteLine();
            //}
        }



        [DllImport("user32.dll")]
        static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
    }
}
