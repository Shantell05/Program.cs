 ﻿using System;
using System.Windows;
using System.Windows.Media;

namespace CybersecurityChatbotWPF.Models
{
    public class ChatMessage
    {
        public string Text { get; set; } = string.Empty;
        public bool IsUser { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Left;
        public Brush BubbleColor { get; set; } = new SolidColorBrush(Color.FromRgb(45, 45, 45));

        public string FormattedTime => Timestamp.ToString("hh:mm tt");
    }
}
