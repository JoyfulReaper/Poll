using PollLibrary.Models;
using System.Threading.Tasks;

namespace PollLibrary.DataAccess
{
    public interface IContextData
    {
        Task<Context> GetContext(string name);
    }
}