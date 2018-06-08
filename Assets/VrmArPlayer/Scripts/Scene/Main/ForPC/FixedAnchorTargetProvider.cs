using System;
using UniRx;
using UnityEngine;

namespace VrmArPlayer
{
    /// <summary>
    /// VR側用（アンカーは常に固定）
    /// </summary>
    class FixedAnchorTargetProvider : MonoBehaviour, IAnchorTargetProvider
    {
        public IObservable<Unit> OnAncherChangedAsObservable => Observable.Return(Unit.Default);
        public Transform CurrentAncherTransform => transform;
    }
}
