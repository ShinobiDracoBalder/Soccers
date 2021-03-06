using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Soccers.Common.Helpers;
using Soccers.Common.Models;
using Soccers.Prism.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soccers.Prism.ViewModels
{
    public class MatchesPageViewModel : ViewModelBase
    {
        private TournamentResponse _tournament;
        private List<MatchResponse> _matches;
        public MatchesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Open;
            LoadMatches();
        }

        public List<MatchResponse> Matches
        {
            get => _matches;
            set => SetProperty(ref _matches, value);
        }

        private void LoadMatches()
        {
            _tournament = JsonConvert.DeserializeObject<TournamentResponse>(Settings.Tournament);
            List<MatchResponse> matches = new List<MatchResponse>();
            foreach (GroupResponse group in _tournament.Groups)
            {
                matches.AddRange(group.Matches);
            }

            Matches = matches.Where(m => !m.IsClosed).OrderBy(m => m.Date).ToList();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _tournament = parameters.GetValue<TournamentResponse>("tournament");
            Title = _tournament.Name;
            List<MatchResponse> matches = new List<MatchResponse>();
            foreach (GroupResponse group in _tournament.Groups)
            {
                matches.AddRange(group.Matches);
            }
            Matches = matches.OrderBy(m => m.Date).ToList();
        }
    }
}
