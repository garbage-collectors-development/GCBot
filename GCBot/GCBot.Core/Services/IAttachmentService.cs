using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GCBot.Shared;

namespace GCBot.Core.Services
{
    public interface IAttachmentService
    {
        IEnumerable<AllowedExtension> GetAllAllowedExtensions();
        bool WhitelistExtension(string extension);
        Task<bool> VerifyAttachmentExtensions(SocketUserMessage msg);
        void BlacklistExtension(string extension);
    }
}