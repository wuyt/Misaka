/* *
 * 版本：1.0
 * 日期：2019年2月15日
 * 作者：吴雁涛
 * 新增：模型碰撞脚本
 * */
using UnityEngine;

/// <summary>
/// 模型碰撞
/// </summary>
public class ModelTrigger : MonoBehaviour
{

    /// <summary>
    /// 负责逻辑的脚本所在游戏对象
    /// </summary>
    private GameObject gameManager;

    void Start()
    {
        //找到名为GameManager的游戏对象，
        gameManager = GameObject.Find("GameManager");
    }

    /// <summary>
    /// 进入
    /// </summary>
    /// <param name="other">进入对象</param>
    private void OnTriggerEnter(Collider other)
    {
        gameManager.SendMessage("ModelTriggerEnter", gameObject.name + "-" + other.gameObject.name);
    }

    /// <summary>
    /// 离开
    /// </summary>
    /// <param name="other">离开对象</param>
    private void OnTriggerExit(Collider other)
    {
        gameManager.SendMessage("ModelTriggerExit", gameObject.name + "-" + other.gameObject.name);
    }
}
