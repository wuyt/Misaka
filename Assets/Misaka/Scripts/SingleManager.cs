/* *
 * 版本：1.0
 * 日期：2019年2月2日
 * 作者：吴雁涛
 * 新增：识别单图场景逻辑
 * */
using UnityEngine;
using EasyAR;

namespace Misaka
{
    /// <summary>
    /// 单图识别场景管理
    /// </summary>
    public class SingleManager : MonoBehaviour
    {
        /// <summary>
        /// 打开裕廊飞禽公园网址
        /// </summary>
        public void OpenURL()
        {
            Application.OpenURL("https://www.visitsingapore.com.cn/see-do-singapore/nature-wildlife/fun-with-animals/jurong-bird-park/");
        }

        /// <summary>
        /// 图片被识别
        /// </summary>
        /// <param name="target">被识别的图片对象</param>
        public void ImageTargetFound(ImageTarget target)
        {
            if (target.Name == "Nanoha") 
            {
                Application.OpenURL("https://zh.moegirl.org/%E9%AD%94%E6%B3%95%E5%B0%91%E5%A5%B3%E5%A5%88%E5%8F%B6%E7%B3%BB%E5%88%97");
            }
        }

        /// <summary>
        /// 图片目标丢失
        /// </summary>
        /// <param name="target">被识别的图片对象</param>
        public void ImageTargetLost(ImageTarget target)
        {

        }
    }
}
