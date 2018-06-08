using UniRx;
using UnityEngine;
using VrmArPlayer;

namespace VrmArPlayer
{
    /// <summary>
    /// VR用（ステージのサイズと向きは常に固定）
    /// </summary>
    class FixedStageScaler : MonoBehaviour, IStageScaler
    {
        private readonly ReadOnlyReactiveProperty<float> _stageScale
            = new FloatReactiveProperty(1.0f).ToReadOnlyReactiveProperty();

        private readonly ReadOnlyReactiveProperty<float> _stageAngle
            = new FloatReactiveProperty(0.0f).ToReadOnlyReactiveProperty();

        public IReadOnlyReactiveProperty<float> StageScale => _stageScale;
        public IReadOnlyReactiveProperty<float> StageAngle => _stageAngle;
    }
}
