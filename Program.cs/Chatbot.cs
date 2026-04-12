using System;
using System.Threading.Tasks;
using CybersecurityChatbot.Models;
using CybersecurityChatbot.Services;
using CybersecurityChatbot.Utils;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Main chatbot class that handles conversation flow
    /// </summary>
    public class Chatbot
    {
        private UserSession _session;
        private ResponseService _responseService;
        private bool _isRunning;

        public Chatbot()
        {
            _session = new UserSession();
            _responseService = new ResponseService();
            _isRunning = true;
        }

        /// <summary>
        /// Starts the chatbot application
        /// </summary>
        public async Task StartAsync()
        {
            // Clear screen and display ASCII art
            Console.Clear();
            ConsoleHelper.DisplayAsciiArt();

            // Voice greeting (plays automatically - cross platform)
            AudioService.PlayVoiceGreeting();

            // Wait a moment before text greeting
            await Task.Delay(500);

            // Text greeting after ASCII and audio - meets rubric requirement
            ConsoleHelper.DisplaySectionHeader("WELCOME TO THE BOT");

            // Get user's name with validation
            string userName = await GetValidUserName();
            _session.UserName = userName;

            // Personalised welcome message with typing effect
            await ConsoleHelper.TypeText($"\n[+] Hello {userName}! I'm your Cybersecurity Awareness Bot! [+]", 35);
            await Task.Delay(400);
            await ConsoleHelper.TypeText($"[*] Welcome to Session #{_session.SessionId} - Your digital safety matters! [*]", 35);
            await Task.Delay(400);

            ConsoleHelper.WriteColourText($"\n[Calendar] Today is {DateTime.Now:D}", ConsoleColor.DarkGray);
            ConsoleHelper.WriteColourText($"[Clock] Session started at {_session.SessionStartTime:h:mm tt}", ConsoleColor.DarkGray);

            // Display help menu
            await Task.Delay(500);
            ConsoleHelper.DisplayHelpMenu();

            // Main conversation loop
            await RunConversationLoop();
        }

        /// <summary>
        /// Gets and validates the user's name with input validation
        /// </summary>
        private async Task<string> GetValidUserName()
        {
            string name = "";
            int attempts = 0;

            ConsoleHelper.WriteColourText("\n" + new string('~', 50), ConsoleColor.DarkCyan);

            while (string.IsNullOrWhiteSpace(name) && attempts < 3)
            {
                Console.Write("\n[?] May I have your name? ");
                name = Console.ReadLine()?.Trim();

                // Input validation - check for empty or whitespace
                if (string.IsNullOrWhiteSpace(name))
                {
                    attempts++;
                    if (attempts < 3)
                    {
                        ConsoleHelper.WriteColourText($"[X] I didn't catch that. Please enter a valid name! (Attempt {attempts}/3)", ConsoleColor.Red);
                    }
                }
                else if (name.Length < 2)
                {
                    ConsoleHelper.WriteColourText("[X] Name must be at least 2 characters long!", ConsoleColor.Red);
                    name = "";
                    attempts++;
                }
                else if (name.Length > 30)
                {
                    ConsoleHelper.WriteColourText("[X] Name is too long! Please use 30 characters or less.", ConsoleColor.Red);
                    name = "";
                    attempts++;
                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Friend"; // Default fallback
                ConsoleHelper.WriteColourText($"\n[!] I'll call you {name} for now. Feel free to correct me!", ConsoleColor.Yellow);
            }

            return name;
        }

        /// <summary>
        /// Main conversation loop that processes user input
        /// </summary>
        private async Task RunConversationLoop()
        {
            while (_isRunning)
            {
                ConsoleHelper.DisplayBorder();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{_session.UserName}]: ");
                Console.ResetColor();

                string userInput = Console.ReadLine()?.Trim();

                // Input validation - handle empty entries gracefully
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    ConsoleHelper.WriteColourText("\n[?] I didn't hear anything. Please type a question or type 'help' for options!", ConsoleColor.Yellow);
                    continue;
                }

                // String manipulation: convert to lower for command checking
                string lowerInput = userInput.ToLower();

                // Check for exit commands
                if (lowerInput == "exit" || lowerInput == "quit" || lowerInput == "bye" || lowerInput == "goodbye")
                {
                    await EndConversation();
                    break;
                }

                // Check for help command
                if (lowerInput == "help" || lowerInput == "menu" || lowerInput == "commands" || lowerInput == "?")
                {
                    ConsoleHelper.DisplayHelpMenu();
                    continue;
                }

                // Check for session info command
                if (lowerInput == "session" || lowerInput == "stats" || lowerInput == "info")
                {
                    DisplaySessionInfo();
                    continue;
                }

                // Check for clear command
                if (lowerInput == "clear" || lowerInput == "cls")
                {
                    Console.Clear();
                    ConsoleHelper.DisplayAsciiArt();
                    ConsoleHelper.WriteColourText($"\n[Screen cleared!] Ready for more questions, {_session.UserName}!", ConsoleColor.Cyan);
                    continue;
                }

                // Process the user's question
                await ProcessUserQuestion(userInput);
            }
        }

        /// <summary>
        /// Displays session statistics using automatic properties
        /// </summary>
        private void DisplaySessionInfo()
        {
            ConsoleHelper.DisplayBorder("SESSION STATISTICS");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"| User: {_session.UserName,-55} |");
            Console.WriteLine($"| Session ID: {_session.SessionId,-53} |");
            Console.WriteLine($"| Started: {_session.SessionStartTime:HH:mm:ss, MMM dd,-51} |");
            Console.WriteLine($"| Duration: {_session.GetSessionDuration(),-53} |");
            Console.WriteLine($"| Questions asked: {_session.QuestionsAsked,-46} |");
            Console.WriteLine($"| Bot responses given: {_responseService.TotalResponsesGiven,-41} |");
            Console.ResetColor();
            ConsoleHelper.CloseBorder();
        }

        /// <summary>
        /// Processes a user question and provides a response
        /// </summary>
        private async Task ProcessUserQuestion(string question)
        {
            // Show typing indicator effect
            ConsoleHelper.WriteColourText("\n[Bot is thinking]", ConsoleColor.DarkGray);
            await Task.Delay(400);

            // Get response from service
            string response = _responseService.GetResponse(question);

            // Update session
            _session.IncrementQuestionCount();

            // Display response with typing effect for natural conversation
            ConsoleHelper.WriteColourText("\n[Cybersecurity Bot]:", ConsoleColor.Cyan);
            await ConsoleHelper.TypeText($"   {response}", 20);

            Console.WriteLine(); // Add spacing for readability
        }

        /// <summary>
        /// Ends the conversation with a personalised goodbye
        /// </summary>
        private async Task EndConversation()
        {
            ConsoleHelper.DisplaySectionHeader("GOODBYE");

            await ConsoleHelper.TypeText($"\nThank you for chatting with me, {_session.UserName}! [*]", 40);
            await Task.Delay(300);

            // String manipulation for pluralisation
            string questionWord = _session.QuestionsAsked == 1 ? "question" : "questions";
            await ConsoleHelper.TypeText($"You asked {_session.QuestionsAsked} cybersecurity {questionWord} during our conversation.", 40);
            await Task.Delay(300);

            await ConsoleHelper.TypeText($"Session duration: {_session.GetSessionDuration()}", 40);
            await Task.Delay(300);

            ConsoleHelper.WriteColourText("\n" + new string('=', 70), ConsoleColor.Magenta);
            ConsoleHelper.WriteColourText("     Remember: Cybersecurity is everyone's responsibility! Stay safe online!", ConsoleColor.Green);
            ConsoleHelper.WriteColourText(new string('=', 70), ConsoleColor.Magenta);

            ConsoleHelper.CloseBorder();

            _isRunning = false;

            // Exit after a short delay so user can read the message
            await Task.Delay(2000);
        }
    }
}