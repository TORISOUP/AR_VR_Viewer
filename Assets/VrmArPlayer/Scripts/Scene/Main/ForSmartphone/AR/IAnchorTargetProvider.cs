using System;
using GoogleARCore;
using UniRx;
using UnityEngine;

namespace VrmArPlayer
{
    /// <summary>
    /// アンカー（AR表示にアバターを表示する座標）を提供する
    /// </summary>
    interface IAnchorTargetProvider
    {
        IObservable<Unit> OnAncherChangedAsObservable { get; }
        Transform CurrentAncherTransform { get; }
    }
}
