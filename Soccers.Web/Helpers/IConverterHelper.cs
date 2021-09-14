using Soccers.Web.Data.Entities;
using Soccers.Web.Models;

namespace Soccers.Web.Helpers
{
    public interface IConverterHelper
    {
        TeamEntity ToTeamEntity(TeamViewModel model, string path, bool isNew);
        TeamViewModel ToTeamViewModel(TeamEntity teamEntity);
    }
}
