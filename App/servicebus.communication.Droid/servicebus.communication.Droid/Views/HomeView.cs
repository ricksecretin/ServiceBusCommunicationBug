using System;
using Android.App;
using Android.Content.PM;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using servicebus.communication.Core;

namespace servicebus.communication.Droid
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme",
              ScreenOrientation = ScreenOrientation.SensorPortrait)]
    public class HomeView : BaseView<HomeViewModel>
    {
        protected override int LayoutResourceId => Resource.Layout.activity_home;
    }
}
