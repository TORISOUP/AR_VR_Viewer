namespace VrmArPlayer
{
    /// <summary>
    /// シーンをまたいでデータを受け渡すときに利用する
    /// </summary>
    public abstract class SceneDataPack
    {
        /// <summary>
        /// 前のシーン
        /// </summary>
        public abstract GameScenes PreviousGameScene { get; }

        public abstract GameScenes[] PreviousAdditiveScene { get; }
    }

    public class DefaultSceneDataPack : SceneDataPack
    {
        public override GameScenes PreviousGameScene { get; }

        public override GameScenes[] PreviousAdditiveScene { get; }

        public DefaultSceneDataPack(GameScenes prev, GameScenes[] additive)
        {
            PreviousGameScene = prev;
            PreviousAdditiveScene = additive;
        }
    }
}