
using RootMotion.FinalIK;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VRM;
using Zenject;


namespace VrmArPlayer
{
    /// <summary>
    /// 入力イベントから表情とか変える
    /// </summary>
    class AvaterController : MonoBehaviour
    {
        [Inject] private IInputEventProvider _inputEventProvider;


        [Inject] private IStageScaler _stageScaler;
        [Inject] private OVRLipSyncContext _lipSyncContext;

        private FingerController _fingerController;
        private VRMBlendShapeProxy _blendShapeProxy;

        private VRIK _vrik;

        private void Start()
        {
            _blendShapeProxy = GetComponent<VRMBlendShapeProxy>();

            SetupFingers();
            SetBlendShape();
            SetIK();
            SetLipSync();

            //LateUpdateで表情を反映
            this.LateUpdateAsObservable().Subscribe(_ => _blendShapeProxy.Apply());
        }

        /// <summary>
        /// 指の曲げ制御
        /// </summary>
        private void SetupFingers()
        {
            _fingerController = gameObject.AddComponent<FingerController>();

            var targetLeft = 0.0f;
            var currentLeft = 0.0f;
            var targetRight = 0.0f;
            var currentRight = 0.0f;

            //右手
            _inputEventProvider.RightTrigger.Subscribe(x =>
            {
                targetRight = x ? 1 : 0;
            });

            //左手
            _inputEventProvider.LeftTrigger.Subscribe(x =>
            {
                targetLeft = x ? 1 : 0;
            });

            this.LateUpdateAsObservable()
                .Subscribe(_ =>
                {
                    //Lerp
                    currentLeft = Mathf.Lerp(currentLeft, targetLeft, 0.3f);
                    currentRight = Mathf.Lerp(currentRight, targetRight, 0.3f);

                    _fingerController.FingerRotation(FingerController.FingerType.RightAll, currentRight);
                    _fingerController.FingerRotation(FingerController.FingerType.LeftAll, currentLeft);

                });
        }

        /// <summary>
        /// リップシンク
        /// </summary>
        private void SetLipSync()
        {
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var currentFrame = _lipSyncContext.GetCurrentPhonemeFrame();
                    _blendShapeProxy.SetValue(BlendShapePreset.A, currentFrame.Visemes[(int)OVRLipSync.Viseme.aa], false);
                    _blendShapeProxy.SetValue(BlendShapePreset.I, currentFrame.Visemes[(int)OVRLipSync.Viseme.ih], false);
                    _blendShapeProxy.SetValue(BlendShapePreset.U, currentFrame.Visemes[(int)OVRLipSync.Viseme.ou], false);
                    _blendShapeProxy.SetValue(BlendShapePreset.E, currentFrame.Visemes[(int)OVRLipSync.Viseme.E], false);
                    _blendShapeProxy.SetValue(BlendShapePreset.O, currentFrame.Visemes[(int)OVRLipSync.Viseme.oh], false);
                });
        }

        /// <summary>
        /// IK設定
        /// </summary>
        private void SetIK()
        {
            _vrik = GetComponent<VRIK>();

            if (_vrik == null) return;

            var defaultFoot = _vrik.solver.locomotion.footDistance;
            var defaultStep = _vrik.solver.locomotion.stepThreshold;

            // 描画スケールが変更されたら股幅などを変更する
            _stageScaler.StageScale
                .SubscribeWithState2(defaultFoot, defaultStep, (x, defaultFootDistance, defaultStepThreshold) =>
                {
                    _vrik.solver.locomotion.footDistance = defaultFootDistance * x;
                    _vrik.solver.locomotion.stepThreshold = defaultStepThreshold * x;
                });
        }

        /// <summary>
        /// 表情
        /// </summary>
        private void SetBlendShape()
        {
            var current = Vector2.zero;
            var input = Vector2.zero;

            // 右手・左手、両方のタッチパッドの座標をマージする
            // (どちらでも操作可能にする)
            _inputEventProvider.LeftTouchPad
                .Merge(_inputEventProvider.RightTouchPad)
                .Subscribe(x =>
                {
                    input = x;
                });

            this.UpdateAsObservable().Subscribe(_ =>
            {
                current = Vector2.Lerp(current, input, 0.2f);

                _blendShapeProxy.SetValue(BlendShapePreset.Fun, Vector2.Dot(current, Vector2.right), false);
                _blendShapeProxy.SetValue(BlendShapePreset.Angry, Vector2.Dot(current, Vector2.up), false);
                _blendShapeProxy.SetValue(BlendShapePreset.Joy, Vector2.Dot(current, Vector2.left), false);
                _blendShapeProxy.SetValue(BlendShapePreset.Sorrow, Vector2.Dot(current, Vector2.down), false);
            });

        }


    }
}
