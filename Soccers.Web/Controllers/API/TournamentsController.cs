using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccers.Web.Data;
using Soccers.Web.Data.Entities;
using Soccers.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers.API
{
    public class TournamentsController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;

        public TournamentsController(DataContext  dataContext,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
        }
        [HttpGet]
        public async Task<IActionResult> GetTournaments()
        {
            List<TournamentEntity> tournaments = await _dataContext.Tournaments
                .Include(t => t.Groups)
                .ThenInclude(g => g.GroupDetails)
                .ThenInclude(gd => gd.Team)
                .Include(t => t.Groups)
                .ThenInclude(g => g.Matches)
                .Include(t => t.Groups)
                .ThenInclude(g => g.Matches)
                .ThenInclude(m => m.Local)
                .Include(t => t.Groups)
                .ThenInclude(g => g.Matches)
                .ThenInclude(m => m.Visitor)
                .Where(t => t.IsActive)
                .ToListAsync();
            return Ok(_converterHelper.ToTournamentResponse(tournaments));
        }

        [HttpGet]
        [Route("GetTournaments2")]
        public async Task<IActionResult> GetTournaments2()
        {
            List<TournamentEntity> tournaments = await _dataContext.Tournaments
                .Where(t => t.IsActive)
                .ToListAsync();
            return Ok(tournaments);
        }
    }
}
