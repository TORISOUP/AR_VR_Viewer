using UnityEngine;

namespace VrmArPlayer
{
    /// <summary>
    /// AR用のカメラの座標を返す
    /// </summary>
    public class DummyArCameraTransformProvider : MonoBehaviour , IArCameraTransformProvider
    {
        // VR側ではARカメラ座標がないので適当な値を返す
        public Transform CameraTransform => transform;
    }
}
