using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;
using Xamarin.Forms.Xaml;

namespace UnityXamarin
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            On<Android>()
                .SendDisappearingEventOnPause(false)
                .SendAppearingEventOnResume(false)
                .ShouldPreserveKeyboardOnResume(true);

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
