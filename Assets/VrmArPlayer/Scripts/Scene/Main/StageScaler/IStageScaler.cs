using UniRx;

namespace VrmArPlayer
{
    /// <summary>
    /// 現在のフィールドのサイズと回転を取得できる
    /// </summary>
    interface IStageScaler
    {
        IReadOnlyReactiveProperty<float> StageScale { get; }
        IReadOnlyReactiveProperty<float> StageAngle { get; }
    }

    /// <summary>
    /// フィールドサイズと回転を設定できる
    /// </summary>
    interface IStageScaleChanger
    {
        void SetStageScale(float scale);
        void SetStageRotate(float angle);
    }
}
