using UnityEngine;

/// <summary>
/// 3D摄像机自适应
/// author: NOW
/// time: 20171019
/// contact: 414255309@qq.com
/// </summary>
[RequireComponent(typeof(Camera)),DisallowMultipleComponent]
public class Camera3DScale : MonoBehaviour
{
    /// <summary>
    /// 是否正交投影
    /// </summary>
    [SerializeField]
    private bool isOrthographic;
    /// <summary>
    /// 设计分辨率 宽度 
    /// </summary>
    [SerializeField]
    private float designWidth;
    /// <summary>
    /// 设计分辨率 高度
    /// </summary>
    [SerializeField]
    private float designHeight;
    /// <summary>
    /// 初始FOV大小(当为透视非正交投影时设置)
    /// </summary>
    [SerializeField]
    private float initFieldOfView;
    /// <summary>
    /// 初始正交投影大小(当为正交非透视投影时设置)
    /// </summary>
    [SerializeField]
    private float initOrthographicSize;

    private Camera myCamera;
    private float initAspectRatio;
    private float infactAspectRatio;

    void Start()
    {
        initAspectRatio = designWidth / designHeight;
        infactAspectRatio = Screen.width * 1.0f / Screen.height;
        if (myCamera == null)
        {
            myCamera = GetComponent<Camera>();
        }

        if (!isOrthographic)
        { 
            if (infactAspectRatio > initAspectRatio)
                myCamera.fieldOfView = initFieldOfView ;
            else
                myCamera.fieldOfView = initFieldOfView * initAspectRatio / infactAspectRatio;

            //Debug.Log(gameObject.name + ".fieldView =" + myCamera.fieldOfView);
        }
        else
        {
            if(infactAspectRatio > initAspectRatio)
            {
                myCamera.orthographicSize = initOrthographicSize;
            }
            else
            {
                myCamera.orthographicSize = initOrthographicSize * initAspectRatio / infactAspectRatio;
            }
            //Debug.Log(gameObject.name + " orthographicSize=" + myCamera.orthographicSize);
        }
    }
}