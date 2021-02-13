using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace UnityXamarin.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public UnityPlayerFragment GetUnityPlayerFragment()
        {
            return this.SupportFragmentManager.FindFragmentByTag(UnityPlayerFragment.FragmentName) as UnityPlayerFragment;
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var unityPlayerFragment = GetUnityPlayerFragment();
            if (unityPlayerFragment != null)
            {
                unityPlayerFragment.ProcessNewIntent(intent);
            }
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            var unityPlayerFragment = GetUnityPlayerFragment();
            if (unityPlayerFragment != null)
            {
                unityPlayerFragment.ProcessWindowFocusChanged(hasFocus);
            }
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            var unityPlayerFragment = GetUnityPlayerFragment();
            if (unityPlayerFragment != null)
            {
                unityPlayerFragment.ProcessTrimMemory(level);
            }
        }
    }
}