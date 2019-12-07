using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using GCBot.Models;

namespace GCBot.Infrastructure.Extensions
{
    public static class SocketMessageExtension
    {
        
        public static bool ContainsIllegalExtension(this SocketMessage msg, IEnumerable<Extension> allowedExtensions)
        {
            return !msg.Attachments.All(a => a.IsAllowed(allowedExtensions));
        }
    }
}