using UniRx;
using UnityEngine;

namespace VrmArPlayer
{
    /// <summary>
    /// 入力イベントを発行する
    /// </summary>
    interface IInputEventProvider
    {
        IReadOnlyReactiveProperty<bool> RightTrigger { get; }
        IReadOnlyReactiveProperty<bool> LeftTrigger { get; }
        IReadOnlyReactiveProperty<Vector2> RightTouchPad { get; }
        IReadOnlyReactiveProperty<Vector2> LeftTouchPad { get; }
    }

    /// <summary>
    /// 入力イベントを強制的に上書きできる
    /// </summary>
    interface IInputSetable
    {
        void SetParams(bool isRightTrigger, bool isLeftTrigger, Vector2 leftTouchPad, Vector2 rightTouchPad);
    }
}
