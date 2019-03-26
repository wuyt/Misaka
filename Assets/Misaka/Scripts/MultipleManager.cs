/* *
 * 版本：1.0
 * 日期：2019年2月15日
 * 作者：吴雁涛
 * 新增：多图识别控制
 * 版本：1.1
 * 日期：2019年2月18日
 * 作者：吴雁涛
 * 新增：添加Yuko逻辑
 * 日期：2019年2月20日
 * 作者：吴雁涛
 * 新增：添加Misaki逻辑
 * */
using System.Collections;
using UnityEngine;
using DG.Tweening;
using EasyAR;

namespace Misaka
{
    /// <summary>
    /// 多图识别控制
    /// </summary>
    public class MultipleManager : MonoBehaviour
    {
        #region Yuko

        /// <summary>
        /// Yuko状态枚举
        /// </summary>
        public enum StateYuko
        {
            /// <summary>
            /// 隐藏
            /// </summary>
            Hidden,
            /// <summary>
            /// 站立
            /// </summary>
            Stand,
            /// <summary>
            /// 摆造型
            /// </summary>
            Pose
        }

        /// <summary>
        /// Yuko当前状态
        /// </summary>
        public StateYuko stateYuko;

        /// <summary>
        /// Yuko控制对象
        /// </summary>
        private Animator yuko;

        /// <summary>
        /// Yuko协程
        /// </summary>
        private Coroutine yukoCoroutine;

        /// <summary>
        /// Yuko隐藏状态
        /// </summary>
        private void YukoHidden()
        {
            //停止协程
            if (yukoCoroutine != null)
            {
                StopCoroutine(yukoCoroutine);
            }

            stateYuko = StateYuko.Hidden;

            yuko.gameObject.SetActive(false);   //隐藏模型
        }

        /// <summary>
        /// Yuko站立状态
        /// </summary>
        private IEnumerator YukoStand()
        {
            stateYuko = StateYuko.Stand;
            yuko.gameObject.SetActive(true);    //显示模型

            //随机等待2到6秒
            yield return new WaitForSeconds(Random.Range(2, 6));

            YukoPose();
        }

        /// <summary>
        /// Yuko摆造型状态
        /// </summary>
        private void YukoPose()
        {
            stateYuko = StateYuko.Pose;
            yuko.SetTrigger("Pose");    //模型做pose动作，动作完成后自动返回
        }

        #endregion

        #region UTC

        /// <summary>
        /// UTC状态枚举
        /// </summary>
        private enum StateUTC
        {
            /// <summary>
            /// 隐藏
            /// </summary>
            Hidden,
            /// <summary>
            /// 站立
            /// </summary>
            Stand,
            /// <summary>
            /// 走开
            /// </summary>
            WalkAway,
            /// <summary>
            /// 走向Yuko
            /// </summary>
            WalkToYuko,
            /// <summary>
            /// 站在Yuko旁边
            /// </summary>
            StandWithYuko,
            /// <summary>
            /// 走回起点
            /// </summary>
            WalkBack
        }

        /// <summary>
        /// UTC状态
        /// </summary>
        private StateUTC stateUTC;

        /// <summary>
        /// UTC控制对象
        /// </summary>
        private Animator utc;

        /// <summary>
        /// UTC的父节点
        /// </summary>
        private Transform utcParent;

        /// <summary>
        /// utc移动动画
        /// </summary>
        private Tween utcTween;

        /// <summary>
        /// UTC隐藏
        /// </summary>
        private void UTCHidden()
        {
            stateUTC = StateUTC.Hidden;
            utc.gameObject.SetActive(false);    //隐藏模型

            UTCRestore();
        }

        /// <summary>
        /// UTC站立在原点
        /// </summary>
        private void UTCStand()
        {
            stateUTC = StateUTC.Stand;
            utc.gameObject.SetActive(true);     //显示模型
        }

