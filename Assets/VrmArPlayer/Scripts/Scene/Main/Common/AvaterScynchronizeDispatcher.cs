using RootMotion.FinalIK;
using UnityEngine;
using Zenject;
using UniRx;
using VrmArPlaye;

namespace VrmArPlayer
{
    /// <summary>
    /// アバターに同期用のオブジェクトとかを紐付ける
    /// </summary>
    public class AvaterScynchronizeDispatcher : MonoBehaviour
    {
        private AvaterProvider _avaterProvider;
        private AvaterSynchronizer _synchronizer;
        private Configs _config;

        /// <summary>
        /// VRコントローラのTransform
        /// </summary>
        [SerializeField] private Transform VrHeadTarget;
        [SerializeField] private Transform VrLeftHandTarget;
        [SerializeField] private Transform VrRightHandTarget;


        [Inject]
        private void Injecting(AvaterProvider avaterProvider, AvaterSynchronizer avaterSynchronizer, Configs config)
        {
            _avaterProvider = avaterProvider;
            _synchronizer = avaterSynchronizer;
            _config = config;
            Itinialzie();
        }

        void Itinialzie()
        {
            //アバターの生成イベント
            _avaterProvider.OnSpawenAvater
                .Subscribe(go =>
                {
                    //IK設定
                    SetIK(go);
                    //同期設定
                    SetupSynchronizer(go);
                });
        }

        /// <summary>
        /// アバター生成時に実行
        /// </summary>
        void SetupSynchronizer(GameObject avater)
        {
            //送信時のみに設定される
            if (!_config.IsVrMode) return;
            var animator = avater.GetComponent<Animator>();
            var syncObject = _synchronizer.TargetTransforms;

            //同期オブジェクトにアバターの各部位を登録する
            syncObject[HumanBodyBones.Head].GetComponent<PostureController>()
                .SetTrackingTransform(animator.GetBoneTransform(HumanBodyBones.Head));
            syncObject[HumanBodyBones.LeftHand].GetComponent<PostureController>()
                .SetTrackingTransform(animator.GetBoneTransform(HumanBodyBones.LeftHand));
            syncObject[HumanBodyBones.LeftFoot].GetComponent<PostureController>()
                .SetTrackingTransform(animator.GetBoneTransform(HumanBodyBones.LeftFoot));
            syncObject[HumanBodyBones.RightHand].GetComponent<PostureController>()
                .SetTrackingTransform(animator.GetBoneTransform(HumanBodyBones.RightHand));
            syncObject[HumanBodyBones.RightFoot].GetComponent<PostureController>()
                .SetTrackingTransform(animator.GetBoneTransform(HumanBodyBones.RightFoot));
            syncObject[HumanBodyBones.Spine].GetComponent<PostureController>()
                .SetTrackingTransform(animator.GetBoneTransform(HumanBodyBones.Spine));
        }


        void SetIK(GameObject target)
        {
            var vrik = target.AddComponent<VRIK>();

            // 同期オブジェクト群
            var syncObjects = _synchronizer.TargetTransforms;

            if (_config.IsVrMode)
            {
                //VRモードのとき
                vrik.solver.spine.headTarget = VrHeadTarget;
                vrik.solver.rightArm.target = VrRightHandTarget;
                vrik.solver.leftArm.target = VrLeftHandTarget;
            }
            else
            {
                //ARモードのとき
                vrik.solver.spine.headTarget = syncObjects[HumanBodyBones.Head];
                vrik.solver.rightArm.target = syncObjects[HumanBodyBones.RightHand];
                vrik.solver.leftArm.target = syncObjects[HumanBodyBones.LeftHand];
            }

            vrik.solver.rightArm.stretchCurve = AnimationCurve.Constant(0, 10, 0);
            vrik.solver.leftArm.stretchCurve = AnimationCurve.Constant(0, 10, 0);
            vrik.solver.rightLeg.target = syncObjects[HumanBodyBones.RightFoot];
            vrik.solver.leftLeg.target = syncObjects[HumanBodyBones.LeftFoot];

            vrik.solver.locomotion.footDistance = 0.2f;
            vrik.solver.rightLeg.swivelOffset = -25;
            vrik.solver.leftLeg.swivelOffset = 25;

        }




    }
}