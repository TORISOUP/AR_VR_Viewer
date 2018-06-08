using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace VrmArPlayer
{
    public class DebugLineTraser : MonoBehaviour
    {
        private Transform target;
        private IArCameraTransformProvider _cameraTransformProvider;

        [Inject]
        void Injecting(IArCameraTransformProvider cameraTransformProvider, AvaterProvider provider)
        {
            _cameraTransformProvider = cameraTransformProvider;
            provider.OnSpawenAvater
                .Subscribe(go =>
                {
                    target = go.transform;
                });
        }

        void Start()
        {
            var lineRender = GetComponent<LineRenderer>();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (target != null && _cameraTransformProvider.CameraTransform != null)
                    {
                        lineRender.SetPositions(new Vector3[] { target.position, _cameraTransformProvider.CameraTransform.position });
                    }
                });
        }

    }
}
