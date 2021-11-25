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
    public class GroupsPageViewModel : ViewModelBase
    {
        private readonly ITransformHelper _transformHelper;
        private TournamentResponse _tournament;
        private List<Group> _groups;
        public GroupsPageViewModel(INavigationService navigationService, 
            ITransformHelper transformHelper) : base(navigationService)
        {
            Title = Languages.Groups;
            _transformHelper = transformHelper;
        }
        public TournamentResponse Tournament
        {
            get => _tournament;
            set => SetProperty(ref _tournament, value);
        }
        public List<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("tournament"))
            {
                Tournament = parameters.GetValue<TournamentResponse>("tournament");
                //Title = Tournament.Name;
                Groups = _transformHelper.ToGroups(_tournament.Groups);
            }
        }
    }
}
