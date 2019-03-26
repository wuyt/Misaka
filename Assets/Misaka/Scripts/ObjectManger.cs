/* *
 * 版本：1.0
 * 日期：2019年2月22日
 * 作者：吴雁涛
 * 新增：物体识别场景逻辑
 * */
using DG.Tweening;
using UnityEngine;

namespace Misaka
{
    /// <summary>
    /// 物体识别场景逻辑
    /// </summary>
    public class ObjectManger : MonoBehaviour
    {
        /// <summary>
        /// 救火车识别后的效果对象
        /// </summary>
        private Transform fireTruckShow;

        /// <summary>
        /// DoTween动画对象
        /// </summary>
        private Sequence sequence;

        /// <summary>
        /// 动画时长
        /// </summary>
        private float time=4f;

        /// <summary>
        /// 物体被识别
        /// </summary>
        /// <param name="ObjectName">被识别物体名称</param>
        public void ObjectTargetFound(string ObjectName)
        {
            //如果识别物体是救火车
            if(ObjectName== "FireTruckII")
            {
                //fireTruckShow第一次使用时赋值
                if (fireTruckShow == null)
                {
                    fireTruckShow = GameObject.Find("ObjectTarget/Cube").transform;
                }

                //动画
                sequence = DOTween.Sequence();
                sequence.Append(fireTruckShow
                    .DOMoveY(40, time)
                    .SetRelative()
                    .SetEase(Ease.Linear));
                sequence.Insert(0, fireTruckShow
                    .DOLocalRotate(new Vector3(0, 90, 0), time / 4)
                    .SetEase(Ease.Linear)
                    .SetLoops(4, LoopType.Restart));
                sequence.Insert(0, fireTruckShow.GetComponent<Renderer>().material
                    .DOColor(Color.yellow, time / 2));
                sequence.SetLoops(-1, LoopType.Yoyo);
            }
        }

        /// <summary>
        /// 物体识别消失
        /// </summary>
        /// <param name="ObjectName">被识别物体名称</param>
        public void ObjectTargetLost(string ObjectName)
        {
            if (ObjectName == "FireTruckII")
            {
                sequence.Kill();
            }
        }
    }
}

