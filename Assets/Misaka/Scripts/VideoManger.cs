/* *
 * 版本：1.0
 * 日期：2019年2月2日
 * 作者：吴雁涛
 * 新增：视频播放场景逻辑
 * */
using EasyAR;
using UnityEngine;

/// <summary>
/// 视频播放场景逻辑
/// </summary>
public class VideoManger : MonoBehaviour {

    /// <summary>
    /// Plane上的视频状态
    /// </summary>
    private bool statusPlane;

    /// <summary>
    /// Cube上的视频状态
    /// </summary>
    private bool statusCube;

    void Start()
    {
        //默认为true，即在播放状态
        statusCube = true;
        statusPlane = true;
    }

    void Update () {
        //只有一个点击点并且点击完成
        if (Input.touchCount == 1 && Input.touches[0].phase ==TouchPhase.Ended)
        {
            //被点中的游戏对象
            RaycastHit hit;

            //将屏幕点击点转换为射线发射位置
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            //射线点击判断
            if (Physics.Raycast(ray, out hit))
            {
                //如果点中了Plane游戏对象
                if (hit.transform.name == "Plane")
                {
                    //如果当前视频在播放
                    if (statusPlane)
                    {
                        hit.transform.GetComponent<VideoPlayerBehaviour>().Pause();
                    }
                    else
                    {
                        hit.transform.GetComponent<VideoPlayerBehaviour>().Play();
                    }

                    statusPlane = !statusPlane;
                }

                //如果点中了Cube游戏对象
                if (hit.transform.name == "Cube")
                {
                    //如果当前视频在播放
                    if (statusCube)
                    {
                        hit.transform.GetComponent<VideoPlayerBehaviour>().Pause();
                    }
                    else
                    {
                        hit.transform.GetComponent<VideoPlayerBehaviour>().Play();
                    }

                    statusCube = !statusCube;
                }
            }
        }
	}
}
