using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;

namespace Soccers.Prism.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash",
              MainLauncher = true,
              NoHistory = true)]
    //public class SplashActivity : AppCompatActivity
    public class SplashActivity : Activity
    {
        // Launches the startup task
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(10);
            StartActivity(typeof(MainActivity));
        }
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        //}
    }
}
