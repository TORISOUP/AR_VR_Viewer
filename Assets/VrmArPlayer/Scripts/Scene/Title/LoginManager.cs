using System;
using System.Threading;
using System.Threading.Tasks;
using PhotonRx;
using UniRx;
using UnityEngine;
using VrmArPlaye;

namespace VrmArPlayer
{
    public class LoginManager : MonoBehaviour
    {
        private void Start()
        {
            PhotonNetwork.autoJoinLobby = true;
        }

        public async Task<IResult<string, Unit>> Login(string roomName)
        {
            var createRoom = true;
#if UNITY_ANDROID && !UNITY_EDITOR
            createRoom = false;
#endif

            var result = await PhotonUtils.Login(roomName, createRoom);

            if (result.IsSuccess)
            {
                PhotonNetwork.isMessageQueueRunning = false;
            }

            return result;
        }

        public void MoveScene()
        {
#if UNITY_ANDROID
            var target = GameScenes.ForSmartphone;
#else
            var target = GameScenes.ForPC;
#endif

            switch (target)
            {
                case GameScenes.ForPC:
                    SceneLoader.MoveScene(target).FireAndForget();
                    break;
                case GameScenes.ForSmartphone:
                    SceneLoader.MoveScene(GameScenes.ForSmartphone, null, new[] { GameScenes.ARCoreScene }).FireAndForget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }

    }
}
