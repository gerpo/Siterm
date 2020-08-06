namespace Siterm.DatabaseInitialization.Services
{
    public interface IImporter
    {
        public int Order { get; }
        public void Execute();
    }
}