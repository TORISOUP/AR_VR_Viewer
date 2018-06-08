using UniRx;
using UnityEngine;
using VrmArPlayer;

namespace VrmArPlayer
{
    /// <summary>
    /// AR用（表示のサイズと向きを変更できる）
    /// </summary>
    class FlexibleStageScaler : MonoBehaviour, IStageScaler, IStageScaleChanger
    {
        [SerializeField]
        private FloatReactiveProperty _stageScale = new FloatReactiveProperty(1.0f);

        [SerializeField]
        private FloatReactiveProperty _stageAngle = new FloatReactiveProperty(0.0f);

        public IReadOnlyReactiveProperty<float> StageScale => _stageScale;
        public IReadOnlyReactiveProperty<float> StageAngle => _stageAngle;
        public void SetStageScale(float scale)
        {
            _stageScale.Value = scale;
        }

        public void SetStageRotate(float angle)
        {
            _stageAngle.Value = angle;
        }
    }
}
