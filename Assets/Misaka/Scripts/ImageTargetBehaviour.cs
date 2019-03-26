/* *
 * 版本：1.0
 * 日期：2019年2月1日
 * 作者：吴雁涛
 * 新增：订阅图片识别和丢失事件
 * */
using EasyAR;
using UnityEngine;

namespace Misaka
{
    /// <summary>
    /// 图片目标
    /// </summary>
    public class ImageTargetBehaviour : ImageTargetBaseBehaviour
    {
        /// <summary>
        /// 负责逻辑的脚本所在游戏对象
        /// </summary>
        public GameObject gameManager;

        /// <summary>
        /// 重写Awake事件
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            //订阅事件
            TargetFound += OnTargetFound;   //识别成功事件
            TargetLost += OnTargetLost;     //识别对象丢失事件
        }

        /// <summary>
        /// 识别成功事件处理方法
        /// </summary>
        void OnTargetFound(TargetAbstractBehaviour behaviour)
        {
            Debug.Log("Found: " + Target.Name);     //输出到控制台
            //消息推送，执行 ImageTargetFound 方法，参数为ImageTarget
            gameManager.SendMessage("ImageTargetFound", Target);
        }

        /// <summary>
        /// 识别对象丢失事件处理方法
        /// </summary>
        void OnTargetLost(TargetAbstractBehaviour behaviour)
        {
            Debug.Log("Lost: " + Target.Name);      //输出到控制台
            //消息推送，执行 ImageTargetLost 方法，参数为ImageTarget
            gameManager.SendMessage("ImageTargetLost", Target);
        }
    }
}