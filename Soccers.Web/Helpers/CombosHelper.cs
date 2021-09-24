using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Soccers.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace Soccers.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IEnumerable<SelectListItem> GetComboTeams()
        {
            var list = _dataContext.Teams.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a team...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboTeams(int id)
        {
            var list = _dataContext.GroupDetails
                .Include(gd => gd.Team)
                .Where(gd => gd.Group.Id == id)
                .Select(gd => new SelectListItem
                {
                    Text = gd.Team.Name,
                    Value = $"{gd.Team.Id}"
                })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a team...]",
                Value = "0"
            });

            return list;
        }
    }
}
