using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccers.Web.Data;
using Soccers.Web.Helpers;
using Soccers.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TournamentsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IMatchHelper _matchHelper;

        public TournamentsController(DataContext dataContext,
            IConverterHelper converterHelper,
            ICombosHelper combosHelper, IImageHelper imageHelper,
            IMatchHelper matchHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
            _imageHelper = imageHelper;
            _matchHelper = matchHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext
            .Tournaments
            .Include(t => t.Groups)
            .OrderBy(t => t.StartDate)
            .ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TournamentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (model.LogoFile != null)
                {
                    //path = await _blobHelper.UploadBlobAsync(model.LogoFile, "tournaments");
                    path = await _imageHelper.UploadImageAsync(model.LogoFile, "Tournaments");
                }

                try
                {
                    var tournament = _converterHelper.ToTournamentEntity(model, path, true);
                    _dataContext.Add(tournament);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.InnerException.Message);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var tournamentEntity = await _dataContext.Tournaments.FindAsync(id);
            if (tournamentEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = _converterHelper.ToTournamentViewModel(tournamentEntity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TournamentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    var path = model.LogoPath;

                    if (model.LogoFile != null)
                    {
                        //path = await _blobHelper.UploadBlobAsync(model.LogoFile, "tournaments");
                        path = await _imageHelper.UploadImageAsync(model.LogoFile, "Tournaments");
                    }

                    try
                    {
                        var tournamentEntity = _converterHelper.ToTournamentEntity(model, path, false);
                        _dataContext.Update(tournamentEntity);
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException dbUpdateException)
                    {
                        if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        }
                    }
                    catch (Exception exception)
                    {
                        ModelState.AddModelError(string.Empty, exception.InnerException.Message);
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var tournamentEntity = await _dataContext.Tournaments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournamentEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            try
            {
                _dataContext.Tournaments.Remove(tournamentEntity);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.InnerException.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var tournamentEntity = await _dataContext.Tournaments
                .Include(t => t.Groups)
                .ThenInclude(t => t.Matches)
                .ThenInclude(t => t.Local)
                .Include(t => t.Groups)
                .ThenInclude(t => t.Matches)
                .ThenInclude(t => t.Visitor)
                .Include(t => t.Groups)
                .ThenInclude(t => t.GroupDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournamentEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            return View(tournamentEntity);
        }

        public async Task<IActionResult> AddGroup(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var tournamentEntity = await _dataContext.Tournaments.FindAsync(id);
            if (tournamentEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = new GroupViewModel
            {
                Tournament = tournamentEntity,
                TournamentId = tournamentEntity.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var groupEntity = await _converterHelper.ToGroupEntityAsync(model, true);
                try
                {
                    _dataContext.Add(groupEntity);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{model.TournamentId}");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.InnerException.Message);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var groupEntity = await _dataContext.Groups
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (groupEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = _converterHelper.ToGroupViewModel(groupEntity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var groupEntity = await _converterHelper.ToGroupEntityAsync(model, false);
                    _dataContext.Update(groupEntity);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{model.TournamentId}");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.InnerException.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            var groupEntity = await _dataContext.Groups
               .Include(g => g.Tournament)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (groupEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            try{
               
                _dataContext.Groups.Remove(groupEntity);
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.InnerException.Message);
            }
            return RedirectToAction($"{nameof(Details)}/{groupEntity.Tournament.Id}");
        }

        //[HttpGet("Something")]
        [HttpGet]
        public async Task<IActionResult> DetailsGroup(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var groupEntity = await _dataContext.Groups
                .Include(g => g.Matches)
                .ThenInclude(g => g.Local)
                .Include(g => g.Matches)
                .ThenInclude(g => g.Visitor)
                .Include(g => g.Tournament)
                .Include(g => g.GroupDetails)
                .ThenInclude(gd => gd.Team)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (groupEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            return View(groupEntity);
        }

        public async Task<IActionResult> AddGroupDetail(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var groupEntity = await _dataContext.Groups.FindAsync(id);
            if (groupEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = new GroupDetailViewModel
            {
                Group = groupEntity,
                GroupId = groupEntity.Id,
                Teams = _combosHelper.GetComboTeams()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGroupDetail(GroupDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var groupDetailEntity = await _converterHelper.ToGroupDetailEntityAsync(model, true);
                    _dataContext.Add(groupDetailEntity);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction($"{nameof(DetailsGroup)}/{model.GroupId}");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                    model.Teams = _combosHelper.GetComboTeams();
                    return View(model);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.InnerException.Message);
                    model.Teams = _combosHelper.GetComboTeams();
                    return View(model);
                }
            }
            model.Group = await _dataContext.Groups.FindAsync(model.GroupId);
            model.Teams = _combosHelper.GetComboTeams();
            return View(model);
        }

        public async Task<IActionResult> AddMatch(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var groupEntity = await _dataContext.Groups.FindAsync(id);
            if (groupEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = new MatchViewModel
            {
                Date = DateTime.Now,
                Group = groupEntity,
                GroupId = groupEntity.Id,
                Teams = _combosHelper.GetComboTeams(groupEntity.Id)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMatch(MatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.LocalId != model.VisitorId)
                {
                    try
                    {
                        var matchEntity = await _converterHelper.ToMatchEntityAsync(model, true);
                        _dataContext.Add(matchEntity);
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction($"{nameof(DetailsGroup)}/{model.GroupId}");
                    }
                    catch (DbUpdateException dbUpdateException)
                    {
                        if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        }
                    }
                    catch (Exception exception)
                    {
                        ModelState.AddModelError(string.Empty, exception.InnerException.Message);
                    }
                    model.Group = await _dataContext.Groups.FindAsync(model.GroupId);
                    model.Teams = _combosHelper.GetComboTeams(model.GroupId);
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "The local and vistitor must be differents teams.");
            }

            model.Group = await _dataContext.Groups.FindAsync(model.GroupId);
            model.Teams = _combosHelper.GetComboTeams(model.GroupId);
            return View(model);
        }

        public async Task<IActionResult> EditGroupDetail(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var groupDetailEntity = await _dataContext.GroupDetails
                .Include(gd => gd.Group)
                .Include(gd => gd.Team)
                .FirstOrDefaultAsync(gd => gd.Id == id);
            if (groupDetailEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = _converterHelper.ToGroupDetailViewModel(groupDetailEntity);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroupDetail(GroupDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var groupDetailEntity = await _converterHelper.ToGroupDetailEntityAsync(model, false);
                _dataContext.Update(groupDetailEntity);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsGroup)}/{model.GroupId}");
            }

            return View(model);
        }
        public async Task<IActionResult> EditMatch(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var matchEntity = await _dataContext.Matches
                .Include(m => m.Group)
                .Include(m => m.Local)
                .Include(m => m.Visitor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matchEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = _converterHelper.ToMatchViewModel(matchEntity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMatch(MatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var matchEntity = await _converterHelper.ToMatchEntityAsync(model, false);
                _dataContext.Update(matchEntity);
                await _dataContext.SaveChangesAsync();
                //return RedirectToAction($"{nameof(DetailsGroup)}/{model.GroupId}");
                return RedirectToAction("DetailsGroup", new { id = model.GroupId });
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteGroupDetail(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var groupDetailEntity = await _dataContext.GroupDetails
                .Include(gd => gd.Group)
                .FirstOrDefaultAsync(gd => gd.Id == id);
            if (groupDetailEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            _dataContext.GroupDetails.Remove(groupDetailEntity);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsGroup)}/{groupDetailEntity.Group.Id}");
        }

        public async Task<IActionResult> DeleteMatch(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var matchEntity = await _dataContext.Matches
                .Include(m => m.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matchEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            _dataContext.Matches.Remove(matchEntity);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsGroup)}/{matchEntity.Group.Id}");
        }

        public async Task<IActionResult> CloseMatch(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var matchEntity = await _dataContext.Matches
                .Include(m => m.Group)
                .Include(m => m.Local)
                .Include(m => m.Visitor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matchEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = new CloseMatchViewModel
            {
                Group = matchEntity.Group,
                GroupId = matchEntity.Group.Id,
                Local = matchEntity.Local,
                LocalId = matchEntity.Local.Id,
                MatchId = matchEntity.Id,
                Visitor = matchEntity.Visitor,
                VisitorId = matchEntity.Visitor.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseMatch(CloseMatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _matchHelper.CloseMatchAsync(model.MatchId, model.GoalsLocal.Value, model.GoalsVisitor.Value);
                //return RedirectToAction($"{nameof(DetailsGroup)}/{model.GroupId}");
                return RedirectToAction("DetailsGroup", new { id = model.GroupId });
            }

            model.Group = await _dataContext.Groups.FindAsync(model.GroupId);
            model.Local = await _dataContext.Teams.FindAsync(model.LocalId);
            model.Visitor = await _dataContext.Teams.FindAsync(model.VisitorId);
            return View(model);
        }
    }
}
