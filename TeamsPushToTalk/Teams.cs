using Sidi;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamsPushToTalk
{
    internal class Teams : IDisposable
    {
        bool mute = true;

        public Teams()
        {
        }

        static Window FindTeamsWindow()
        {
            var candidates = Window.GetDesktopWindow().GetWindows()
                .Where(_ => _.Text.EndsWith("Microsoft Teams") && _.Class.Equals("Chrome_WidgetWin_1"))
                .ToList();

            foreach (var i in candidates)
            {
                Console.WriteLine($"{i.Text} | {i.Class}");
            }

            return candidates.FirstOrDefault();
        }

        Window foregroundWindow;

        internal Task Unmute() => Task.Factory.StartNew(() =>
        {
            lock (this)
            {
                Console.WriteLine("unmute");
                if (!mute) return;
                mute = false;

                var teamsWindow = FindTeamsWindow();

                if (teamsWindow != null)
                {
                    foregroundWindow = Window.GetForegroundWindow();
                    teamsWindow.SetForegroundWindow();
                    SendMuteShortcut(teamsWindow);
                }
            }
        }, TaskCreationOptions.LongRunning);

        static void SendMuteShortcut(Window teamsWindow)
        {
            Keyboard.Messaging.PostMessageAll(teamsWindow.Handle, new Keyboard.Key(Keyboard.Messaging.VKeys.KEY_M), false, true, true);
        }

        internal Task Mute() => Task.Factory.StartNew(() =>
        {
            lock (this)
            {
                Console.WriteLine("mute");
                if (mute) return;
                mute = true;

                var teamsWindow = FindTeamsWindow();

                if (teamsWindow != null)
                {
                    teamsWindow.SetForegroundWindow();
                    SendMuteShortcut(teamsWindow);
                    foregroundWindow.SetForegroundWindow();
                }
            }
        }, TaskCreationOptions.LongRunning);

        public void Dispose()
        {
        }
    }
}