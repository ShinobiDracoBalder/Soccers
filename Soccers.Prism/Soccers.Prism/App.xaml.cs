using Prism;
using Prism.Ioc;
using Soccers.Common.Helpers;
using Soccers.Common.Services;
using Soccers.Prism.ViewModels;
using Soccers.Prism.Views;
using Syncfusion.Licensing;
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
            SyncfusionLicenseProvider.RegisterLicense("NTM1OTMxQDMxMzkyZTMzMmUzMGhFeVlZcTQ3YlYzSzR1UElORUZPNzlySWpDSklONzlSNWQ3YW5oM2c2ZEk9");

            InitializeComponent();
            await NavigationService.NavigateAsync("/SoccerMasterDetailPage/NavigationPage/TournamentsPage");
            //await NavigationService.NavigateAsync("NavigationPage/TournamentsPage");
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
            containerRegistry.RegisterForNavigation<ClosedMatchesPage, ClosedMatchesPageViewModel>();
            containerRegistry.RegisterForNavigation<TournamentTabbedPage, TournamentTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<SoccerMasterDetailPage, SoccerMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<MyPredictionsPage, MyPredictionsPageViewModel>();
            containerRegistry.RegisterForNavigation<MyPositionsPage, MyPositionsPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
        }
    }
}
