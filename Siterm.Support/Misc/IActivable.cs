using System.Threading.Tasks;

namespace Siterm.Support.Misc
{
    public interface IActivable
    {
        Task ActivateAsync(object parameter);
    }
}