using System.Threading.Tasks;

namespace Soccers.Web.Helpers
{
    public interface IMatchHelper
    {
        Task CloseMatchAsync(int matchId, int goalsLocal, int goalsVisitor);
    }
}
