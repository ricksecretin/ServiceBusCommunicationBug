using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Support.V7.AppCompat;
using servicebus.communication.Core;

namespace servicebus.communication.Droid
{
    [Activity(
        MainLauncher = true,
        Theme = "@style/Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenAppCompatActivity<Setup, App>
    {
    }
}
