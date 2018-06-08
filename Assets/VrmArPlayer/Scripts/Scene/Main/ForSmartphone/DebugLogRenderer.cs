using System.Linq;
using System.Text;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace VrmArPlayer
{
    public class DebugLogRenderer : MonoBehaviour
    {
        [SerializeField] private Text text;
        private string[] stringArray;

        void Start()
        {
            stringArray = new string[15];
            int current = -1;
            ObservableLogger.Listener.Subscribe(log =>
            {
                if (++current >= stringArray.Length)
                {
                    current = 0;
                }
                stringArray[current] = log.LoggerName + ":" + log.Message;

                var sb = new StringBuilder();
                for (var i = 0; i < stringArray.Length; i++)
                {
                    sb.Append(stringArray[(current + 1 + i) % stringArray.Length] + System.Environment.NewLine);
                }
                text.text = sb.ToString();
            });
        }

    }
}
