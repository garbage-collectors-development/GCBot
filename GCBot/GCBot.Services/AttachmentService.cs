using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GCBot.Models;
using GCBot.Services.Repositories;
using GCBot.Services.Services;

namespace GCBot.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAllowedExtensionRepository _allowedExtensionRepository;

        public AttachmentService(IAllowedExtensionRepository repository)
        {
            _allowedExtensionRepository = repository;
        }

        public IEnumerable<Extension> GetAllAllowedExtensions()
        {
            return _allowedExtensionRepository.GetAll().ToList();
        }

        /// <summary>
        /// Adds the given extension to the whitelist for uploaded attachments.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns>Returns true if the extension was added, otherwise returns false if the extension exists</returns>
        public bool WhitelistExtension(string extension)
        {
            if(_allowedExtensionRepository.ExtensionExists(extension))
            {
                return false;
            }
            
            _allowedExtensionRepository.Create(new Extension() {Value = extension.Trim('.').ToLowerInvariant()});
            
            return true;
        }
        
        public void BlacklistExtension(string extension)
        {
            _allowedExtensionRepository.Delete(extension);
        }
    }
    
}
