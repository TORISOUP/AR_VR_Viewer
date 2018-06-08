using System;
using SteamVRInputObservables;
using UniRx;
using UnityEngine;

namespace VrmArPlayer
{
    /// <summary>
    /// SteamVRからの入力イベントを発行する
    /// </summary>
    public class VrInputEventProvider : IInputEventProvider, IInputSetable
    {
        readonly Lazy<IReadOnlyReactiveProperty<bool>> _rightTrigger =
            new Lazy<IReadOnlyReactiveProperty<bool>>(ObservableSteamInput.RightTriggerPress);
        readonly Lazy<IReadOnlyReactiveProperty<bool>> _leftTrigger =
            new Lazy<IReadOnlyReactiveProperty<bool>>(ObservableSteamInput.LeftTriggerPress);
        readonly Lazy<IReadOnlyReactiveProperty<Vector2>> _rightTouchPad =
            new Lazy<IReadOnlyReactiveProperty<Vector2>>(ObservableSteamInput.RightTouchPosition);
        readonly Lazy<IReadOnlyReactiveProperty<Vector2>> _leftTouchPad =
            new Lazy<IReadOnlyReactiveProperty<Vector2>>(ObservableSteamInput.LeftTouchPosition);

        public IReadOnlyReactiveProperty<bool> RightTrigger => _rightTrigger.Value;
        public IReadOnlyReactiveProperty<bool> LeftTrigger => _leftTrigger.Value;
        public IReadOnlyReactiveProperty<Vector2> RightTouchPad => _rightTouchPad.Value;
        public IReadOnlyReactiveProperty<Vector2> LeftTouchPad => _leftTouchPad.Value;

        public void SetParams(bool isRightTrigger, bool isLeftTrigger, Vector2 leftTouchPad, Vector2 rightTouchPad)
        {
            // do nothing
        }
    }
}
