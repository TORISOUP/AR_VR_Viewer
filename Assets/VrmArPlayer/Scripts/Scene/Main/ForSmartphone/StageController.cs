using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace VrmArPlayer
{
    /// <summary>
    /// AR側の表示状態を管理する
    /// </summary>
    class StageController : MonoBehaviour
    {
        private IStageScaler _stageScaler;
        private AvaterProvider _avaterProvider;
        private IAnchorTargetProvider _anchorTargetProvider;

        [SerializeField] private Transform _rooTransform;
        public Transform Root => _rooTransform;


        [Inject]
        private void Injecting(IStageScaler scaler, AvaterProvider avaterProvider, IAnchorTargetProvider anchorTargetProvider)
        {
            _stageScaler = scaler;
            _avaterProvider = avaterProvider;
            _anchorTargetProvider = anchorTargetProvider;

            _avaterProvider.OnSpawenAvater.Subscribe(go =>
            {
                go.transform.SetParent(_rooTransform, false);
            });

            //おおきさ
            _stageScaler.StageScale
                .Subscribe(x =>
                {
                    _rooTransform.localScale = Vector3.one * x;
                });

            //むき（未使用）
            _stageScaler.StageAngle
                .Subscribe(x =>
                {
                    _rooTransform.rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.identity;
                });


            // タップされた場所を基準座標にする
            _anchorTargetProvider.OnAncherChangedAsObservable
                .Subscribe(_ =>
                {
                    _rooTransform.position = _anchorTargetProvider.CurrentAncherTransform.position;
                });
        }



    }
}
