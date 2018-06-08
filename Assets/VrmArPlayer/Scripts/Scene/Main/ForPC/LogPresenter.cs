using UniRx.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace VrmArPlayer
{
    class LogPresenter : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private void Start()
        {
            ObservableLogger.Listener.Subscribe(log =>
            {
                _text.text = log.LoggerName + ":" + log.Message;
            });
        }
    }
}
