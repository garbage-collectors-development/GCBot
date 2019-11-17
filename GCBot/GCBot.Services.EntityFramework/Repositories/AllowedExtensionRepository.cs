using System;
using System.Linq;
using GCBot.Models;
using GCBot.Services.Repositories;
using AllowedExtension = GCBot.Services.EntityFramework.Entities.AllowedExtension;

namespace GCBot.Services.EntityFramework.Repositories
{
    public class AllowedExtensionRepository : IAllowedExtensionRepository
    {
        private readonly AllowedExtensionContext _context;

        public AllowedExtensionRepository(AllowedExtensionContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public IQueryable<Extension> GetAll()
        {
            return _context.AllowedExtensions.Select(at => new Extension(){Id = at.Id, Value = at.Value});
        }

        public Extension GetById(int id)
        {
            AllowedExtension allowedExtension = _context.AllowedExtensions.FirstOrDefault(at => at.Id == id);
            return allowedExtension == null ? null : new Extension(){Id = allowedExtension.Id, Value = allowedExtension.Value};
        }
        
        public void Delete(string ext)
        {
            AllowedExtension allowedExtension = _context.AllowedExtensions.FirstOrDefault(at => string.Equals(at.Value, ext, StringComparison.InvariantCultureIgnoreCase));
            if (allowedExtension != null) Delete(allowedExtension.Id);
        }

        public bool ExtensionExists(string extension)
        {
            AllowedExtension ext = _context.AllowedExtensions.FirstOrDefault(at => string.Equals(at.Value, extension, StringComparison.InvariantCultureIgnoreCase));
            return ext != null;
        }
        
        public bool ExtensionExists(Extension extension)
        {
            AllowedExtension ext = _context.AllowedExtensions.FirstOrDefault(s => s.Id == extension.Id && s.Value.Equals(extension.Value));
            return ext != null;
        }

        public void Create(Extension entity)
        {
            _context.AllowedExtensions.Add(new AllowedExtension(){Value = entity.Value});
            Console.WriteLine("Added entity");
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            AllowedExtension allowedExtension = _context.AllowedExtensions.Find(id);
            _context.AllowedExtensions.Remove(allowedExtension);
            _context.SaveChanges();
        }
    }
}