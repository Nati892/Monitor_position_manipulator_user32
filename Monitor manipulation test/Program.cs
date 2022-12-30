using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Monitor_manipulation_test
{
    internal class Program
    {
        static void Main(string[] args)
        {

            for (int index = 0; index < Screen.AllScreens.Length; index++)
            {
                var screen = Screen.AllScreens[index];
                Console.WriteLine("Name: {0}", screen.DeviceName);
                Console.WriteLine("Bounds: {0}", screen.Bounds);
                Console.WriteLine("Working Area: {0}", screen.WorkingArea);
                Console.WriteLine("Primary Screen: {0}", screen.Primary);
                Console.WriteLine("Type: {0}", screen.GetType());
            }

            var devices = DisplayWrapper.GetAllConnectedMonitors();
            var a = DisplayWrapper.GetDeviceModeByRef(devices[1]);

            DisplayWrapper. DISPLAY_DEVICE DD = new DisplayWrapper.DISPLAY_DEVICE();
            DisplayWrapper.DEVMODE dm = new DisplayWrapper.DEVMODE();
            DD.cb = Marshal.SizeOf(DD);
            DisplayWrapper.EnumDisplayDevices(null, 1, ref DD, 0);
            DisplayWrapper.EnumDisplaySettings(DD.DeviceName, 0, ref dm);
            dm.dmPositionX = 1920;
            dm.dmPositionY = 0;
            dm.dmFields = (uint)(DisplayWrapper.DM.Position);
            var res = DisplayWrapper.ChangeDisplaySettingsExW(DD.DeviceName, ref dm, IntPtr.Zero, DisplayWrapper.ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY | DisplayWrapper.ChangeDisplaySettingsFlags.CDS_RESET_EX);
            //DEVMODE dmNULL = new DEVMODE();
            // var res2 = ChangeDisplaySettingsEx(null,ref dmNULL, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_NONE, IntPtr.Zero);

        }





    }
}
