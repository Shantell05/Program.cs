using System;

namespace CybersecurityChatbot.Models
{
    /// <summary>
    /// Represents a user session with the chatbot
    /// Uses automatic properties for clean, concise code
    /// </summary>
    public class UserSession
    {
        // Automatic properties - these are a key requirement for the assignment
        public string UserName { get; set; }
        public DateTime SessionStartTime { get; set; }
        public int QuestionsAsked { get; set; }
        public bool IsActive { get; set; }

        // Automatic property with private set (read-only outside class)
        public string SessionId { get; private set; }

        /// <summary>
        /// Constructor initialises a new user session
        /// </summary>
        public UserSession()
        {
            SessionStartTime = DateTime.Now;
            IsActive = true;
            QuestionsAsked = 0;
            SessionId = Guid.NewGuid().ToString().Substring(0, 8);
        }

        /// <summary>
        /// Returns formatted session duration using string manipulation
        /// </summary>
        public string GetSessionDuration()
        {
            TimeSpan duration = DateTime.Now - SessionStartTime;
            // String manipulation for formatting with proper pluralisation
            string minuteText = duration.Minutes == 1 ? "minute" : "minutes";
            string secondText = duration.Seconds == 1 ? "second" : "seconds";
            return $"{duration.Minutes} {minuteText} and {duration.Seconds} {secondText}";
        }

        /// <summary>
        /// Increments the question counter
        /// </summary>
        public void IncrementQuestionCount()
        {
            QuestionsAsked++;
        }
    }
} 