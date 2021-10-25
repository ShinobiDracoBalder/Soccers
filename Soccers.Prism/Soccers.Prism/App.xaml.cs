using Prism;
using Prism.Ioc;
using Soccers.Common.Helpers;
using Soccers.Common.Services;
using Soccers.Prism.ViewModels;
using Soccers.Prism.Views;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace Soccers.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/TournamentsPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<ITransformHelper, TransformHelper>();
            containerRegistry.RegisterForNavigation<TournamentsPage, TournamentsPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupsPage, GroupsPageViewModel>();
            containerRegistry.RegisterForNavigation<MatchesPage, MatchesPageViewModel>();
        }
    }
}
