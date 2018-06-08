using PhotonRx;
using UniRx;
using UnityEngine;
using Zenject;

namespace VrmArPlayer
{
    /// <summary>
    /// VoiceのAudioSourceを取り扱う
    /// </summary>
    class AvaterVoiceController : MonoBehaviour
    {

        [Inject]
        void Inject(OVRLipSyncContext context)
        {
            if(PhotonView.Get(this).isMine) return; //自身の生成物の場合は無視
            context.audioSource = GetComponent<AudioSource>();
        }
    }
}
