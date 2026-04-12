using System;
using System.Threading.Tasks;
using CybersecurityChatbot.Utils;

namespace CybersecurityChatbot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Set up the console environment
                ConsoleHelper.SetupConsole();

                // Display a loading message
                ConsoleHelper.WriteColourText("Initializing Cybersecurity Bot...", ConsoleColor.DarkGray);
                await Task.Delay(500);

                // Create and start the chatbot
                Chatbot chatbot = new Chatbot();
                await chatbot.StartAsync();
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteColourText($"\n❌ An unexpected error occurred: {ex.Message}", ConsoleColor.Red);
                ConsoleHelper.WriteColourText("Please restart the application.", ConsoleColor.Yellow);
            }
            finally
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}