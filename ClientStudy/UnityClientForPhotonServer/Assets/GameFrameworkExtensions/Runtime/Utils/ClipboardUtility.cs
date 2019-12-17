using System;
using System.Runtime.InteropServices;
using GameFramework;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public static class ClipboardUtility
    {
        public static IClipboardBase ClipboardInstance_ = null;

        public static void CopyToClipboard(string input)
        {

#if UNITY_STANDALONE_WIN  || UNITY_EDITOR
            TextEditor te = new TextEditor();
            te.text = input;
            te.OnFocus();
            te.Copy();
#elif UNITY_ANDROID
            if (ClipboardInstance_ == null)
            {  
                ClipboardInstance_ = new AndirodClipboardUtility();;
            }
    
            ClipboardInstance_.CopyToClipboard(input);
#elif UNITY_IOS
            if (ClipboardInstance_ == null)
            {  
                ClipboardInstance_ = new IosClipboardUtility();
            }

            ClipboardInstance_.CopyToClipboard(input);  
#endif
        }
    }

    public interface IClipboardBase
    {
        /// <summary>
        /// 粘贴文本到剪贴板
        /// </summary>
        /// <param name="input"></param>
        void CopyToClipboard(string input);
    }

    internal class AndirodClipboardUtility : IClipboardBase
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private AndroidJavaObject androidObject = new AndroidJavaObject("com.tool.wjc.clipboardtool.ClipboardTool");
        private AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
#endif
        public void CopyToClipboard(string input)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (activity == null)
                return;
            Log.Info("invoke copyTextToClipboard");
            // 复制到剪贴板
            androidObject.Call("copyTextToClipboard", activity, input);
#endif
        }
    }


    internal class IosClipboardUtility : IClipboardBase
    {
#if UNITY_IOS && !UNITY_EDITOR
        /* Interface to native implementation */
        [DllImport ("__Internal")]
        private static extern void _copyTextToClipboard(string text);
#endif
        public void CopyToClipboard(string input)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Log.Info("invoke _copyTextToClipboard");
            _copyTextToClipboard(input);
#endif
        }
    }

}
