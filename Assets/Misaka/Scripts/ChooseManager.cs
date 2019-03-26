/* *
 * 版本：1.0
 * 日期：2019年2月28日
 * 作者：吴雁涛
 * 新增：识别控制场景逻辑
 * */
using EasyAR;
using UnityEngine;

namespace Misaka
{
    /// <summary>
    /// 识别控制场景逻辑
    /// </summary>
    public class ChooseManager : MonoBehaviour
    {
        /// <summary>
        /// bus图片目标类
        /// </summary>
        EasyAR.ImageTargetBehaviour itBus;
        /// <summary>
        /// nanoha图片目标类
        /// </summary>
        EasyAR.ImageTargetBehaviour itNanoha;
        /// <summary>
        /// friend图片目标类
        /// </summary>
        EasyAR.ImageTargetBehaviour itFriend;

        /// <summary>
        /// bus图片识别状态
        /// </summary>
        bool statusBus;
        /// <summary>
        /// bus图片识别状态
        /// </summary>
        bool statusNanoha;
        /// <summary>
        /// bus图片识别状态
        /// </summary>
        bool statusFriend;

        // Use this for initialization
        void Awake()
        {
            //找到游戏对象上的组件
            itBus = GameObject.Find("/itBus")
                .GetComponent<EasyAR.ImageTargetBehaviour>();
            itNanoha = GameObject.Find("/itNanoha")
                .GetComponent<EasyAR.ImageTargetBehaviour>();
            itFriend = GameObject.Find("/itFriend")
                .GetComponent<EasyAR.ImageTargetBehaviour>();

            //默认都是不识别
            statusBus = false;
            statusNanoha = false;
            statusFriend = false;
        }

        /// <summary>
        /// bus图片识别控制
        /// </summary>
        public void RecognizeBus()
        {
            if (statusBus)
            {
                itBus.SetupWithImage("", StorageType.Assets, "", Vector2.zero);
            }
            else
            {
                itBus.SetupWithImage("bus.jpg", StorageType.Assets, "bus", new Vector2(1f, 0.665f));
            }
            statusBus = !statusBus;
        }

        /// <summary>
        /// Nanoha图片识别控制
        /// </summary>
        public void RecognizeNanoha()
        {
            if (statusNanoha)
            {
                itNanoha.SetupWithImage("", StorageType.Assets, "", Vector2.zero);
            }
            else
            {
                itNanoha.SetupWithImage("nanoha.jpg", StorageType.Assets, "nanoha", new Vector2(1f, 0.976f));
            }
            statusNanoha = !statusNanoha;
        }

        /// <summary>
        /// Friend图片识别控制
        /// </summary>
        public void RecognizeFriend()
        {
            if (statusFriend)
            {
                itFriend.SetupWithImage("", StorageType.Assets, "", Vector2.zero);
            }
            else
            {
                itFriend.SetupWithImage("friend.jpg", StorageType.Assets, "friend", new Vector2(1f, 0.88f));
            }
            statusFriend = !statusFriend;
        }
    }

}
