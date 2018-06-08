using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace VrmArPlayer
{
    /// <summary>
    /// Avater位置を追従する
    /// </summary>
    class AvaterVoiceTracer : MonoBehaviour
    {
        private Transform target;

        [Inject]
        private void Injecting(AvaterProvider avaterProvider)
        {
            avaterProvider.OnSpawenAvater
                .Subscribe(go =>
                {
                    target = go.transform;
                });
        }

        void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => target != null)
                .Subscribe(_ =>
                {
                    transform.position = target.position;
                });
        }
    }
}
