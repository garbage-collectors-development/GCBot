using System;
using System.ComponentModel.DataAnnotations;

namespace GCBot.Models.Backup
{
    public class UserMessage
    {
        public DateTime DateSent { get; set; }
        public ulong SenderId { get; set; }
        public ulong ChannelId { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
    }
}
