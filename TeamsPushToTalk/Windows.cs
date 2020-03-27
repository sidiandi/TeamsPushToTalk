using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Sidi
{
    public static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();


        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        
        [DllImport("user32.dll")]
        internal static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetDesktopWindow();

        public enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }
    }

    public class Window
    {
        IntPtr handle;

        //  User-defined conversion from double to Digit
        public static implicit operator IntPtr(Window w)
        {
            return w.handle;
        }
        
        Window(IntPtr handle)
        {
            this.handle = handle;
        }

        public IntPtr Handle => handle;

        public static Window GetForegroundWindow()
        {
            return Window.FromHandle(NativeMethods.GetForegroundWindow());
        }
        
        public static Window GetDesktopWindow()
        {
            return FromHandle(NativeMethods.GetDesktopWindow());
        }

        public static Window FromHandle(IntPtr handle)
        {
            return new Window(handle);
        }

        public Window GetTopWindow()
        {
            return FromHandle(NativeMethods.GetTopWindow(this));
        }

        public Window GetNext()
        {
            return FromHandle(NativeMethods.GetWindow(this, (uint)NativeMethods.GetWindow_Cmd.GW_HWNDNEXT)); ;
        }

        public IEnumerable<Window> GetWindows()
        {
            for (
                var w = GetTopWindow();
                w.IsValid;
                w = w.GetNext())
            {
                yield return w;
            }
        }

        bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        public string Text
        {
            get
            {
                var sb = new StringBuilder(1024);
                NativeMethods.GetWindowText(this, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public string Class
        {
            get
            {
                var sb = new StringBuilder(1024);
                NativeMethods.GetClassName(this, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            var r = obj as Window;
            return r != null && handle == r.handle;
        }

        public override int GetHashCode()
        {
            return handle.ToInt32();
        }

        const int WM_KEYDOWN = 0x0100;

        public void SetForegroundWindow()
        {
            NativeMethods.SetForegroundWindow(this.handle);
        }

        /*
        [TestFixture]
        public class Test : TestBase
        {
            private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            [Test, Explicit]
            public void GetZOrder()
            {
                var desktop = Window.GetDesktopWindow();
                var windows = desktop.GetWindows().ToList();
                log.Info(windows.ListFormat().Add(_ => _.Class, _ => _.Text));
            }

            [Test, Explicit]
            public void Dte()
            {
                var dte = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE");
                log.Info(dte.ActiveWindow.Caption);
            }
        }
        */
    }
}
