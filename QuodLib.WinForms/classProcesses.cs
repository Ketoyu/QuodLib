using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

using System.Drawing;

namespace QuodLib.WinForms {
    class classProcesses
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        public const short SWP_NOMOVE = 0X2;
        public const short SWP_NOSIZE = 1;
        public const short SWP_NOZORDER = 0X4;
        public const int SWP_SHOWWINDOW = 0x0040;

        static Dictionary<uint, Process> GetPrc()
        {
            Process[] Processes = Process.GetProcesses();
            Dictionary<uint, Process> Prcs = new Dictionary<uint, Process>();

            uint i = 1;
            foreach (var process in Processes)
            {
                Prcs.Add(i, process);
                IntPtr handle = Prcs[i].MainWindowHandle;
                var form = Control.FromHandle(handle);
                //int w_ = form.Width;
                //if (w_ > 1)
                //{
                    Console.WriteLine("" + i + ") " + process.ProcessName);
                //}
                i++;
            }
            return Prcs;
        }
        static void Move(Dictionary<uint, Process> Prcs, uint PrcNum, int x, int y)
        {
            IntPtr handle = Prcs[PrcNum].MainWindowHandle;
            var form = Control.FromHandle(handle);
            SetWindowPos(handle, 0, x, y, form.Width, form.Height, SWP_NOZORDER | SWP_SHOWWINDOW);
        }
        static void Size(Dictionary<uint, Process> Prcs, uint PrcNum, int w, int h)
        {
            IntPtr handle = Prcs[PrcNum].MainWindowHandle;
            var form = Control.FromHandle(handle);
            SetWindowPos(handle, 0, form.Location.X, form.Location.Y, w, h, SWP_NOZORDER | SWP_SHOWWINDOW);
        }
        static void Move(Dictionary<uint, Process> Prcs, uint PrcNum, int x, int y, int w, int h)
        {
            IntPtr handle = Prcs[PrcNum].MainWindowHandle;
            var form = Control.FromHandle(handle);
            SetWindowPos(handle, 0, x, y, w,  h, SWP_NOZORDER | SWP_SHOWWINDOW);
        }
        static void Move(Dictionary<uint, Process> Prcs, uint PrcNum, int x, int y, int w, int h, bool topmost)
        {
            Process prc = Prcs[PrcNum];
            IntPtr handle = prc.MainWindowHandle;
            SetWindowPos(handle, 0, x, y, w, h, SWP_NOZORDER | SWP_SHOWWINDOW);
            if (topmost)
            {
                bool topMost = SetForegroundWindow(handle);
            }
        }
    }
}
