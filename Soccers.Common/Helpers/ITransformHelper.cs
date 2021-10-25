using Soccers.Common.Models;
using System.Collections.Generic;

namespace Soccers.Common.Helpers
{
    public interface ITransformHelper
    {
        List<Group> ToGroups(List<GroupResponse> groupResponses);
    }
}
