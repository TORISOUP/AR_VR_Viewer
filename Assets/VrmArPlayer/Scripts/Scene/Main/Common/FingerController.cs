using System;
using System.Collections.Generic;
using UnityEngine;

namespace VrmArPlayer
{
    /// <summary>
    /// 指の曲げ制御
    /// </summary>
    public class FingerController : MonoBehaviour
    {
        // 指の場所の定数
        public enum FingerType
        {
            LeftIndex,
            LeftMiddle,
            LeftRing,
            LeftLittle,
            RightIndex,
            RightMiddle,
            RightRing,
            RightLittle,
            LeftThumb,
            RightThumb,
            LeftAll,
            RightAll,
            All
        }


        private static readonly Dictionary<FingerType, Dictionary<HumanBodyBones, Vector3>> FingerBoneMap = new Dictionary<FingerType, Dictionary<HumanBodyBones, Vector3>>
        {
            {FingerType.LeftIndex, new Dictionary<HumanBodyBones,Vector3>
                {
                    { HumanBodyBones.LeftIndexProximal,         new Vector3(0f,0f,80f) },
                    { HumanBodyBones.LeftIndexIntermediate,     new Vector3(0f,0f,70f) },
                    { HumanBodyBones.LeftIndexDistal,           new Vector3(0f,0f,90f) },
                }
            },
            {FingerType.LeftMiddle, new Dictionary<HumanBodyBones,Vector3>
                {
                    { HumanBodyBones.LeftMiddleProximal,        new Vector3(0f,0f,80f) },
                    { HumanBodyBones.LeftMiddleIntermediate,    new Vector3(0f,0f,70f) },
                    { HumanBodyBones.LeftMiddleDistal,          new Vector3(0f,0f,90f) },
                }
            },
            {FingerType.LeftRing, new Dictionary<HumanBodyBones,Vector3>
                {
                    { HumanBodyBones.LeftRingProximal,          new Vector3(0f,0f,80f) },
                    { HumanBodyBones.LeftRingIntermediate,      new Vector3(0f,0f,70f) },
                    { HumanBodyBones.LeftRingDistal,            new Vector3(0f,0f,90f) },
                }
            },
            {FingerType.LeftLittle, new Dictionary<HumanBodyBones, Vector3>
                {
                    { HumanBodyBones.LeftLittleProximal,        new Vector3(0f,0f,80f) },
                    { HumanBodyBones.LeftLittleIntermediate,    new Vector3(0f,0f,70f) },
                    { HumanBodyBones.LeftLittleDistal,          new Vector3(0f,0f,90f) },
                }
            },

            {FingerType.RightIndex, new Dictionary<HumanBodyBones, Vector3>
                {
                    { HumanBodyBones.RightIndexProximal,        new Vector3(0f,0f,-80f) },
                    { HumanBodyBones.RightIndexIntermediate,    new Vector3(0f,0f,-70f) },
                    { HumanBodyBones.RightIndexDistal,          new Vector3(0f,0f,-90f) },
                }
            },
            {FingerType.RightMiddle, new Dictionary<HumanBodyBones, Vector3>
                {
                    { HumanBodyBones.RightMiddleProximal,       new Vector3(0f,0f,-80f) },
                    { HumanBodyBones.RightMiddleIntermediate,   new Vector3(0f,0f,-70f) },
                    { HumanBodyBones.RightMiddleDistal,         new Vector3(0f,0f,-90f) },
                }
            },
            {FingerType.RightRing, new Dictionary<HumanBodyBones, Vector3>
                {
                    { HumanBodyBones.RightRingProximal,         new Vector3(0f,0f,-80f) },
                    { HumanBodyBones.RightRingIntermediate,     new Vector3(0f,0f,-70f) },
                    { HumanBodyBones.RightRingDistal,           new Vector3(0f,0f,-90f) },
                }
            },
            {FingerType.RightLittle, new Dictionary<HumanBodyBones, Vector3>
                {
                    { HumanBodyBones.RightLittleProximal,       new Vector3(0f,0f,-80f) },
                    { HumanBodyBones.RightLittleIntermediate,   new Vector3(0f,0f,-70f) },
                    { HumanBodyBones.RightLittleDistal,         new Vector3(0f,0f,-90f) },
                }
            },

            {FingerType.LeftThumb, new Dictionary<HumanBodyBones, Vector3>
                {
                    { HumanBodyBones.LeftThumbProximal,         new Vector3(0f,0f,0f) },
                    { HumanBodyBones.LeftThumbIntermediate,     new Vector3(43f,28f,55f) },
                    { HumanBodyBones.LeftThumbDistal,           new Vector3(34f,-56f,7f) },
                }
            },

            {FingerType.RightThumb, new Dictionary<HumanBodyBones, Vector3>
                {
                    { HumanBodyBones.RightThumbProximal,        new Vector3(0f,0f,0f) },
                    { HumanBodyBones.RightThumbIntermediate,    new Vector3(43f,-28f,-55f) },
                    { HumanBodyBones.RightThumbDistal,          new Vector3(34f,56f,-7f) },
                }
            },
        };


