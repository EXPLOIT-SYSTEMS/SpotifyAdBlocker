﻿using System.Net;
using System.Security.Principal;

namespace SpotifyAdBlocker
{
    internal class Main
    {
        public static void Init()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                Console.WriteLine("Run the Program as Administrator to make it Work!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Press Enter to Block analytics");
            Console.ReadLine();
            BlockAnalytics();
        }

        public static void BlockAnalytics()
        {
            string HostsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
            List<string> AllHostLines = File.ReadAllLines(HostsFile).ToList();
            AllHostLines.Add("");
            AllHostLines.Add("#ExploitSystems Spotify Blocker");
            AllHostLines.Add("");
            foreach (string url in apiendpoints.URLToBlock)
            {
                bool IsExisting = false;
                foreach (string line in AllHostLines)
                {
                    if (line.Contains(url))
                    {
                        IsExisting = true;
                        break;
                    }
                }
                if (!IsExisting) AllHostLines.Add($"0.0.0.0 {url}");
            }
            File.WriteAllLines(HostsFile, AllHostLines);

            foreach (string url in apiendpoints.URLToBlock)
            {
                try
                {
                    Uri uri = new("http://" + url + "/");
                    var ip = Dns.GetHostAddresses(uri.Host)[0];
                    Console.WriteLine($"failed to block {url}", ConsoleColor.Red);
                }
                catch
                {
                    Console.WriteLine($"{url} is succesfully blocked", ConsoleColor.Green);
                }
            }

            Console.ReadLine();
        }
    }
}