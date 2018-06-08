using System.Collections.Generic;
using PhotonRx;
using UnityEngine;
using Zenject;
using UniRx;

namespace VrmArPlayer
{
    /// <summary>
    /// 同期情報を管理する
    /// </summary>
    public class AvaterSynchronizer : MonoBehaviour
    {
        private AvaterProvider _avaterProvider;
        private IInputEventProvider _inputEventProvider;
        private IInputSetable _inputSetable;

        [Inject]
        private void Injecting(
            AvaterProvider avaterProvider,
            IInputEventProvider inputEventProvider,
            IInputSetable inputSetable)
        {
            _avaterProvider = avaterProvider;
            _inputEventProvider = inputEventProvider;
            _inputSetable = inputSetable;

            Initialize();
        }

        [SerializeField] private Transform Head;
        [SerializeField] private Transform LeftHand;
        [SerializeField] private Transform LeftFoot;
        [SerializeField] private Transform RightHand;
        [SerializeField] private Transform RightFoot;
        [SerializeField] private Transform Spine;

        private PhotonView _photonView;

        private Dictionary<HumanBodyBones, Transform> _targetTransforms;

        public Dictionary<HumanBodyBones, Transform> TargetTransforms
        {
            get
            {
                if (_targetTransforms != null) return _targetTransforms;
                _targetTransforms = new Dictionary<HumanBodyBones, Transform>
                {
                    [HumanBodyBones.Head] = Head,
                    [HumanBodyBones.LeftHand] = LeftHand,
                    [HumanBodyBones.RightHand] = RightHand,
                    [HumanBodyBones.LeftFoot] = LeftFoot,
                    [HumanBodyBones.RightFoot] = RightFoot,
                    [HumanBodyBones.Spine] = Spine
                };
                return _targetTransforms;

            }
        }

        private Dictionary<HumanBodyBones, PostureController> _postureControllers 
            = new Dictionary<HumanBodyBones, PostureController>();

        private bool _isInitialized = false;

        void Initialize()
        {
            _avaterProvider.OnSpawenAvater
                .Take(1)
                .Subscribe(_ =>
                {
                    //初期化（先にGetComponentしてキャッシュしておく）
                    _postureControllers[HumanBodyBones.Head] =
                        TargetTransforms[HumanBodyBones.Head].GetComponent<PostureController>();
                    _postureControllers[HumanBodyBones.LeftHand] =
                        TargetTransforms[HumanBodyBones.LeftHand].GetComponent<PostureController>();
                    _postureControllers[HumanBodyBones.LeftFoot] =
                        TargetTransforms[HumanBodyBones.LeftFoot].GetComponent<PostureController>();
                    _postureControllers[HumanBodyBones.RightFoot] =
                        TargetTransforms[HumanBodyBones.RightFoot].GetComponent<PostureController>();
                    _postureControllers[HumanBodyBones.RightHand] =
                        TargetTransforms[HumanBodyBones.RightHand].GetComponent<PostureController>();
                    _postureControllers[HumanBodyBones.Spine] =
                        TargetTransforms[HumanBodyBones.Spine].GetComponent<PostureController>();

                    _isInitialized = true;
                });
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!_isInitialized) return;

            if (stream.isWriting)
            {
                //送信側（VR）
                stream.SendNext(Head.position);
                stream.SendNext(Head.rotation);
                stream.SendNext(LeftHand.position);
                stream.SendNext(LeftHand.rotation);
                stream.SendNext(LeftFoot.position);
                stream.SendNext(LeftFoot.rotation);
                stream.SendNext(RightHand.position);
                stream.SendNext(RightHand.rotation);
                stream.SendNext(RightFoot.position);
                stream.SendNext(RightFoot.rotation);
                stream.SendNext(Spine.position);
                stream.SendNext(Spine.rotation);

                stream.SendNext(_inputEventProvider.RightTrigger.Value);
                stream.SendNext(_inputEventProvider.LeftTrigger.Value);
                stream.SendNext(_inputEventProvider.RightTouchPad.Value);
                stream.SendNext(_inputEventProvider.LeftTouchPad.Value);

            }
            else
            {
                //受信側(AR)
                _postureControllers[HumanBodyBones.Head].SetTargetPosture((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
                _postureControllers[HumanBodyBones.LeftHand].SetTargetPosture((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
                _postureControllers[HumanBodyBones.LeftFoot].SetTargetPosture((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
                _postureControllers[HumanBodyBones.RightHand].SetTargetPosture((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
                _postureControllers[HumanBodyBones.RightFoot].SetTargetPosture((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
                _postureControllers[HumanBodyBones.Spine].SetTargetPosture((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());

                var rightTriger = (bool)stream.ReceiveNext();
                var leftTriger = (bool)stream.ReceiveNext();
                var rightTouch = (Vector2)stream.ReceiveNext();
                var leftTouch = (Vector2)stream.ReceiveNext();

                // ネットワーク同期の結果を反映する
                _inputSetable.SetParams(rightTriger, leftTriger, leftTouch, rightTouch);
            }
        }


    }
}
