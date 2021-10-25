using Prism.Commands;
using Prism.Navigation;
using Soccers.Common.Models;
using Soccers.Prism.Views;
using System;

namespace Soccers.Prism.ViewModels
{
    public class TournamentItemViewModel : TournamentResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectTournamentCommand;

        public TournamentItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectTournamentCommand => _selectTournamentCommand ?? (_selectTournamentCommand =
        new DelegateCommand(SelectTournamentAsync));

        private async void SelectTournamentAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
              { "tournament", this }
            };
            //await _navigationService.NavigateAsync("GroupsPage", parameters);
            await _navigationService.NavigateAsync(nameof(MatchesPage), parameters);
        }
    }
}
