using System;
using Android.App;
using Android.Content;
using AndroidX.AppCompat.App;
using Com.Unity3d.Player;
using UnityXamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Unity3dView), typeof(UnityXamarin.Droid.Unity3dViewRenderer))]
namespace UnityXamarin.Droid
{
    public class Unity3dViewRenderer : ViewRenderer<Unity3dView, global::Android.Views.View>, IUnityMessageListener
    {
        Context _context;
        public static UnityPlayer UnityPlayerInstance { get; set; }


        global::Android.Views.View view;
        AppCompatActivity activity;
        int NewViewID;
        const string FragmentName = UnityPlayerFragment.FragmentName;

        public Unity3dViewRenderer(Context context) : base(context)
        {
            _context = context;
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Unity3dView> e)
        {

            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var unity3dView = e.OldElement;
                if (unity3dView != null)
                {
                    unity3dView.DestroyView -= RemoveFragment;
                    unity3dView.CreateView -= AddFragment;
                    unity3dView.UnitySendMessage -= UnitySendMessage;
                }
            }

            if (e.NewElement != null)
            {
                var unity3dView = e.NewElement;
                if (unity3dView != null)
                {
                    unity3dView.DestroyView += RemoveFragment;
                    unity3dView.CreateView += AddFragment;
                    unity3dView.UnitySendMessage += UnitySendMessage;
                }

                try
                {
                    SetupUserInterface();
                    SetNativeControl(view);

                    AddFragment();
                }
                catch (Exception ex)
                {

                }
            }

        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            RemoveFragment();
        }

        void RemoveFragment()
        {
            activity = this.Context as AppCompatActivity;

            var unityFragment = activity.SupportFragmentManager.FindFragmentByTag(FragmentName);
            if (unityFragment != null)
            {
                activity.SupportFragmentManager.BeginTransaction().Remove(unityFragment).Commit();
            }
        }

        void AddFragment()
        {
            var unityFragment = activity.SupportFragmentManager.FindFragmentByTag(FragmentName);
            if (unityFragment == null)
            {
                activity = this.Context as AppCompatActivity;
                var unityPlayerFragment = new UnityPlayerFragment();
                UnityPlayerFragment.SetUnityMessageListener(this);
                activity.SupportFragmentManager
                           .BeginTransaction()
                           .Replace(NewViewID,
                           unityPlayerFragment, FragmentName)
                           .Commit();
            }
        }


        void UnitySendMessage(string gameObject, string function, string param)
        {
            var unityFragment = activity.SupportFragmentManager.FindFragmentByTag(FragmentName) as UnityPlayerFragment;
            if (unityFragment != null)
            {
                unityFragment.UnitySendMessage(gameObject, function, param);
            }
        }

        void SetupUserInterface()
        {
            activity = this.Context as AppCompatActivity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.UnityPlayerHolder, this, false);

            var fragmentContainer = view.FindViewById(Resource.Id.fragment_container);
            NewViewID = fragmentContainer.Id;
        }

        public void MessageFromUnity(string msg)
        {
            Element.RaiseMessageFromUnity(msg);
        }
    }
}
