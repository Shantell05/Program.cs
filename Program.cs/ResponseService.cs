using System;
using System.Collections.Generic;

namespace CybersecurityChatbot.Services
{
    /// <summary>
    /// Manages all chatbot responses
    /// </summary>
    public class ResponseService
    {
        // Automatic property
        public int TotalResponsesGiven { get; private set; }

        private Dictionary<string, string> _responses;
        private List<string> _fallbackResponses;
        private Random _random;

        public ResponseService()
        {
            TotalResponsesGiven = 0;
            _random = new Random();
            InitialiseResponses();
            InitialiseFallbackResponses();
        }

        private void InitialiseResponses()
        {
            _responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Greeting responses
                { "hello", "Hello! How can I help you with cybersecurity today? 🔒" },
                { "hi", "Hi there! Ready to learn about staying safe online? 🛡️" },
                { "hey", "Hey! Let's talk about keeping your digital life secure! 💻" },
                { "good morning", "Good morning! Start your day with cybersecurity awareness! ☀️" },
                { "good afternoon", "Good afternoon! Ready to boost your online safety? 📱" },
                { "good evening", "Good evening! Perfect time to review your security habits! 🌙" },
                
                // "How are you" responses
                { "how are you", "I'm functioning perfectly and ready to help you stay secure online! My security protocols are all green! ✅" },
                { "how are you doing", "I'm doing great! Just processed another security update. Thanks for asking! 🤖" },
                { "how's it going", "Going well! Always here to help with cybersecurity questions. How can I assist you? 🚀" },
                { "how are things", "Things are secure on my end! Ready to help you with online safety! 🛡️" },
                
                // Purpose responses
                { "what's your purpose", "My purpose is to educate and protect users like you from online threats. I provide cybersecurity awareness training and best practices! 🎯" },
                { "what can you do", "I can help you with:\n   • Creating strong passwords\n   • Identifying phishing emails\n   • Safe browsing habits\n   • Two-factor authentication\n   • Malware protection\n   • VPN usage\n   • Online privacy tips" },
                { "what can i ask", "You can ask me about:\n   • Password safety\n   • Phishing detection\n   • Safe browsing\n   • 2FA/MFA\n   • Malware protection\n   • VPN services\n   • Data privacy\n   • Social engineering" },
                { "who are you", "I'm your Cybersecurity Awareness Bot - a digital assistant dedicated to helping you navigate the online world safely! 🤖" },
                { "what do you do", "I provide cybersecurity education and best practices to help you stay safe online! Ask me anything about digital security! 🔐" },
                
                // Password-related responses
                { "password", "🔐 Strong Password Tips:\n   • Use at least 12-16 characters\n   • Mix uppercase, lowercase, numbers, and symbols\n   • Never reuse passwords across sites\n   • Use a password manager\n   • Enable 2FA whenever possible" },
                { "strong password", "A strong password should be long (12+ chars), complex (mix of character types), and unique. Try using passphrases like 'PurpleTiger$Jumps42!' 🦁" },
                { "password manager", "Password managers like Bitwarden, LastPass, or 1Password securely store and generate strong passwords. Never store passwords in your browser! 🔑" },
                { "create password", "Use a passphrase: combine 4+ random words with numbers and symbols. Example: 'Coffee!Mountain42River$Sun' - easy to remember, hard to crack! 💪" },
                
                // Phishing responses
                { "phishing", "🎣 Phishing Red Flags:\n   • Urgent or threatening language\n   • Suspicious sender email addresses\n   • Spelling/grammar errors\n   • Unexpected attachments\n   • Requests for personal info\n   • Too-good-to-be-true offers" },
                { "phishing email", "Never click links in suspicious emails! Hover over links first to see the real URL. When in doubt, go directly to the website rather than clicking email links. 📧" },
                { "phishing attack", "Phishers create fake websites that look real. Always check the URL carefully - look for misspellings like 'Amaz0n.com' instead of 'Amazon.com'! 🕸️" },
                
                // Safe browsing responses
                { "safe browsing", "🌐 Safe Browsing Checklist:\n   ✓ Look for HTTPS (padlock icon)\n   ✓ Keep browser updated\n   ✓ Use ad blockers\n   ✓ Don't save passwords in browser\n   ✓ Clear cookies regularly\n   ✓ Use private/incognito mode for sensitive browsing" },
                { "https", "HTTPS encrypts data between you and websites. Always check for the padlock icon in your address bar before entering passwords or payment info! 🔒" },
                { "public wifi", "Public Wi-Fi is risky! Always use a VPN, avoid accessing sensitive accounts, and enable 'HTTPS Only' mode in your browser. Never do banking on public WiFi! 📱" },
                
                // 2FA responses
                { "2fa", "Two-Factor Authentication adds a second verification step. Options include:\n   • SMS codes (least secure)\n   • Authenticator apps (Google Authenticator, Authy)\n   • Hardware keys (YubiKey)\n   • Biometrics (fingerprint, face ID)" },
                { "two factor", "2FA is essential! Even if someone steals your password, they can't access your account without the second factor. Enable it everywhere you can! 🔐" },
                { "mfa", "Multi-Factor Authentication (MFA) uses multiple verification methods: something you KNOW (password), HAVE (phone), or ARE (fingerprint)." },
                { "authenticator", "Authenticator apps are more secure than SMS! Try Google Authenticator, Microsoft Authenticator, or Authy for better protection! 📱" },
                
                // Malware responses
                { "malware", "🦠 Malware Protection:\n   • Install reputable antivirus software\n   • Keep everything updated\n   • Don't download from untrusted sources\n   • Be careful with email attachments\n   • Use ad blockers\n   • Regular system scans" },
                { "virus", "Viruses spread by attaching to legitimate files. Always scan downloads before opening, and keep your antivirus definitions updated! 🛡️" },
                { "ransomware", "Ransomware encrypts your files and demands payment. Prevention is key: regular backups, software updates, and never click suspicious links! 💀" },
                { "antivirus", "Good antivirus options include Windows Defender (built-in), Bitdefender, Kaspersky, or Malwarebytes. Keep them updated and run regular scans! 🛡️" },
                
                // VPN responses
                { "vpn", "🔒 VPN Benefits:\n   • Encrypts your internet traffic\n   • Hides your IP address\n   • Protects on public Wi-Fi\n   • Bypasses geo-restrictions\n   • Prevents ISP tracking\n\nChoose a paid VPN that doesn't keep logs (NordVPN, ExpressVPN, ProtonVPN)." },
                { "vpn service", "Free VPNs often sell your data. Invest in a reputable paid VPN for real privacy protection! 🛡️" },
                
                // General cybersecurity
                { "cybersecurity", "Cybersecurity is protecting systems, networks, and data from digital attacks. Key practices: strong passwords, 2FA, software updates, backups, and being cautious online! 🛡️" },
                { "online safety", "Online safety tips:\n   1. Use unique passwords\n   2. Enable 2FA\n   3. Keep software updated\n   4. Think before clicking\n   5. Backup your data\n   6. Use a VPN on public Wi-Fi" },
                { "data privacy", "Protect your data by:\n   • Minimising what you share online\n   • Reviewing privacy settings\n   • Using encrypted messaging (Signal, WhatsApp)\n   • Being careful with permissions\n   • Reading privacy policies" },
                { "social engineering", "Social engineering manipulates people into giving up info. Never share passwords or codes over phone/email, even if someone claims to be from IT support! 🎭" },
                { "backup", "Follow the 3-2-1 backup rule: 3 copies of data, 2 different media types, 1 off-site backup. Use cloud + external drive! 💾" },
                { "update", "Software updates contain security patches. Enable automatic updates on your OS, browsers, and apps! Never delay updates! 🔄" }
            };
        }

