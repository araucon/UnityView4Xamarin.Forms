using System;

using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Com.Unity3d.Player;

namespace UnityXamarin.Droid
{
    public interface IUnityMessageListener
    {
        void MessageFromUnity(string msg);
    }

    public class UnityPlayerFragment : Fragment , IUnityPlayerLifecycleEvents
    {

        public const string FragmentName = "UnityPlayerFragment";
        

        protected UnityPlayer mUnityPlayer;
        //FrameLayout frameLayoutForUnity;

        UnityMessageReceiver receiver;

        static WeakReference<IUnityMessageListener> weakUnityMessageListener;

        public static void SetUnityMessageListener(IUnityMessageListener unityMessageListener)
        {
            weakUnityMessageListener = new WeakReference<IUnityMessageListener>(unityMessageListener);
        }

        public UnityPlayerFragment()
        {
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            receiver = new UnityMessageReceiver();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            mUnityPlayer = new UnityPlayer(Activity, this);
            //View view = inflater.Inflate(R.layout.fragment_unity, container, false);
            //this.frameLayoutForUnity = (FrameLayout)view.findViewById(R.id.frameLayoutForUnity);
            //this.frameLayoutForUnity.addView(mUnityPlayer.getView(),
            //        FrameLayout.LayoutParams.MATCH_PARENT, FrameLayout.LayoutParams.MATCH_PARENT);

            mUnityPlayer.RequestFocus();
            return mUnityPlayer;
        }


        public void ProcessNewIntent(Intent intent)
        {
            mUnityPlayer.NewIntent(intent);
        }

        public void ProcessWindowFocusChanged(bool hasFocus)
        {
            mUnityPlayer.WindowFocusChanged(hasFocus);
        }

        public void ProcessTrimMemory([GeneratedEnum] TrimMemory level)
        {
            if (level == TrimMemory.RunningCritical)
            {
                mUnityPlayer.LowMemory();
            }
        }


        public void UnitySendMessage(string gameObject, string function, string param)
        {
            UnityPlayer.UnitySendMessage(gameObject, function, param);
        }

        public override void OnPause()
        {
            base.OnPause();
            Activity.UnregisterReceiver(receiver);
            mUnityPlayer.Pause();
        }

        public override void OnResume()
        {
            base.OnResume();

            Activity.RegisterReceiver(receiver, new IntentFilter("com.example.broadcast.UnityMessage"));
            mUnityPlayer.Resume();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mUnityPlayer.LowMemory();
        }

        
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mUnityPlayer.ConfigurationChanged(newConfig);
        }

       
        #region IUnityPlayerLifecycleEvents
        public void OnUnityPlayerQuitted()
        {
            //TODO
            //Process.KillProcess(Process.MyPid());
        }

        public void OnUnityPlayerUnloaded()
        {
            //TODO
            //Activity.MoveTaskToBack(true);
        }
        #endregion IUnityPlayerLifecycleEvents


        public class UnityMessageReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                var data = intent.GetStringExtra("data");
                IUnityMessageListener unityMessageListener;
                if (weakUnityMessageListener.TryGetTarget(out unityMessageListener))
                {
                    unityMessageListener.MessageFromUnity(data);
                }
                
            }
        }
    }
}
