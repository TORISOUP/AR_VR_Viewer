using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace VrmArPlayer
{
    /// <summary>
    /// 座標同期するカメラ
    /// </summary>
    public class SynchronizeCamera : MonoBehaviour
    {
        private Transform _target;
        private IAnchorTargetProvider _anchorTargetProvider;
        private IStageScaler _stageScaler;
        [SerializeField] private GameObject _cameraModel;

        private bool _isInitialized;

        void Awake()
        {
            if (SceneManager.sceneCount > 1)
            {
                var scene = SceneManager.GetSceneAt(1);
                SceneManager.MoveGameObjectToScene(gameObject, scene);
            }
        }

        [Inject]
        private void Inject(
            IArCameraTransformProvider cameraTransformProvider,
            IAnchorTargetProvider anchorTargetProvider,
            IStageScaler stageScaler            )
        {
            _target = cameraTransformProvider.CameraTransform;
            _anchorTargetProvider = anchorTargetProvider;
            _stageScaler = stageScaler;

            // 初期化終わったフラグ
            _isInitialized = true;
        }

        void Start()
        {
            if (PhotonView.Get(this).isMine)
            {
                _cameraModel.SetActive(false);
                this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if (_target != null && _anchorTargetProvider.CurrentAncherTransform != null)
                        {
                            var ancher = _anchorTargetProvider.CurrentAncherTransform;
                            transform.position = _target.position - ancher.position;
                            transform.rotation = ancher.rotation * _target.rotation;
                        }
                    });
            }
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!_isInitialized) return;

            if (stream.isWriting)
            {
                if (_target != null && _anchorTargetProvider.CurrentAncherTransform != null)
                {
                    var ancher = _anchorTargetProvider.CurrentAncherTransform;
                    stream.SendNext(_stageScaler.StageScale.Value);
                    stream.SendNext((_target.position - ancher.position));
                    stream.SendNext(ancher.rotation * _target.rotation);
                }
            }
            else
            {
                var scale = (float)stream.ReceiveNext();
                var pos = (Vector3)stream.ReceiveNext();
                var rot = (Quaternion)stream.ReceiveNext();
                transform.SetPositionAndRotation(5 * (pos / scale), rot);
                transform.localScale = 0.5f * Vector3.one / scale;
            }
        }
    }
}
