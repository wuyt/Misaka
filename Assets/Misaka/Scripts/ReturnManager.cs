/* *
 * 版本：1.0
 * 日期：2019年2月1日
 * 作者：吴雁涛
 * 新增：返回主菜单功能，退出应用
 * */
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misaka
{
    /// <summary>
    /// 返回功能管理
    /// </summary>
    public class ReturnManager : MonoBehaviour
    {

        /// <summary>
        /// 返回功能UI画布
        /// </summary>
        public GameObject canvas;
        /// <summary>
        /// 返回按钮
        /// </summary>
        private GameObject btnReturn;

        void Start()
        {
            //找到canvas下的返回按钮，
            btnReturn = canvas.transform.Find("Panel/ButtonReturn").gameObject;

            //不让【Return】游戏对象（当前游戏对象的父对象）在加载场景时被删除
            DontDestroyOnLoad(transform.parent);

            //不显示画布
            canvas.SetActive(false);
        }

        void Update()
        {
            //当在Loading场景之外的场景中按了返回键，
            if (Input.GetKeyUp(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Loading")
            {
                //canvas激活状态变更
                canvas.SetActive(!canvas.activeSelf);

                //如果是在Menu场景
                if (SceneManager.GetActiveScene().name == "Menu")
                {
                    if (canvas.activeSelf)
                    {
                        //画布激活时返回按钮隐藏
                        btnReturn.SetActive(false);
                    }
                    else
                    {
                        //重置状态
                        btnReturn.SetActive(true);
                    }
                }
            }

        }

        /// <summary>
        /// 返回主菜单场景
        /// </summary>
        public void ReturnMenu()
        {
            canvas.SetActive(false);        //关闭返回的界面
            SceneManager.LoadScene("Menu");
        }

        /// <summary>
        /// 应用退出
        /// </summary>
        public void Quit()
        {
           #if UNITY_EDITOR     //  Unity 编辑器中退出
            EditorApplication.isPlaying = false;
           #else
            Application.Quit();     //程序打包后退出
           #endif
        }
    }
}