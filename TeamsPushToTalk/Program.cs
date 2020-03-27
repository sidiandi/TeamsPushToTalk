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
            PushToTalkWithMiddleMouseButton();
        }

        static void PushToTalkWithMiddleMouseButton()
        {
            using (var teams = new Teams())
            using (var monitor = new HumanInterfaceDeviceMonitor())
            {
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
            using (var teams = new Teams())
            using (var monitor = new HumanInterfaceDeviceMonitor())
            {
                using (monitor.KeyDown.Subscribe(kea =>
                {
                    if (kea.KeyCode == System.Windows.Forms.Keys.LControlKey)
                    {
                        monitor.Disable = true;
                        teams.Unmute();
                        monitor.Disable = false;
                    }
                }))
                using (monitor.KeyUp.Subscribe(kea =>
                {
                    if (kea.KeyCode == System.Windows.Forms.Keys.LControlKey)
                    {
                        monitor.Disable = true;
                        teams.Mute();
                        monitor.Disable = false;
                    }
                }))
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