        private Dictionary<HumanBodyBones, Transform> mecanimBones = new Dictionary<HumanBodyBones, Transform>();

        private List<float> FingerNowVal = new List<float>();


        public void Awake()
        {
            // ボーンマップ生成
            MapBones();

            // 現在の指の値をすべて0にする
            for (int i = 0; i < Enum.GetNames(typeof(FingerType)).Length; i++)
            {
                FingerNowVal.Add(0f);
            }
        }


        /// <summary>
        /// 指を動かす
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="val"></param>
        public void FingerRotation(FingerType ft, float val)
        {
            switch (ft)
            {
                case FingerType.All:
                    FingerLeftAllRotation(val);
                    FingerRightAllRotation(val);
                    break;
                case FingerType.LeftAll:
                    FingerLeftAllRotation(val);
                    break;
                case FingerType.RightAll:
                    FingerRightAllRotation(val);
                    break;
                default:
                    FingerSeparateRotation(ft, val);
                    break;
            }
        }

        /// <summary>
        /// 左手全部
        /// </summary>
        /// <param name="val"></param>
        void FingerLeftAllRotation(float val)
        {
            FingerSeparateRotation(FingerType.LeftIndex, val);
            FingerSeparateRotation(FingerType.LeftLittle, val);
            FingerSeparateRotation(FingerType.LeftMiddle, val);
            FingerSeparateRotation(FingerType.LeftRing, val);
            FingerSeparateRotation(FingerType.LeftThumb, val);
        }

        /// <summary>
        /// 右手全部
        /// </summary>
        /// <param name="val"></param>
        void FingerRightAllRotation(float val)
        {
            FingerSeparateRotation(FingerType.RightIndex, val);
            FingerSeparateRotation(FingerType.RightLittle, val);
            FingerSeparateRotation(FingerType.RightMiddle, val);
            FingerSeparateRotation(FingerType.RightRing, val);
            FingerSeparateRotation(FingerType.RightThumb, val);
        }



        /// <summary>
        /// 指定した指を動かす
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="val">0-1</param>
        void FingerSeparateRotation(FingerType ft, float val)
        {
         //   val = Mathf.Clamp01(val);
            var _finger = FingerBoneMap[ft];

            foreach (var _obj in _finger.Keys)
            {
                mecanimBones[_obj].localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, _finger[_obj], val));
            }

            // 値を記憶
            FingerNowVal[(int)ft] = val;

        }

        /// <summary>
        /// ボーンマップを生成
        /// </summary>
        void MapBones()
        {
            Animator animatorComponent = GetComponent<Animator>();

            foreach (var _finger in FingerBoneMap.Values)
            {
                foreach (var _obj in _finger.Keys)
                {
                    mecanimBones.Add(_obj, animatorComponent.GetBoneTransform(_obj).transform);
                }
            }
        }
    }
}
