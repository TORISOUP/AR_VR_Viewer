using System;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VrmArPlayer
{
    /// <summary>
    /// シーン遷移を管理する
    /// </summary>
    public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
    {
        [SerializeField] private TransitionImageController _transitionImageController;

        public GameScenes CurrentGameScene { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(transform.parent.gameObject);
        }

        /// <summary>
        /// シーン遷移を実行する
        /// </summary>
        /// <param name="nextScene">次のシーン</param>
        /// <param name="data">次のシーンへ引き継ぐデータ</param>
        /// <param name="additiveLoadScenes">追加ロードするシーン</param>
        public async Task Move(GameScenes nextScene,
            SceneDataPack data,
            GameScenes[] additiveLoadScenes)
        {

            SceneLoader.PreviousSceneData = data;
            CurrentGameScene = nextScene;

            //画面をトランジションエフェクトで隠す
            await _transitionImageController.CloseTransition();

            //シーン遷移
            await SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            //追加シーンがある場合は一緒に読み込む
            if (additiveLoadScenes != null)
            {
                await Task.WhenAll(additiveLoadScenes.Select(scene =>
                     SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive).AsObservable().ToTask()));
            }

            // 未使用アセットすべて開放
            await Resources.UnloadUnusedAssets();

            // GC実行
            GC.Collect();

            // 画面を戻す
            await _transitionImageController.OpenTransition();
        }
    }
}