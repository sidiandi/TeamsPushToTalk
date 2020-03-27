using Sidi;
using System;
using System.Diagnostics;
using System.Linq;
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

        internal void Unmute()
        {
            var teamsWindow = FindTeamsWindow();

            if (teamsWindow != null)
            {
                if (!mute) return;
                mute = false;
                Console.WriteLine("unmute");
                foregroundWindow = Window.GetForegroundWindow();
                teamsWindow.SetForegroundWindow();
                SendMuteShortcut(teamsWindow);
            }
        }

        static void SendMuteShortcut(Window teamsWindow)
        {
            Keyboard.Messaging.PostMessageAll(teamsWindow.Handle, new Keyboard.Key(Keyboard.Messaging.VKeys.KEY_M), false, true, true);
        }

        internal void Mute()
        {
            var teamsWindow = FindTeamsWindow();

            if (teamsWindow != null)
            {
                if (mute) return;
                mute = true;
                Console.WriteLine("mute");
                teamsWindow.SetForegroundWindow();
                SendMuteShortcut(teamsWindow);
                foregroundWindow.SetForegroundWindow();
            }
        }

        public void Dispose()
        {
        }
    }
}