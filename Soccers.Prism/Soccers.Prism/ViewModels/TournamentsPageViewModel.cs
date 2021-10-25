using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Soccers.Common.Models;
using Soccers.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soccers.Prism.ViewModels
{
    public class TournamentsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<TournamentItemViewModel> _tournaments;


        public TournamentsPageViewModel(INavigationService navigationService,
               IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Tourmanments";
            LoadTournamentsAsync();
        }
        
        public List<TournamentItemViewModel> Tournaments
        {
            get => _tournaments;
            set => SetProperty(ref _tournaments, value);
        }

        private async void LoadTournamentsAsync()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<TournamentResponse>(
            url,
            "/api",
            "/Tournaments");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                "Error",
                response.Message,
                "Accept");
                return;
            }
           
            List<TournamentResponse> list = (List<TournamentResponse>)response.Result;
            Tournaments = list.Select(t => new TournamentItemViewModel(_navigationService)
            {
                EndDate = t.EndDate,
                Groups = t.Groups,
                Id = t.Id,
                IsActive = t.IsActive,
                LogoPath = t.LogoPath,
                Name = t.Name,
                StartDate = t.StartDate
            }).ToList();
        }
    }
}
