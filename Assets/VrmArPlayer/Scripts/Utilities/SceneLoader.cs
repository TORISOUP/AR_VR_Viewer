using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VrmArPlayer
{
    public static class SceneLoader
    {
        /// <summary>
        /// 前のシーンから引き継いだデータ
        /// </summary>
        public static SceneDataPack PreviousSceneData;

        private static SceneLoadManager _sceneLoadManager;

        private static SceneLoadManager SceneLoadManager
        {
            get
            {
                if (_sceneLoadManager != null) return _sceneLoadManager;
                Initialize();
                return _sceneLoadManager;
            }
        }


        public static void Initialize()
        {
            if (SceneLoadManager.Instance == null)
            {
                var resource = Resources.Load("Utilities/TransitionManager");
                Object.Instantiate(resource);
                _sceneLoadManager = SceneLoadManager.Instance;
            }
        }

        /// <summary>
        /// シーン遷移を行う
        /// </summary>
        /// <param name="scene">次のシーン</param>
        /// <param name="data">次のシーンへ引き継ぐデータ</param>
        /// <param name="additiveLoadScenes">追加でロードするシーン</param>
        public static async Task MoveScene(GameScenes scene,
            SceneDataPack data = null,
            GameScenes[] additiveLoadScenes = null)
        {
            if (data == null)
            {
                data = new DefaultSceneDataPack(SceneLoadManager.CurrentGameScene, additiveLoadScenes);
            }
            await SceneLoadManager.Move(scene, data, additiveLoadScenes);
        }

    }
}