        /// <summary>
        /// UTC走开
        /// </summary>
        private void UTCWalkAway()
        {
            stateUTC = StateUTC.WalkAway;

            //将UTC设为根节点游戏对象
            utc.transform.parent = null;

            //识别图片周围4个点
            float spacing = 1.1f;
            Vector3[] targets = new Vector3[4];
            targets[0] = new Vector3(utcParent.position.x - spacing,
                utcParent.position.y,
                utcParent.position.z + spacing);
            targets[1] = new Vector3(utcParent.position.x - spacing,
                utcParent.position.y,
                utcParent.position.z - spacing);
            targets[2] = new Vector3(utcParent.position.x + spacing,
                utcParent.position.y,
                utcParent.position.z - spacing);
            targets[3] = new Vector3(utcParent.position.x + spacing,
                utcParent.position.y,
                utcParent.position.z + spacing);
            Vector3 target;

            //随机选取一个，并且保证距离Misaki模型足够远
            do
            {
                target = targets[Random.Range(0, 4)];
            } while (Vector3.Distance(target, misaki.transform.position) < 1f);

            utc.transform.LookAt(target);   //面向目标点
            utc.SetBool("Walk", true);      //模型动画变为行走

            utc.transform
                .DOMove(target, 0.5f)      //向目标点移动，速度0.5
                .SetEase(Ease.Linear)               //线性移动
                .SetDelay(1f)                       //1秒以后开始移动
                .SetSpeedBased()
                .OnComplete(OnCompleteUTCWalkAway);     //完成后运行方法
        }

        /// <summary>
        /// UTC走开动作完成。
        /// </summary>
        private void OnCompleteUTCWalkAway()
        {
            utc.SetBool("Walk", false);
            UTCStand();

            MisakiRunToUTC();
        }

        /// <summary>
        /// UTC走向Yuko
        /// </summary>
        private IEnumerator UTCWalkToYuko()
        {
            stateUTC = StateUTC.WalkToYuko;

            yield return new WaitForSeconds(0.2f);


            //将UTC设为根节点游戏对象
            utc.transform.parent = null;

            //面向Yuko
            utc.transform.LookAt(yuko.transform);

            //动作变为行走
            utc.SetBool("Walk", true);

            utcTween = utc.transform
                .DOMove(yuko.transform.position, 0.5f) //向目标点移动
                .SetEase(Ease.Linear)               //线性移动
                .SetDelay(1f)                       //1秒以后开始移动
                .SetSpeedBased();
        }

        /// <summary>
        /// UTC站在Yuko旁边
        /// </summary>
        private void UTCStandWithYuko()
        {
            stateUTC = StateUTC.StandWithYuko;

            //utc移动停止
            utcTween.Kill();
            //停止行走动画
            utc.SetBool("Walk", false);
        }

        /// <summary>
        /// UTC走回原点
        /// </summary>
        private void UTCWalkBack()
        {
            stateUTC = StateUTC.WalkBack;

            //停止移动
            utcTween.Complete();
            //开始行走动画
            utc.SetBool("Walk", true);
            //面向识别图片
            utc.transform.LookAt(utcParent);

            utcTween = utc.transform
                .DOMove(utcParent.position, 0.5f) //返回起点
                .SetEase(Ease.Linear)               //线性移动
                .SetDelay(1f)                       //1秒以后开始移动
                .SetSpeedBased()
                .OnComplete(OnCompleteUTCWalkBack); //完成后运行方法
        }

        /// <summary>
        /// UTC走回原处动作完成
        /// </summary>
        private void OnCompleteUTCWalkBack()
        {
            utc.SetBool("Walk", false);
            UTCRestore();
            UTCStand();
        }

