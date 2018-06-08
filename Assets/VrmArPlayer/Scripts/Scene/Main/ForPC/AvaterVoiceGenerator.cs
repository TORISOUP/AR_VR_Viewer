using PhotonRx;
using UniRx;
using UnityEngine;
using Zenject;

namespace VrmArPlayer
{
    public class AvaterVoiceGenerator : MonoBehaviour
    {
        [Inject] private Configs _configs;

        void Start()
        {
            if (PhotonNetwork.inRoom)
            {
                PhotonNetwork.Instantiate("AvaterVoiceObject", Vector3.zero, Quaternion.identity, 0, new object[] { _configs.IsVrMode });
            }
            else
            {
                this.OnJoinedRoomAsObservable()
                    .Subscribe(_ =>
                    {
                        PhotonNetwork.Instantiate("AvaterVoiceObject", Vector3.zero, Quaternion.identity, 0, new object[] { _configs.IsVrMode });
                    });
            }
        }
    }
}
