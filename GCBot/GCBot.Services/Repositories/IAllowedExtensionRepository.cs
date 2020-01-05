using System.Linq;
using System.Threading.Tasks;
using GCBot.Models;

namespace GCBot.Services.Repositories
{
    public interface IAllowedExtensionRepository
    {
        IQueryable<Extension> GetAll();
        Extension GetById(int id);
        void Create(Extension entity);
        void Delete(int id);
        void Delete(string extension);
        bool ExtensionExists(string extension);
        bool ExtensionExists(Extension extension);
    }
}