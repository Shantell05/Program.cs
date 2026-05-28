using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CybersecurityChatbotWPF.Models;
using CybersecurityChatbotWPF.Services;

namespace CybersecurityChatbotWPF
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotService _chatbotService;
        private readonly AudioService _audioService;
        private ObservableCollection<ChatMessage> _messages;

        public MainWindow()
        {
            InitializeComponent();

            LoadAsciiArt();

            _chatbotService = new ChatbotService();
            _audioService = new AudioService();
            _messages = new ObservableCollection<ChatMessage>();

            MessagesItemsControl.ItemsSource = _messages;

            _audioService.PlayVoiceGreeting();

            AddBotMessage("Hello! Welcome to the Cybersecurity Awareness Bot!");
            AddBotMessage("I'm here to help you stay safe online and protect your digital life.");
            AddBotMessage("Please enter your name to get started!");
        }

        private void LoadAsciiArt()
        {
            // Smaller ASCII art that fits in the window
            string asciiArt = @"
╔════════════════════════════════════════════════════════════════════════════╗
║                     CYBERSECURITY AWARENESS BOT                            ║
╠════════════════════════════════════════════════════════════════════════════╣
║  ██████╗██╗   ██╗██████╗ ███████╗██████╗     █████╗ ██╗    ██╗ █████╗ ██████╗║
║ ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗   ██╔══██╗██║    ██║██╔══██╗██╔══██╗║
║ ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝   ███████║██║ █╗ ██║███████║██████╔╝║
║ ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗   ██╔══██║██║███╗██║██╔══██║██╔══██╗║
║ ╚██████╗   ██║   ██████╔╝███████╗██║  ██║   ██║  ██║╚███╔███╔╝██║  ██║██║  ██║║
║  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝   ╚═╝  ╚═╝ ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝║
╠════════════════════════════════════════════════════════════════════════════╣
║                    Your Digital Safety Companion                          ║
╚════════════════════════════════════════════════════════════════════════════╝";

            AsciiArtTextBlock.Text = asciiArt;
            AsciiArtTextBlock.FontSize = 9;
            AsciiArtTextBlock.FontFamily = new FontFamily("Consolas");
        }

        private void AddUserMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                _messages.Add(new ChatMessage
                {
                    Text = message,
                    IsUser = true,
                    Timestamp = DateTime.Now,
                    Alignment = HorizontalAlignment.Right,
                    BubbleColor = new SolidColorBrush(Color.FromRgb(26, 95, 122))
                });
                ScrollToBottom();
            });
        }

        private void AddBotMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                _messages.Add(new ChatMessage
                {
                    Text = message,
                    IsUser = false,
                    Timestamp = DateTime.Now,
                    Alignment = HorizontalAlignment.Left,
                    BubbleColor = new SolidColorBrush(Color.FromRgb(45, 45, 45))
                });
                ScrollToBottom();
            });
        }

        private void ScrollToBottom()
        {
            Dispatcher.Invoke(() =>
            {
                ChatScrollViewer.UpdateLayout();
                ChatScrollViewer.ScrollToBottom();
            });
        }

        private async Task ShowTypingIndicator(bool show)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                TypingIndicator.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            });
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await ProcessUserInput();
        }

        private async void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                await ProcessUserInput();
            }
        }

        private async void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowTypingIndicator(true);
            await Task.Delay(300);

            string stats = _chatbotService.GetStatistics();
            AddBotMessage(stats);

            if (_chatbotService.Memory.HasName)
            {
                AddBotMessage($"I remember your name is {_chatbotService.Memory.RecallName()}!");
            }

            if (_chatbotService.Memory.HasPreferredTopic)
            {
                AddBotMessage($"I've noticed you're interested in {_chatbotService.Memory.RecallPreferredTopic()}. Great focus area!");
            }

            await ShowTypingIndicator(false);
        }

        private async void SaveNameButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a valid name (at least 2 characters).", "Input Required",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (name.Length < 2)
            {
                MessageBox.Show("Name must be at least 2 characters long.", "Invalid Name",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (name.Length > 30)
            {
                MessageBox.Show("Name must be less than 30 characters.", "Invalid Name",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _chatbotService.SetUserName(name);
            NameInputPanel.Visibility = Visibility.Collapsed;

            InputTextBox.IsEnabled = true;
            SendButton.IsEnabled = true;
            HelpButton.IsEnabled = true;

            InputTextBox.Focus();

            await ShowTypingIndicator(true);
            await Task.Delay(500);

            string welcome = _chatbotService.GetPersonalizedWelcome();
            AddBotMessage(welcome);

            await Task.Delay(300);
            AddBotMessage("You can ask me about passwords, phishing, privacy, 2FA, safe browsing, malware, or VPNs!");
            AddBotMessage("Type any cybersecurity question - I'll remember what you're interested in!");

            await ShowTypingIndicator(false);
        }

        private async void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowTypingIndicator(true);
            await Task.Delay(300);

            AddBotMessage("I can help you with these cybersecurity topics:\n\n" +
                "Password Safety - Ask about 'password' or 'password tips'\n" +
                "Phishing Detection - Ask about 'phishing' or 'scam'\n" +
                "Privacy Protection - Ask about 'privacy'\n" +
                "Two-Factor Authentication - Ask about '2fa'\n" +
                "Safe Browsing - Ask about 'browsing'\n" +
                "Malware Protection - Ask about 'malware'\n" +
                "VPN Security - Ask about 'vpn'\n\n" +
                "Try these phrases:\n" +
                "- 'Give me a phishing tip'\n" +
                "- 'Tell me another tip'\n" +
                "- 'How do I create a strong password?'\n" +
                "- 'What is 2FA and why do I need it?'\n\n" +
                "I remember your interests! You can ask for 'another tip' anytime!");

            await ShowTypingIndicator(false);
        }

        private async Task ProcessUserInput()
        {
            string userInput = InputTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(userInput))
                return;

            InputTextBox.IsEnabled = false;
            SendButton.IsEnabled = false;

            AddUserMessage(userInput);
            InputTextBox.Clear();

            await ShowTypingIndicator(true);

            string response = await _chatbotService.ProcessUserInput(userInput);

            await ShowTypingIndicator(false);

            AddBotMessage(response);
            UpdateStatusFromSentiment(userInput);

            InputTextBox.IsEnabled = true;
            SendButton.IsEnabled = true;
            InputTextBox.Focus();
        }

        private void UpdateStatusFromSentiment(string userInput)
        {
            string lowerInput = userInput.ToLower();

            if (lowerInput.Contains("worried") || lowerInput.Contains("scared") || lowerInput.Contains("nervous"))
            {
                StatusText.Text = "User seems worried - Providing reassurance...";
                StatusText.Foreground = new SolidColorBrush(Colors.Orange);
                SentimentEmoji.Text = "😟";
                SentimentText.Text = "Worried";
                SentimentText.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (lowerInput.Contains("frustrated") || lowerInput.Contains("confused"))
            {
                StatusText.Text = "User frustrated - Offering simplified help...";
                StatusText.Foreground = new SolidColorBrush(Colors.Red);
                SentimentEmoji.Text = "😤";
                SentimentText.Text = "Frustrated";
                SentimentText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (lowerInput.Contains("curious") || lowerInput.Contains("interested") || lowerInput.Contains("tell me"))
            {
                StatusText.Text = "User curious - Providing educational content...";
                StatusText.Foreground = new SolidColorBrush(Colors.LightBlue);
                SentimentEmoji.Text = "🤔";
                SentimentText.Text = "Curious";
                SentimentText.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
            else if (lowerInput.Contains("thanks") || lowerInput.Contains("good") || lowerInput.Contains("great"))
            {
                StatusText.Text = "Positive interaction - User satisfied...";
                StatusText.Foreground = new SolidColorBrush(Colors.LightGreen);
                SentimentEmoji.Text = "😊";
                SentimentText.Text = "Positive";
                SentimentText.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                StatusText.Text = "Ready to help with cybersecurity!";
                StatusText.Foreground = new SolidColorBrush(Colors.LightGreen);
                SentimentEmoji.Text = "😐";
                SentimentText.Text = "Neutral";
                SentimentText.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}