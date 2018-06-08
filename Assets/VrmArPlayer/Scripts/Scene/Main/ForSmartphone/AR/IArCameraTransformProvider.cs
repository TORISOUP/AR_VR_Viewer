using UnityEngine;

namespace VrmArPlayer
{
    interface IArCameraTransformProvider
    {
        Transform CameraTransform { get; }
    }
}
