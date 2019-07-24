using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;
using Plugin.Permissions;
using servicebus.communication.Core;

namespace servicebus.communication.Droid
{
    [Activity(Label = "BaseView")]
    public abstract class BaseView<T> : MvxAppCompatActivity<T> where T : BaseViewModel
    {
        protected abstract int LayoutResourceId { get; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(LayoutResourceId);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
