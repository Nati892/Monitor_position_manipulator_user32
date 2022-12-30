using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_manipulation_test
{
    public class DisplayWrapper
    {


        public static bool ChangeDisplaySettingsExW(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags) {
            return (ChangeDisplaySettingsEx(lpszDeviceName, ref lpDevMode, hwnd, dwflags, IntPtr.Zero) == DISP_CHANGE.Successful);

        }

        public static List<DISPLAY_DEVICE> GetAllConnectedMonitors()
        {
            List<DISPLAY_DEVICE> devices = new List<DISPLAY_DEVICE>();
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            d.cb = Marshal.SizeOf(d);
            for (uint id = 0; EnumDisplayDevices(null, id, ref d, 0); id++)
            {
                if (d.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                {
                    devices.Add(d);
                }
                d.cb = Marshal.SizeOf(d);
            }
            return devices;
        }


        //Gets the device mode of a wanted Device by index
        public static bool GetDeviceModeByIndex(int ModeIndex, ref DEVMODE mode)
        {
            return EnumDisplaySettings(null, ModeIndex, ref mode);
        }



        //Gets monitor id(uint) and changes x y positions immediatly
        public static bool ChangeMonitorPositionEx(uint DeviceId, int X, int Y)
        {
            DISPLAY_DEVICE DD = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            DD.cb = Marshal.SizeOf(DD);
            EnumDisplayDevices(null, DeviceId, ref DD, 0);
            EnumDisplaySettings(DD.DeviceName, 0, ref dm);
            dm.dmPositionX = X;
            dm.dmPositionY = Y;
            dm.dmFields = (uint)(DM.Position);
            var res = ChangeDisplaySettingsEx(DD.DeviceName, ref dm, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);
            return (res == DISP_CHANGE.Successful);
        }

        //Gets monitor id(uint) and changes x y positions immediatly
        public static void ChangeMonitorPositionMain(int X, int Y)
        {
            DISPLAY_DEVICE DD = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            DD.cb = Marshal.SizeOf(DD);
            GetDeviceModeByIndex(0, ref dm);
            EnumDisplaySettings(DD.DeviceName, 0, ref dm);
            dm.dmPositionX = X;
            dm.dmPositionY = Y;
            dm.dmFields = (uint)(DM.Position);
            ChangeDisplaySettings(ref dm, (uint)ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY);
        }

        public static DEVMODE GetDeviceModeByRef(DISPLAY_DEVICE dd)
        {
            DEVMODE dmode = new DEVMODE();
            try
            {
                int id = int.Parse(dd.DeviceKey.Substring(dd.DeviceKey.Length - 4, 4));
                GetDeviceModeByIndex(id, ref dmode);
            }
            catch
            { }
            return dmode;
        }


        #region deisplay devices
        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [Flags()]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }
        #endregion

        #region display settings
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean EnumDisplaySettings(
 [param: MarshalAs(UnmanagedType.LPTStr)]
string lpszDeviceName,
 [param: MarshalAs(UnmanagedType.U4)]
int iModeNum,
 [In, Out]
ref DEVMODE lpDevMode);



        [StructLayout(LayoutKind.Sequential,
CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            // You can define the following constant  
            // but OUTSIDE the structure because you know  
            // that size and layout of the structure  
            // is very important  
            // CCHDEVICENAME = 32 = 0x50  
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            // In addition you can define the last character array  
            // as following:  
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]  
            //public Char[] dmDeviceName;  
            // After the 32-bytes array  
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSpecVersion;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverVersion;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSize;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverExtra;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmFields;
            [MarshalAs(UnmanagedType.I4)]
            public int dmPositionX;
            [MarshalAs(UnmanagedType.I4)]
            public int dmPositionY;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayOrientation;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFixedOutput;
            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmColor;
            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmDuplex;
            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmYResolution;
            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmTTOption;
            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmCollate;
            // CCHDEVICENAME = 32 = 0x50  
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            // Also can be defined as  
            //[MarshalAs(UnmanagedType.ByValArray,  
            // SizeConst = 32, ArraySubType = UnmanagedType.U1)]  
            //public Byte[] dmFormName;  
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmLogPixels;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmBitsPerPel;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsWidth;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsHeight;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFlags;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFrequency;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMMethod;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMIntent;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmMediaType;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDitherType;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved1;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved2;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningWidth;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningHeight;
        }

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int ChangeDisplaySettings(
        [In, Out]
ref DEVMODE lpDevMode,
        [param: MarshalAs(UnmanagedType.U4)]
uint dwflags);



        [DllImport("user32.dll")]
        static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);
        enum DISP_CHANGE : int
        {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }

        [Flags()]
        public enum ChangeDisplaySettingsFlags : uint
        {
            CDS_NONE = 0,
            CDS_UPDATEREGISTRY = 0x00000001,
            CDS_TEST = 0x00000002,
            CDS_FULLSCREEN = 0x00000004,
            CDS_GLOBAL = 0x00000008,
            CDS_SET_PRIMARY = 0x00000010,
            CDS_VIDEOPARAMETERS = 0x00000020,
            CDS_ENABLE_UNSAFE_MODES = 0x00000100,
            CDS_DISABLE_UNSAFE_MODES = 0x00000200,
            CDS_RESET = 0x40000000,
            CDS_RESET_EX = 0x20000000,
            CDS_NORESET = 0x10000000
        }

        [Flags()]
        public enum DM : int
        {
            Orientation = 0x1,
            PaperSize = 0x2,
            PaperLength = 0x4,
            PaperWidth = 0x8,
            Scale = 0x10,
            Position = 0x20,
            NUP = 0x40,
            DisplayOrientation = 0x80,
            Copies = 0x100,
            DefaultSource = 0x200,
            PrintQuality = 0x400,
            Color = 0x800,
            Duplex = 0x1000,
            YResolution = 0x2000,
            TTOption = 0x4000,
            Collate = 0x8000,
            FormName = 0x10000,
            LogPixels = 0x20000,
            BitsPerPixel = 0x40000,
            PelsWidth = 0x80000,
            PelsHeight = 0x100000,
            DisplayFlags = 0x200000,
            DisplayFrequency = 0x400000,
            ICMMethod = 0x800000,
            ICMIntent = 0x1000000,
            MediaType = 0x2000000,
            DitherType = 0x4000000,
            PanningWidth = 0x8000000,
            PanningHeight = 0x10000000,
            DisplayFixedOutput = 0x20000000
        }


        #endregion






    }
}
