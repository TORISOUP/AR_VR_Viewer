using System;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;
using VRM;
using Zenject;
using Logger = UniRx.Diagnostics.Logger;

namespace VrmArPlayer
{
    /// <summary>
    /// アバターキャラクタを生成する
    /// </summary>
    public class AvaterProvider : MonoBehaviour
    {
        [Inject] private DiContainer _container;

        public IObservable<GameObject> OnSpawenAvater => _spawnSubject;

        private AsyncSubject<GameObject> _spawnSubject = new AsyncSubject<GameObject>();

        private string fileName = "AliciaSolid.vrm";

        async void Start()
        {
            var filePath = Application.streamingAssetsPath + "/" + fileName;
            var www = new WWW(filePath);
            await www; // WWWでvrmファイルを読み込み

            //VRMファイルをGameObjectに変換
            var loadGameObject = await VRMImporter.LoadVrmAsync(www.bytes);

            loadGameObject.transform.localScale = Vector3.one * 1.13f; //サイズ補正

            // AvaterControllerをAddする
            // DIを実行したいのでコンテナ経由でAddComponentする
            _container.InstantiateComponent<AvaterController>(loadGameObject);

            _spawnSubject.OnNext(loadGameObject);
            _spawnSubject.OnCompleted();
        }
    }
}
