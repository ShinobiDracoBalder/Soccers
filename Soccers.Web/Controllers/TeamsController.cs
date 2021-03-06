using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccers.Web.Data;
using Soccers.Web.Data.Entities;
using Soccers.Web.Data.Repositories;
using Soccers.Web.Helpers;
using Soccers.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeamsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGenericRepository<TeamEntity> _genericRepository;

        public TeamsController(DataContext dataContext
            , IImageHelper imageHelper, IConverterHelper converterHelper, IGenericRepository<TeamEntity> genericRepository)
        {
            _dataContext = dataContext;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _genericRepository = genericRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _genericRepository.GetAllAsync());
            //return View(await _dataContext.Teams.OrderBy(t => t.Name).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            //TeamEntity teamEntity = await _dataContext.Teams
            //    .FirstOrDefaultAsync(m => m.Id == id);
            TeamEntity teamEntity = await _genericRepository.GetAsync(id.Value);
            if (teamEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            return View(teamEntity);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            //TeamEntity teamEntity = await _dataContext.Teams
            //    .FirstOrDefaultAsync(m => m.Id == id);
            TeamEntity teamEntity = await _genericRepository.GetAsync(id.Value);
            if (teamEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            //_dataContext.Teams.Remove(teamEntity);
            //await _dataContext.SaveChangesAsync();
            var reponse = await _genericRepository.DeleteAsync(teamEntity);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (model.LogoFile != null)
                {
                    //path = await _blobHelper.UploadBlobAsync(model.LogoFile, "teams");
                    path = await _imageHelper.UploadImageAsync(model.LogoFile, "Teams");
                }

                TeamEntity team = _converterHelper.ToTeamEntity(model, path, true);
                _dataContext.Add(team);

                try
                {
                    //await _dataContext.SaveChangesAsync();
                   await _genericRepository.CreateAsync(team);
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

            TeamEntity teamEntity = await _dataContext.Teams.FindAsync(id);
            if (teamEntity == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            TeamViewModel model = _converterHelper.ToTeamViewModel(teamEntity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    string path = model.LogoPath;

                    if (model.LogoFile != null)
                    {
                        //path = await _blobHelper.UploadBlobAsync(model.LogoFile, "teams");
                        path = await _imageHelper.UploadImageAsync(model.LogoFile, "Teams");
                    }

                    TeamEntity team = _converterHelper.ToTeamEntity(model, path, false);
                    _dataContext.Update(team);

                    try
                    {
                        //await _dataContext.SaveChangesAsync();
                       await _genericRepository.UpdateAsync(team);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException dbUpdateException)
                    {
                        if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, $"Ya existe este el equipo:{team.Name}.");
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
    }
}
