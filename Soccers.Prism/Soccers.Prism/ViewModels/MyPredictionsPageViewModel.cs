using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soccers.Prism.ViewModels
{
    public class MyPredictionsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MyPredictionsPageViewModel(INavigationService navigationService) : base(navigationService)

        {
            _navigationService = navigationService;
            Title = "My Predictions";
        }
    }
}
