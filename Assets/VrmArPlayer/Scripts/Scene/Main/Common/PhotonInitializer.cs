using UnityEngine;
using VrmArPlaye;

namespace VrmArPlayer
{
    public class PhotonInitializer : MonoBehaviour
    {
        void Start()
        {
            PhotonNetwork.isMessageQueueRunning = true;

#if UNITY_EDITOR
            if (!PhotonUtils.IsConnected())
            {
                PhotonUtils.Login("test1", true).FireAndForget();
            }
#endif
        }
    }
}
