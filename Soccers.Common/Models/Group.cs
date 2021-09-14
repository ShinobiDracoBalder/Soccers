using System.Collections.Generic;

namespace Soccers.Common.Models
{
    public class Group : List<GroupDetailResponse>
    {
        public string Name { get; set; }

        public List<GroupDetailResponse> Teams => this;
    }
}
