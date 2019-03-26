/* *
 * 版本：1.0
 * 日期：2019年1月31日
 * 作者：吴雁涛
 * 新增：加载场景
 * */
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misaka
{
    /// <summary>
    /// 菜单场景管理
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}