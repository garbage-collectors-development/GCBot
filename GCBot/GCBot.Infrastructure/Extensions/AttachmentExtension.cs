using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord;
using GCBot.Models;

namespace GCBot.Infrastructure.Extensions
{
    public static class AttachmentExtension
    {
        public static bool IsAllowed(this Attachment attachment, IEnumerable<Extension> allowedExtensions)
        {
            Extension allowedExtension = allowedExtensions?.FirstOrDefault(extension =>
                extension.Value.Trim('.').Equals(attachment.GetExtension(), StringComparison.CurrentCultureIgnoreCase));
            
            return  allowedExtension != null;
        }
        
        private static string GetExtension(this IAttachment attachment)
        {
            return Path.GetExtension(attachment.Url).Trim('.');
        }
    }
}