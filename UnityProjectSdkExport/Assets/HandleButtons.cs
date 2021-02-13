using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleButtons : MonoBehaviour
{
    [SerializeField]
    private Button redButton;
    [SerializeField]
    private Button greenButton;
    [SerializeField]
    private Button blueButton;

    [SerializeField]
    private GameObject cube;

    private Material cubeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        redButton.onClick.AddListener(RedButtonOnClick);
        greenButton.onClick.AddListener(GreenButtonOnClick);
        blueButton.onClick.AddListener(BlueButtonOnClick);


        cubeMaterial = cube.GetComponent<Renderer>().material;
    }

    void RedButtonOnClick()
    {
        ResetColor("#FF0000");
    }

    void GreenButtonOnClick()
    {
        ResetColor("#00FF00");
    }

    void BlueButtonOnClick()
    {
        ResetColor("#0000FF");
    }

    public void SetCubeColor(string colorHex)
    {
        if (ColorUtility.TryParseHtmlString(colorHex, out Color newCol))
        {
            cubeMaterial.SetColor("_Color", newCol);  
        }
    }

    private void ResetColor(string colorHex)
    {
        SetCubeColor(colorHex);
        SendMsgToNative(new NativeMessageObject
        {
            functionName = "SetColor",
            functionArg = new string[] { colorHex }
        });
    }


    private void SendMsgToNative(NativeMessageObject msg)
    {
        //#if UNITY_ANDROID
        try
        {
            string json = JsonUtility.ToJson(msg);
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "com.example.broadcast.UnityMessage");
            intent.Call<AndroidJavaObject>("putExtra", "data", json);
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("sendBroadcast", intent);
        }
        catch (Exception ex)
        {
            Debug.Log($"---- SendMsgToNative Exception: {ex.Message} - {ex.StackTrace}");
        }
        //#endif

    }


    [Serializable]
    public class NativeMessageObject
    {
        public string functionName;
        public string[] functionArg;
    }
}
