using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VrmArPlayer;

namespace VrmArPlayer
{
    /// <summary>
    /// キー入力をInputEventにする（デバッグ用）
    /// </summary>
    class KeybordEventProvider : MonoBehaviour, IInputEventProvider, IInputSetable
    {
        public IReadOnlyReactiveProperty<bool> RightTrigger => _rightTrigger;
        public IReadOnlyReactiveProperty<bool> LeftTrigger => _leftTrigger;
        public IReadOnlyReactiveProperty<Vector2> RightTouchPad => _rightTouchPad;
        public IReadOnlyReactiveProperty<Vector2> LeftTouchPad => _leftTouchPad;

        private BoolReactiveProperty _rightTrigger = new BoolReactiveProperty();
        private BoolReactiveProperty _leftTrigger = new BoolReactiveProperty();
        private ReactiveProperty<Vector2> _rightTouchPad = new ReactiveProperty<Vector2>();
        private ReactiveProperty<Vector2> _leftTouchPad = new ReactiveProperty<Vector2>();

        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    _rightTrigger.Value = Input.GetKey(KeyCode.Z);
                    _leftTrigger.Value = Input.GetKey(KeyCode.X);
                });

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var v = Vector2.zero;
                    if (Input.GetKey(KeyCode.W)) v += Vector2.up;
                    if (Input.GetKey(KeyCode.A)) v += Vector2.left;
                    if (Input.GetKey(KeyCode.S)) v += Vector2.down;
                    if (Input.GetKey(KeyCode.D)) v += Vector2.right;
                    _leftTouchPad.Value = v;
                });

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var v = Vector2.zero;
                    if (Input.GetKey(KeyCode.T)) v += Vector2.up;
                    if (Input.GetKey(KeyCode.F)) v += Vector2.left;
                    if (Input.GetKey(KeyCode.G)) v += Vector2.down;
                    if (Input.GetKey(KeyCode.H)) v += Vector2.right;
                    _rightTouchPad.Value = v;
                });
        }

        public void SetParams(bool isRightTrigger, bool isLeftTrigger, Vector2 leftTouchPad, Vector2 rightTouchPad)
        {
            // do nothing
        }
    }
}