        /// <summary>
        /// UTC恢复初始状态
        /// </summary>
        private void UTCRestore()
        {
            //模型恢复到原有位置和角度
            utc.transform.parent = utcParent;
            utc.transform.localPosition = new Vector3(0f, 0f, 0f);
            utc.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        #endregion

        #region Misaki

        /// <summary>
        /// Misaki状态枚举
        /// </summary>
        private enum StateMisaki
        {
            /// <summary>
            /// 隐藏
            /// </summary>
            Hidden,
            /// <summary>
            /// 站立
            /// </summary>
            Stand,
            /// <summary>
            /// 跑向UTC
            /// </summary>
            RunToUTC,
            /// <summary>
            /// 跑向Yuko
            /// </summary>
            RunToYuko,
            /// <summary>
            /// 站在Yuko旁边
            /// </summary>
            StandWithYuko,
            /// <summary>
            /// 跑回起点
            /// </summary>
            RunBack
        }

        /// <summary>
        /// Misaki状态
        /// </summary>
        private StateMisaki stateMisaki;

        /// <summary>
        /// Misaki控制对象
        /// </summary>
        private Animator misaki;

        /// <summary>
        /// UTC的父节点
        /// </summary>
        private Transform misakiParent;

        /// <summary>
        /// utc移动动画
        /// </summary>
        private Tween misakiTween;

        /// <summary>
        /// Misaki隐藏
        /// </summary>
        private void MisakiHidden()
        {
            stateMisaki = StateMisaki.Hidden;
            misaki.gameObject.SetActive(false);

            MisakiRestore();
        }

        /// <summary>
        /// Misaki站立
        /// </summary>
        private void MisakiStand()
        {
            if (stateMisaki == StateMisaki.RunToUTC)
            {
                misakiTween.Kill();
                misaki.SetBool("Run", false);
            }

            if (stateMisaki == StateMisaki.RunBack)
            {
                MisakiRestore();
            }

            stateMisaki = StateMisaki.Stand;
            misaki.gameObject.SetActive(true);

        }

        /// <summary>
        /// Misaki跑向UTC
        /// </summary>
        private void MisakiRunToUTC()
        {
            stateMisaki = StateMisaki.RunToUTC;

            //将misaki设为根节点游戏对象
            misaki.transform.parent = null;
            //面向UTC
            misaki.transform.LookAt(utc.transform);
            //动画进入跑动
            misaki.SetBool("Run", true);
            //移动到UTC位置
            misakiTween = misaki.transform
                .DOMove(utc.transform.position, 1f) //向UTC移动
                .SetEase(Ease.Linear)   //线性移动
                .SetDelay(1f)           //1秒以后开始移动
                .SetSpeedBased();
        }


        /// <summary>
        /// Misaki跑向Yuko
        /// </summary>
        private IEnumerator MisakiRunToYuko()
        {
            stateMisaki = StateMisaki.RunToYuko;

            yield return new WaitForSeconds(0.2f);

            //将Misaki设为根节点游戏对象
            misaki.transform.parent = null;
            //面向Yuko
            misaki.transform.LookAt(yuko.transform);
            //动作变为行走
            misaki.SetBool("Run", true);

            misakiTween = misaki.transform
                .DOMove(yuko.transform.position, 1f)    //向yukok移动
                .SetEase(Ease.Linear)               //线性移动
                .SetDelay(1f)                       //1秒以后开始移动
                .SetSpeedBased();
        }

        /// <summary>
        /// Misaki站在Yuko旁
        /// </summary>
        private void MisakiStandWithYuko()
        {
            stateMisaki = StateMisaki.StandWithYuko;

            //Misaki移动停止
            misakiTween.Kill();
            //停止行走动画
            misaki.SetBool("Run", false);
        }

        /// <summary>
        /// Misaki跑回起点
        /// </summary>
        private void MisakiRunBack()
        {
            stateMisaki = StateMisaki.RunBack;

            //停止移动
            misakiTween.Complete();
            //开始行走动画
            misaki.SetBool("Run", true);
            //面向识别图片
            misaki.transform.LookAt(misakiParent);

            misakiTween = misaki.transform
                .DOMove(misakiParent.position, 1f)  //返回起点
                .SetEase(Ease.Linear)               //线性移动
                .SetDelay(1f)                       //1秒以后开始移动
                .SetSpeedBased()
                .OnComplete(OnCompleteMisakiRunBack); //完成后运行方法
        }
        /// <summary>
        /// Misaki走回原处动作完成
        /// </summary>
        private void OnCompleteMisakiRunBack()
        {
            misaki.SetBool("Run", false);
            MisakiStand();
        }
        /// <summary>
        /// Misaki恢复初始状态
        /// </summary>
        private void MisakiRestore()
        {
            misaki.transform.parent = misakiParent;
            misaki.transform.localPosition = Vector3.zero;
            misaki.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        #endregion

        private void Start()
        {
            #region Yuko

            //初始化yuKo变量
            yuko = GameObject.Find("itBirds/Yuko").GetComponent<Animator>();

            YukoHidden();

            #endregion Yuko

            #region UTC

            //初始化UTC
            utc = GameObject.Find("itBeauty/UTC").GetComponent<Animator>();
            utcParent = utc.transform.parent;

            UTCHidden();

            #endregion

            #region Misaki

            //初始化Misaki
            misaki = GameObject.Find("itNanoha/Misaki").GetComponent<Animator>();
            misakiParent = misaki.transform.parent;

            MisakiHidden();

            #endregion
        }

        private void Update()
        {
            #region Yuko

            //如果Yuko在Pose状态
            if (stateYuko == StateYuko.Pose)
            {
                //如果动作已经播放完成
                if (yuko.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f
                    && yuko.GetCurrentAnimatorStateInfo(0).IsName("pose"))
                {
                    yukoCoroutine = StartCoroutine("YukoStand");
                }
            }

            #endregion Yuko
        }

        /// <summary>
        /// 图片目标被识别
        /// </summary>
        /// <param name="target">被识别对象</param>
        private void ImageTargetFound(ImageTarget target)
        {
            switch (target.Name)
            {
                case "Birds":
                    //Yuko显示
                    yukoCoroutine = StartCoroutine("YukoStand");

                    //UTC走向Yuko
                    if (stateUTC == StateUTC.Stand
                        || stateUTC == StateUTC.WalkAway)
                    {
                        //UTCWalkToYuko();
                        StartCoroutine("UTCWalkToYuko");
                    }

                    //Misaki跑向Yuko
                    if (stateMisaki == StateMisaki.Stand
                        || stateMisaki == StateMisaki.RunToUTC)
                    {
                        StartCoroutine("MisakiRunToYuko");
                    }
                    break;
                case "Beauty":
                    UTCStand();
                    break;
                case "Nanoha":
                    MisakiStand();
                    break;
            }
        }

        /// <summary>
        /// 图片目标丢失
        /// </summary>
        /// <param name="target">识别对象</param>
        private void ImageTargetLost(ImageTarget target)
        {
            switch (target.Name)
            {
                case "Birds":
                    YukoHidden();
                    if (stateUTC == StateUTC.StandWithYuko
                        || stateUTC == StateUTC.WalkToYuko)
                    {
                        UTCWalkBack();
                    }
                    if (stateMisaki == StateMisaki.StandWithYuko
                        || stateMisaki == StateMisaki.RunToYuko)
                    {
                        MisakiRunBack();
                    }
                    break;
                case "Beauty":
                    UTCHidden();
                    break;
                case "Nanoha":
                    MisakiHidden();
                    break;
            }
        }

        /// <summary>
        /// 模型进入
        /// </summary>
        /// <param name="TriggerInfo">模型信息</param>
        private void ModelTriggerEnter(string ModelInfo)
        {
            if (ModelInfo == "UTC-Misaki"
                && stateUTC == StateUTC.Stand)
            {
                UTCWalkAway();
            }
            if (ModelInfo == "UTC-Yuko"
                && stateUTC == StateUTC.WalkToYuko)
            {
                UTCStandWithYuko();
            }
            if (ModelInfo == "Misaki-UTC"
                && stateMisaki == StateMisaki.RunToUTC)
            {
                MisakiStand();
            }
            if (ModelInfo == "Misaki-Yuko"
                && stateMisaki == StateMisaki.RunToYuko)
            {
                MisakiStandWithYuko();
            }
        }

        /// <summary>
        /// 模型离开
        /// </summary>
        /// <param name="ModelInfo">模型信息</param>
        private void ModelTriggerExit(string ModelInfo)
        {
            //Debug.Log("Trigger Exit");
            //Debug.Log(ModelInfo);
        }
    }
}