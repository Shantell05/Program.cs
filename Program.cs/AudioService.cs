 ﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CybersecurityChatbot.Services
{
    public static class AudioService
    {
        [DllImport("winmm.dll", SetLastError = true)]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);

        private const uint SND_FILENAME = 0x00020000;
        private const uint SND_ASYNC = 0x00000001;

        public static void PlayVoiceGreeting()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

            if (File.Exists(path))
            {
                Console.WriteLine("\n[VOICE GREETING] Playing...");
                PlaySound(path, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine("[COMPLETE]\n");
            }
            else
            {
                Console.WriteLine("\nHello! Welcome to the Cybersecurity Awareness Bot.\n");
            }
        }
    }
}
