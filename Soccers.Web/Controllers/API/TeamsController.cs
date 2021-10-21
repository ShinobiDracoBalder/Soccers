using Microsoft.AspNetCore.Mvc;
using Soccers.Web.Data;
using Soccers.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers.API
{
    public class TeamsController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public TeamsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public IEnumerable<TeamEntity> GetTeams()
        {
            return _dataContext.Teams.OrderBy(x => x.Name).ToList();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeamEntity teamEntity = await _dataContext.Teams.FindAsync(id);

            if (teamEntity == null)
            {
                return NotFound();
            }

            return Ok(teamEntity);
        }
    }
}
