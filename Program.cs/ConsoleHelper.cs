 ﻿using System;
using System.Threading.Tasks;

namespace CybersecurityChatbot.Utils
{
    /// <summary>
    /// Helper class for console UI enhancements
    /// CROSS-PLATFORM - Works on Windows, Linux, and Mac
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Sets up the console with appropriate colours and sizing
        /// </summary>
        public static void SetupConsole()
        {
            Console.Title = "Cybersecurity Awareness Bot | PROG6221";

            // Cross-platform window sizing (works on all platforms)
            try
            {
                // These work on all platforms
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Clear();

                // Window sizing - may not work on all platforms, so wrapped in try-catch
                try
                {
                    Console.WindowWidth = Math.Min(120, Console.LargestWindowWidth);
                    Console.WindowHeight = Math.Min(40, Console.LargestWindowHeight);
                }
                catch
                {
                    // Ignore - sizing not supported on this platform
                }
            }
            catch
            {
                // Fallback for any console issues
                Console.Clear();
            }
        }

        /// <summary>
        /// Displays the ASCII art logo (cross-platform)
        /// </summary>
        public static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string asciiArt = @"
    +====================================================================================+
    |                                                                                    |
    |          ██████╗██╗   ██╗██████╗ ███████╗██████╗     █████╗ ██╗    ██╗ █████╗ ██████╗ ███████╗    |
    |         ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗   ██╔══██╗██║    ██║██╔══██╗██╔══██╗██╔════╝    |
    |         ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝   ███████║██║ █╗ ██║███████║██████╔╝███████╗    |
    |         ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗   ██╔══██║██║███╗██║██╔══██║██╔══██╗╚════██║    |
    |         ╚██████╗   ██║   ██████╔╝███████╗██║  ██║   ██║  ██║╚███╔███╔╝██║  ██║██║  ██║███████║    |
    |          ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝   ╚═╝  ╚═╝ ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝    |
    |                                                                                    |
    |                    ██████╗  ██████╗ ████████╗                                      |
    |                    ██╔══██╗██╔═══██╗╚══██╔══╝                                      |
    |                    ██████╔╝██║   ██║   ██║                                         |
    |                    ██╔══██╗██║   ██║   ██║                                         |
    |                    ██████╔╝╚██████╔╝   ██║                                         |
    |                    ╚═════╝  ╚═════╝    ╚═╝                                         |
    |                                                                                    |
    |                    >>> CYBERSECURITY AWARENESS BOT <<<                             |
    |                         Your Digital Safety Companion                              |
    |                                                                                    |
    +====================================================================================+";

            Console.WriteLine(asciiArt);
            Console.ResetColor();
        }

        /// <summary>
        /// Creates a typing effect for more natural conversation
        /// Cross-platform compatible
        /// </summary>
        public static async Task TypeText(string text, int delayMilliseconds = 25)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                await Task.Delay(delayMilliseconds);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Writes coloured text to the console
        /// Cross-platform compatible
        /// </summary>
        public static void WriteColourText(string text, ConsoleColor colour)
        {
            try
            {
                Console.ForegroundColor = colour;
                Console.WriteLine(text);
                Console.ResetColor();
            }
            catch
            {
                // Fallback for platforms without colour support
                Console.WriteLine(text);
            }
        }

        /// <summary>
        /// Displays a decorative border with optional title
        /// Cross-platform - uses only ASCII characters
        /// </summary>
        public static void DisplayBorder(string title = "")
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            catch { }

            Console.WriteLine("+" + new string('-', 78) + "+");

            if (!string.IsNullOrEmpty(title))
            {
                int padding = (78 - title.Length) / 2;
                Console.WriteLine("|" + new string(' ', padding) + title + new string(' ', 78 - padding - title.Length) + "|");
                Console.WriteLine("|" + new string('-', 78) + "|");
            }

            try
            {
                Console.ResetColor();
            }
            catch { }
        }

        /// <summary>
        /// Closes the border
        /// Cross-platform compatible
        /// </summary>
        public static void CloseBorder()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            catch { }

            Console.WriteLine("+" + new string('-', 78) + "+");

            try
            {
                Console.ResetColor();
            }
            catch { }
        }

        /// <summary>
        /// Displays the help menu showing available topics
        /// Cross-platform compatible
        /// </summary>
        public static void DisplayHelpMenu()
        {
            DisplayBorder("WHAT CAN I HELP YOU WITH?");

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            catch { }

            Console.WriteLine("|                                                                              |");
            Console.WriteLine("|  [PASSWORD SAFETY]     |  [PHISHING DETECTION]     |  [SAFE BROWSING]        |");
            Console.WriteLine("|  [TWO-FACTOR AUTH]     |  [MALWARE PROTECTION]     |  [VPN & PRIVACY]        |");
            Console.WriteLine("|  [SOCIAL ENGINEERING]  |  [DATA PRIVACY]           |  [GENERAL CYBERSECURITY] |");
            Console.WriteLine("|                                                                              |");
            Console.WriteLine("|  TIPS TO ASK:                                                                 |");
            Console.WriteLine("|     - 'How do I create a strong password?'                                   |");
            Console.WriteLine("|     - 'How can I spot a phishing email?'                                     |");
            Console.WriteLine("|     - 'What is two-factor authentication?'                                   |");
            Console.WriteLine("|     - 'How do I stay safe on public Wi-Fi?'                                  |");
            Console.WriteLine("|     - 'What is a VPN and do I need one?'                                     |");
            Console.WriteLine("|                                                                              |");
            Console.WriteLine("|  COMMANDS:                                                                    |");
            Console.WriteLine("|     'help'    - Show this menu                                                |");
            Console.WriteLine("|     'stats'   - Show session statistics                                      |");
            Console.WriteLine("|     'clear'   - Clear the screen                                             |");
            Console.WriteLine("|     'exit'    - End the conversation                                         |");
            Console.WriteLine("|                                                                              |");

            try
            {
                Console.ResetColor();
            }
            catch { }

            CloseBorder();
        }

        /// <summary>
        /// Displays a section header - using only ASCII characters
        /// Cross-platform compatible
        /// </summary>
        public static void DisplaySectionHeader(string title)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            catch { }

            Console.WriteLine($"\n[+] {title} [+]");
            Console.WriteLine(new string('=', 50));

            try
            {
                Console.ResetColor();
            }
            catch { }
        }
    }
}
