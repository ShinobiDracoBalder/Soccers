using Prism.Navigation;
using Soccers.Common.Models;
using Soccers.Common.Services;
using Soccers.Prism.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Prism.ViewModels
{
    public class TournamentsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<TournamentItemViewModel> _tournaments;
        private bool _isRunning;
        private bool _isEnabled;
        public TournamentsPageViewModel(INavigationService navigationService,
               IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Soccer Tourmanments";
            IsEnabled = true;
            LoadTournamentsAsync();
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public List<TournamentItemViewModel> Tournaments
        {
            get => _tournaments;
            set => SetProperty(ref _tournaments, value);
        }
        //public override async void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    await LoadTournamentsAsync();
        //}

        private async void LoadTournamentsAsync()
        {
            IsRunning = true;
            IsEnabled = false;
            string url = App.Current.Resources["UrlAPI"].ToString();

            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            else
            {
                Response response = await _apiService.GetListAsync<TournamentResponse>(
                url,
                "/api",
                "/Tournaments");

                IsRunning = false;
                IsEnabled = true;
                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
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
}
