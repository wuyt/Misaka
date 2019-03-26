/* *
 * 版本：1.0
 * 日期：2019年2月22日
 * 作者：吴雁涛
 * 新增：脱卡场景逻辑
 * */
using EasyAR;
using UnityEngine;
using Lean.Touch;

namespace Misaka
{
    /// <summary>
    /// 脱卡场景逻辑
    /// </summary>
    public class RemoveManger : MonoBehaviour
    {
        /// <summary>
        /// 脱卡按钮
        /// </summary>
        private GameObject btnRemove;
        /// <summary>
        /// 还原按钮
        /// </summary>
        private GameObject btnRestore;

        /// <summary>
        /// 图片识别对象
        /// </summary>
        private Transform targetTransform;

        /// <summary>
        /// 显示的模型
        /// </summary>
        private Transform model;

        /// <summary>
        /// 模型默认位置
        /// </summary>
        private Vector3 position;
        /// <summary>
        /// 模型默认旋转
        /// </summary>
        private Quaternion rotation;
        /// <summary>
        /// 模型默认缩放
        /// </summary>
        private Vector3 scale;

        /// <summary>
        /// Touch插件
        /// </summary>
        private LeanTouch leanTouch;

        private void Start()
        {
            //找到按钮
            btnRemove = GameObject.Find("/Canvas/btnRemove");
            btnRestore = GameObject.Find("/Canvas/btnRestore");
            //禁用按钮
            btnRemove.SetActive(false);
            btnRestore.SetActive(false);
            //找到插件
            leanTouch = FindObjectOfType<LeanTouch>();
            //禁用插件
            leanTouch.enabled = false;
        }

        /// <summary>
        /// 图片被识别
        /// </summary>
        /// <param name="target">被识别的图片对象</param>
        public void ImageTargetFound(ImageTarget target)
        {
            //如果已经在脱卡状态
            if (model != null)
            {
                Restore();
            }

            //根据识别图片名称获取游戏对象
            targetTransform = GameObject.Find("/it" + target.Name).transform;
            model = targetTransform.GetChild(0);
            //记录模型位置信息
            position = model.localPosition;
            rotation = model.localRotation;
            scale = model.localScale;
            //显示脱卡按钮
            btnRemove.SetActive(true);
        }

        /// <summary>
        /// 图片目标丢失
        /// </summary>
        /// <param name="target">被识别的图片对象</param>
        public void ImageTargetLost(ImageTarget target)
        {
        }

        /// <summary>
        /// 脱卡
        /// </summary>
        public void Remove()
        {
            //脱卡
            model.parent = null;
            //启用插件，允许拖动和缩放模型
            leanTouch.enabled = true;
            //禁用脱卡按钮
            btnRemove.SetActive(false);
            //启用还原按钮
            btnRestore.SetActive(true);
        }

        /// <summary>
        /// 还原
        /// </summary>
        public void Restore()
        {
            //模型还原
            model.parent = targetTransform;
            model.localPosition = position;
            model.localRotation = rotation;
            model.localScale = scale;
            //禁用插件
            leanTouch.enabled = false;
            //禁用还原按钮
            btnRestore.SetActive(false);

            //清除模型信息，用于再次识别的判断
            model = null;
            targetTransform = null;
        }
    }
}

