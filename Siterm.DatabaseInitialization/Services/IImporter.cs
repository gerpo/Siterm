using System.Threading.Tasks;
namespace Siterm.DatabaseInitialization.Services
{
    public interface IImporter
    {
        public int Order { get; }
        public Task Execute();
    }
}