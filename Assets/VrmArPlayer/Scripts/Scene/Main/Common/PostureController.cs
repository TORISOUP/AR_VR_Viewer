using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Logger = UniRx.Diagnostics.Logger;

namespace VrmArPlayer
{
    /// <summary>
    /// 姿勢に一致させる同期オブジェクトの制御
    /// </summary>
    class PostureController : MonoBehaviour
    {

        private Vector3 _originalTargetPosition;
        private Quaternion _originalTargetRotation = Quaternion.identity;

        private IStageScaler _stageScaler;
        private StageController _stageController;
        private Configs _configs;

        private Transform _trackingTarget;

        [SerializeField] private float scaleOffset = 1.0f;
        [SerializeField] private float LerpParam = 10.0f;

        /// <summary>
        /// 姿勢を設定する（受信用）
        /// </summary>
        public void SetTargetPosture(Vector3 targetPost, Quaternion targetRot)
        {
            _originalTargetPosition = targetPost;
            _originalTargetRotation = targetRot;
        }

        /// <summary>
        /// 追従する対象（送信用)
        /// </summary>
        public void SetTrackingTransform(Transform tracking)
        {
            _trackingTarget = tracking;
        }

        [Inject]
        private void Inject(IStageScaler stageScaler, StageController stageController, Configs configs)
        {
            _stageScaler = stageScaler;
            _stageController = stageController;
            _configs = configs;

            if (_configs.IsVrMode)
            {
                //VRの座標に追従
                this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if (_trackingTarget == null) return;
                        transform.SetPositionAndRotation(_trackingTarget.position, _trackingTarget.rotation);
                    });
            }
            else
            {
                //受信データに追従
                this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        //　スケール計算
                        var scale = _stageScaler.StageScale.Value * scaleOffset;

                        // 現在の原点座標
                        var root = _stageController.Root.position;

                        // 線形補間
                        var np = Vector3.Lerp(transform.position, root + _originalTargetPosition * scale,
                            Time.deltaTime * LerpParam);
                        var nr = Quaternion.Lerp(transform.rotation, _originalTargetRotation,
                            Time.deltaTime * LerpParam);

                        transform.SetPositionAndRotation(np, nr);
                    });

            }
        }

    }
}
