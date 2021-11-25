using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soccers.Prism.ViewModels
{
    public class MyPositionsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MyPositionsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "My Positions";
        }
    }
}
