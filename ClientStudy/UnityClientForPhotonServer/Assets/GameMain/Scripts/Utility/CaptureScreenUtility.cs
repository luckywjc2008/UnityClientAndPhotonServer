/*
 * Author:      NOW
 * CreateTime:  20171205
 * Description: 对屏幕截图并保存
 * 
*/
using UnityEngine;
using System.IO;
using System.Collections;

namespace StarForce
{
    public partial class CaptureScreenUtility
    {
        /// <summary>  
        /// 使用Application类下的CaptureScreenshot()方法实现截图  
        /// 优点：简单，可以快速地截取某一帧的画面、全屏截图  
        /// 缺点：不能针对摄像机截图，无法进行局部截图  
        /// </summary>  
        /// <param name="fileSavaPath">图片文件存储路径。</param>  
        private void CaptureByUnity(string fileSavaPath)
        {
            ScreenCapture.CaptureScreenshot(fileSavaPath, 0);
        }

        /// <summary>  
        /// 根据一个Rect类型来截取指定范围的屏幕。
        /// 左下角为(0,0)  
        /// </summary>  
        /// <param name="mRect">截取屏幕范围。</param>  
        /// <param name="fileSavePath">图片文件存储路径。</param>  
        private IEnumerator CaptureByRect(Rect mRect, string fileSavePath)
        {
            //等待渲染线程结束  
            yield return new WaitForEndOfFrame();
            //初始化Texture2D  
            Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
            //读取屏幕像素信息并存储为纹理数据  
            mTexture.ReadPixels(mRect, 0, 0);
            //应用  
            mTexture.Apply();

            //保存
            if (!string.IsNullOrEmpty(fileSavePath))
            {
                //将图片信息编码为字节信息  
                byte[] bytes = mTexture.EncodeToPNG();

                FileTool.CreatFilePath(fileSavePath);
                File.WriteAllBytes(fileSavePath, bytes);
            }
        }

        /// <summary>
        /// 根据一个Rect类型来截取指定摄像机里指定范围的内容
        /// </summary>
        /// <param name="mCamera">目标摄像机</param>
        /// <param name="mRect">摄像机里截取范围</param>
        /// <param name="fileSavePath">图片文件存储路径。</param>
        /// <returns></returns>
        private IEnumerator CaptureByCamera(Camera mCamera, Rect mRect, string fileSavePath)
        {
            //等待渲染线程结束  
            yield return new WaitForEndOfFrame();

            //初始化RenderTexture  
            RenderTexture mRender = new RenderTexture((int)mRect.width, (int)mRect.height, 0);
            //设置相机的渲染目标  
            mCamera.targetTexture = mRender;
            //开始渲染  
            mCamera.Render();

            //激活渲染贴图读取信息  
            RenderTexture.active = mRender;

            Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
            //读取屏幕像素信息并存储为纹理数据  
            mTexture.ReadPixels(mRect, 0, 0);
            //应用  
            mTexture.Apply();

            //释放相机，销毁渲染贴图  
            mCamera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(mRender);

            //保存
            if (!string.IsNullOrEmpty(fileSavePath))
            {
                //将图片信息编码为字节信息  
                byte[] bytes = mTexture.EncodeToPNG();

                FileTool.CreatFilePath(fileSavePath);
                File.WriteAllBytes(fileSavePath, bytes);
            }

        }

    }
}