        private void InitialiseFallbackResponses()
        {
            _fallbackResponses = new List<string>
            {
                "I didn't quite understand that. Could you rephrase your question about cybersecurity? 🤔",
                "I'm not sure about that. Try asking me about passwords, phishing, safe browsing, or 2FA! 💡",
                "Let me focus on cybersecurity. Would you like to learn about creating strong passwords or identifying phishing emails? 🎯",
                "Hmm, I specialise in cybersecurity topics. Ask me about online safety, malware protection, or data privacy! 🔒",
                "I didn't catch that. Type 'help' to see all the topics I can help you with! 📚",
                "Not sure about that one. Want to know how to create an unbreakable password instead? 🔐",
                "I'm designed to answer cybersecurity questions. Try asking about password safety or how to spot phishing emails! 🛡️",
                "That's outside my cybersecurity focus. Ask me about protecting yourself online! 💻"
            };
        }

        /// <summary>
        /// Gets a response based on user input using string manipulation
        /// </summary>
        public string GetResponse(string userInput)
        {
            TotalResponsesGiven++;

            // String manipulation: convert to lower and trim
            string processedInput = userInput.Trim().ToLower();

            // Check for matches using string manipulation
            foreach (var keyword in _responses.Keys)
            {
                // String manipulation: Contains check with case-insensitive comparison
                if (processedInput.Contains(keyword.ToLower()))
                {
                    return _responses[keyword];
                }
            }

            // Return a random fallback response
            return _fallbackResponses[_random.Next(_fallbackResponses.Count)];
        }
    }
}