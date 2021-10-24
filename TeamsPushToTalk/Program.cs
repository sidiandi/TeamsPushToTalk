using hagen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsPushToTalk
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Push-to-Talk for MS Teams

Usage:
  Hold down the middle mouse button to speak.
  Press Ctrl+C to exit.

See https://github.com/sidiandi/TeamsPushToTalk

");
            PushToTalkWithMiddleMouseButton();
        }

        static void PushToTalkWithMiddleMouseButton()
        {
            var keys = new Dictionary<System.Windows.Forms.Keys, bool>();

            bool HotkeyPressed()
            {
                return keys.Get(System.Windows.Forms.Keys.LControlKey) && keys.Get(System.Windows.Forms.Keys.LWin);
            }

            using (var teams = new Teams())
            using (var monitor = new HumanInterfaceDeviceMonitor())
            {
                using (monitor.KeyDown.Subscribe(kea =>
                {
                    Console.WriteLine($"dn: {kea.KeyCode}");
                    keys[kea.KeyCode] = true;

                    if (HotkeyPressed())
                    {
                        monitor.Disable = true;
                        teams.Unmute();
                        monitor.Disable = false;
                    }
                }))
                using (monitor.KeyUp.Subscribe(kea =>
                {
                    Console.WriteLine($"up: {kea.KeyCode}");
                    keys[kea.KeyCode] = false;

                    if (!HotkeyPressed())
                    {
                        monitor.Disable = true;
                        teams.Mute();
                        monitor.Disable = false;
                    }
                }))
                using (monitor.Mouse.Subscribe(me =>
                {
                    if (me.Button == System.Windows.Forms.MouseButtons.Middle)
                    {
                        if (me.Clicks > 0)
                        {
                            teams.Unmute();
                        }
                        else if (me.Clicks < 0)
                        {
                            teams.Mute();
                        }
                    }
                }))
                {
                    Console.ReadLine();
                }
            }
        }

        static void PushToTalkWithLeftControlKey()
        {
            var keys = new Dictionary<System.Windows.Forms.Keys, bool>();

            bool HotkeyPressed()
            {
                return keys.Get(System.Windows.Forms.Keys.LControlKey) && keys.Get(System.Windows.Forms.Keys.LWin);
            }

            using (var teams = new Teams())
            using (var monitor = new HumanInterfaceDeviceMonitor())
            {
                using (monitor.KeyDown.Subscribe(kea =>
                {
                    keys[kea.KeyCode] = true;
                    
                    if (HotkeyPressed())
                    {
                        teams.Unmute();
                    }
                }))
                using (monitor.KeyUp.Subscribe(kea =>
                {
                    keys[kea.KeyCode] = false;

                    if (!HotkeyPressed())
                    {
                        teams.Mute();
                    }
                }))
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
