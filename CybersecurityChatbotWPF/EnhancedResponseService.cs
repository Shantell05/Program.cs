using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF.Services
{
    public class EnhancedResponseService
    {
        public int TotalResponsesGiven { get; private set; }

        private Dictionary<string, List<string>> _keywordResponses;
        private Dictionary<string, List<string>> _randomResponses;
        private List<string> _fallbackResponses;
        private Random _random;

        public EnhancedResponseService()
        {
            TotalResponsesGiven = 0;
            _random = new Random();
            _keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            _randomResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            _fallbackResponses = new List<string>();

            InitializeKeywordResponses();
            InitializeRandomResponses();
            InitializeFallbackResponses();
        }

        private void InitializeKeywordResponses()
        {
            _keywordResponses["password"] = new List<string>
            {
                "Use at least 12 characters with uppercase, lowercase, numbers, and symbols!",
                "Never reuse passwords across different accounts - use a password manager!",
                "Consider using a passphrase like 'Coffee-Mountain-Rainbow-42' instead of a single word!",
                "Enable Two-Factor Authentication (2FA) on all accounts that support it!"
            };

            _keywordResponses["phishing"] = new List<string>
            {
                "Always check the sender's email address - scammers use fake addresses!",
                "Hover over links before clicking to see the real URL destination!",
                "Look for spelling errors and urgent language - these are major red flags!",
                "Never share personal information or passwords via email or text messages!"
            };

            _keywordResponses["privacy"] = new List<string>
            {
                "Review your privacy settings on social media platforms every few months!",
                "Use encrypted messaging apps like Signal or WhatsApp for sensitive conversations!",
                "Be careful what personal information you share publicly online!",
                "Use a VPN when using public Wi-Fi networks to encrypt your traffic!"
            };

            _keywordResponses["scam"] = new List<string>
            {
                "If something sounds too good to be true, it probably is a scam!",
                "Never send money or gift cards to someone you've only met online!",
                "Verify the identity of anyone asking for personal or financial information!",
                "Report suspected scams to the authorities!"
            };

            _keywordResponses["2fa"] = new List<string>
            {
                "Two-Factor Authentication adds an extra security layer beyond just your password!",
                "Use authenticator apps like Google Authenticator or Authy instead of SMS!",
                "Always enable 2FA on your email, banking, and social media accounts!",
                "Store backup codes in a safe place - you'll need them if you lose your phone!"
            };

            _keywordResponses["browsing"] = new List<string>
            {
                "Look for HTTPS and the padlock icon in the address bar before entering information!",
                "Keep your browser and extensions updated to the latest versions!",
                "Use ad blockers to prevent malicious ads from infecting your computer!",
                "Clear your cookies and cache regularly to protect your privacy!"
            };

            _keywordResponses["malware"] = new List<string>
            {
                "Install reputable antivirus software and keep it updated automatically!",
                "Don't download software from untrusted sources or pop-up ads!",
                "Be extremely careful with email attachments - scan them before opening!",
                "Run regular system scans to detect and remove malware infections!"
            };

            _keywordResponses["vpn"] = new List<string>
            {
                "A VPN encrypts your internet traffic and hides your IP address from trackers!",
                "Always use a VPN on public Wi-Fi networks like coffee shops or airports!",
                "Choose a paid VPN service that has a strict no-logs policy!",
                "Free VPNs often sell your data - invest in a reputable paid service!"
            };
        }

        private void InitializeRandomResponses()
        {
            _randomResponses["phishing tip"] = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Check the sender's email address carefully - scammers use addresses that look almost identical to real ones!",
                "Never click links in suspicious emails. Hover over them first to see the real destination.",
                "If an email creates urgency or threatens account closure, it's likely a phishing attempt."
            };

            _randomResponses["password tip"] = new List<string>
            {
                "Use a passphrase: combine 4 random words with numbers and symbols!",
                "Never use personal information like birthdays, pet names, or addresses in passwords.",
                "Change passwords immediately if you suspect a data breach on any service you use.",
                "Use a different password for every account to prevent credential stuffing attacks."
            };

            _randomResponses["tip"] = new List<string>
            {
                "Keep your software updated - updates contain important security patches!",
                "Backup your important files regularly using the 3-2-1 rule.",
                "Use a password manager to generate and store strong, unique passwords.",
                "Enable automatic updates on your operating system and important applications.",
                "Be skeptical of unsolicited messages, even from friends - their accounts could be compromised!"
            };
        }

        private void InitializeFallbackResponses()
        {
            _fallbackResponses = new List<string>
            {
                "I'm not sure I understand. Could you rephrase your cybersecurity question?",
                "That's outside my cybersecurity focus. Try asking about passwords, phishing, privacy, or online safety!",
                "I didn't catch that. Try asking about password safety, phishing detection, or secure browsing!",
                "Type 'help' to see all the topics I can help you with regarding cybersecurity!"
            };
        }

        public string GetResponse(string userInput, out string detectedTopic)
        {
            TotalResponsesGiven++;
            string lowerInput = userInput.Trim().ToLower();
            detectedTopic = "";

            if (IsFollowUpRequest(lowerInput))
            {
                detectedTopic = "followup";
                return GetFollowUpResponse();
            }

            foreach (var randomKey in _randomResponses.Keys)
            {
                if (lowerInput.Contains(randomKey))
                {
                    detectedTopic = randomKey;
                    return GetRandomResponse(randomKey);
                }
            }

            foreach (var keyword in _keywordResponses.Keys)
            {
                if (lowerInput.Contains(keyword.ToLower()))
                {
                    detectedTopic = keyword;
                    return GetRandomResponseForKeyword(keyword);
                }
            }

            if (lowerInput.Contains("help") || lowerInput.Contains("menu"))
            {
                detectedTopic = "help";
                return GetHelpMessage();
            }

            detectedTopic = "fallback";
            return _fallbackResponses[_random.Next(_fallbackResponses.Count)];
        }

        private bool IsFollowUpRequest(string input)
        {
            string[] followUpPhrases = {
                "another tip", "tell me more", "explain more", "more info",
                "continue", "elaborate", "what else", "more details",
                "tell me another", "give me another", "next tip"
            };

            foreach (var phrase in followUpPhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }

        private string GetFollowUpResponse()
        {
            string[] followUpResponses = {
                "Of course! Here's another important cybersecurity tip for you:",
                "I'd be happy to share more information on this topic! Here's another tip:",
                "Great question! Continuing with cybersecurity awareness, here's another important point:",
                "Absolutely! Let me give you more details on that:"
            };
            return followUpResponses[_random.Next(followUpResponses.Length)];
        }

        private string GetRandomResponseForKeyword(string keyword)
        {
            if (_keywordResponses.ContainsKey(keyword) && _keywordResponses[keyword].Count > 0)
            {
                int index = _random.Next(_keywordResponses[keyword].Count);
                return _keywordResponses[keyword][index];
            }
            return GetDefaultResponse();
        }

        private string GetRandomResponse(string responseType)
        {
            if (_randomResponses.ContainsKey(responseType) && _randomResponses[responseType].Count > 0)
            {
                int index = _random.Next(_randomResponses[responseType].Count);
                return _randomResponses[responseType][index];
            }

            if (_randomResponses.ContainsKey("tip") && _randomResponses["tip"].Count > 0)
            {
                int index = _random.Next(_randomResponses["tip"].Count);
                return _randomResponses["tip"][index];
            }

            return GetDefaultResponse();
        }

        private string GetDefaultResponse()
        {
            return "I'm here to help with cybersecurity! Try asking about passwords, phishing, privacy, 2FA, safe browsing, malware, or VPNs.";
        }

        private string GetHelpMessage()
        {
            return "I can help you with these cybersecurity topics:\n\n" +
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
                "- 'What is 2FA and why do I need it?'";
        }
    }
}