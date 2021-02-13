using System;
using Xamarin.Forms;

namespace UnityXamarin
{
    public class Unity3dView : View
    {
        //public virtual void UnitySendMessage(string gameObj, string method, string arg)
        //{
        //}

        public Action DestroyView;
        public Action CreateView;

        public Action<string, string, string> UnitySendMessage;
        public event EventHandler<String> OnMessageFromUnity;

        public void RaiseMessageFromUnity(string msg)
        {
            OnMessageFromUnity?.Invoke(this, msg);
        }
    }
}
