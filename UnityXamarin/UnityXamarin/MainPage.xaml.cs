using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace UnityXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            UnityView.OnMessageFromUnity += UnityView_OnMessageFromUnity;
        }

        private async void UnityView_OnMessageFromUnity(object sender, string e)
        {
            await DisplayAlert("Message from unity", $"this is the json message sent by Unity\n {e}", "OK");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.UnityView.CreateView?.Invoke();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.UnityView.DestroyView?.Invoke();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            UnityView.UnitySendMessage("MyCanvas", "SetCubeColor", HexColorEntry.Text.Trim());
        }
    }
}
