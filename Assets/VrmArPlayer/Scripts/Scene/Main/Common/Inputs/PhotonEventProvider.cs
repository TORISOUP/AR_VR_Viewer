using UniRx;
using UnityEngine;
using VrmArPlayer;

namespace VrmArPlayer
{
    /// <summary>
    /// ネットワーク同期を前提としたInputProvider
    /// </summary>
    class PhotonEventProvider :MonoBehaviour, IInputEventProvider, IInputSetable
    {
        public IReadOnlyReactiveProperty<bool> RightTrigger => _rightTrigger;
        public IReadOnlyReactiveProperty<bool> LeftTrigger => _leftTrigger;
        public IReadOnlyReactiveProperty<Vector2> RightTouchPad => _rightTouchPad;
        public IReadOnlyReactiveProperty<Vector2> LeftTouchPad => _leftTouchPad;

        private BoolReactiveProperty _rightTrigger = new BoolReactiveProperty();
        private BoolReactiveProperty _leftTrigger = new BoolReactiveProperty();
        private ReactiveProperty<Vector2> _rightTouchPad = new ReactiveProperty<Vector2>();
        private ReactiveProperty<Vector2> _leftTouchPad = new ReactiveProperty<Vector2>();

        /// <summary>
        /// ネットワーク同期で送られてきた値を入力イベントとして発行する
        /// </summary>
        public void SetParams(bool isRightTrigger, bool isLeftTrigger, Vector2 leftTouchPad, Vector2 rightTouchPad)
        {
            _rightTrigger.Value = isRightTrigger;
            _leftTrigger.Value = isLeftTrigger;
            _rightTouchPad.Value = rightTouchPad;
            _leftTouchPad.Value = leftTouchPad;
        }
    }
}
