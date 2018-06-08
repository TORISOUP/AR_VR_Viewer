using System.Diagnostics;
using System.Threading.Tasks;
using PhotonRx;
using UniRx;

namespace VrmArPlaye
{
    /// <summary>
    /// Photon用の便利機能
    /// </summary>
    public static class PhotonUtils
    {
        /// <summary>
        /// サーバに接続していないなら接続する（Editor時のみ）
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void ConnectToServerDebug()
        {
            if (PhotonNetwork.connectionState != ConnectionState.Disconnected) return;
            PhotonNetwork.ConnectUsingSettings("DEBUG");
        }

        /// <summary>
        /// サーバに接続済みか
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            return PhotonNetwork.connectionState != ConnectionState.Disconnected;
        }

        /// <summary>
        /// サーバに接続していないなら接続する
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="playerName"></param>
        public static void ConnectToServerEasy(string serverName, string playerName)
        {
            if (PhotonNetwork.connectionState != ConnectionState.Disconnected) return;
            PhotonNetwork.playerName = playerName;
            PhotonNetwork.ConnectUsingSettings(serverName);
        }


        /// <summary>
        /// カスタムプロパティを上書きする
        /// </summary>
        public static void Override(this PhotonPlayer player, string key, object value)
        {
            var cp = player.customProperties;
            cp[key] = value;
            player.SetCustomProperties(cp);
        }

        /// <summary>
        /// カスタムプロパティを上書きする
        /// </summary>
        public static void Override(this Room room, string key, object value)
        {
            var cp = room.CustomProperties;
            cp[key] = value;
            room.SetCustomProperties(cp);
        }

        /// <summary>
        /// マスタークライアントであるか？
        /// サーバに接続していない場合はtrue
        /// </summary>
        /// <returns></returns>
        public static bool IsMasterClient => 
            PhotonNetwork.connectionState == ConnectionState.Disconnected || PhotonNetwork.isMasterClient;

        public static string PunErrorToMessage(short errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.MaxCcuReached:
                    return string.Format("サーバが満員です: {0}", errorCode);
                case ErrorCode.NoRandomMatchFound:
                    return string.Format("参加可能な部屋が見つかりませんでした: {0}", errorCode);
                case ErrorCode.GameIdAlreadyExists:
                    return string.Format("同じ部屋名の部屋がすでに存在しています: {0}", errorCode);
                case ErrorCode.GameClosed:
                    return string.Format("対象の部屋には現在参加できません: {0}", errorCode);
                case ErrorCode.GameDoesNotExist:
                    return string.Format("対象の部屋が存在しません: {0}", errorCode);
                case ErrorCode.GameFull:
                    return string.Format("部屋が満員です: {0}", errorCode);
                default:
                    var status = string.Format("不明なエラーが発生しました: {0}", errorCode);
                    return status;
            }
        }


        /// <summary>
        /// サーバにログインし、部屋にも接続する
        /// </summary>
        /// <param name="roomName">接続する部屋名</param>
        /// <param name="createIfNoExists">部屋がない場合に作成するか</param>
        /// <returns></returns>
        public static async Task<IResult<string, Unit>> Login(string roomName, bool createIfNoExists)
        {
            var connected = await Connect();

            if (!connected.IsSuccess) return ToFailure("サーバに接続できませんでした");

            var joined = await JoinRoom(roomName, createIfNoExists);

            return joined;
        }

        /// <summary>
        /// Photonのサーバに接続する
        /// </summary>
        private static async Task<IResult<string, Unit>> Connect()
        {
            // サーバに接続
            var connect = await PhotoTask.ConnectUsingSettings("v1");

            if (connect.IsFailure)
            {
                return ToFailure(connect.TryGetFailureValue.ToString());
            }

            return ToSuccess();
        }

        /// <summary>
        /// 部屋に参加する
        /// </summary>
        private static async Task<IResult<string, Unit>> JoinRoom(string roomName, bool createIfNoExists)
        {

            var joined = await PhotoTask.JoinRoom(roomName);
            if (joined.IsSuccess) return ToSuccess();

            if (!createIfNoExists) return ToFailure("No room.");

            // 部屋を作って参加する
            var created = await PhotoTask.CreateRoom(roomName, null, null, null);

            if (!created.IsSuccess)
            {
                //失敗
                return ToFailure(created.TryGetFailureValue.ToString());
            }

            return ToSuccess();
        }

        private static IResult<string, Unit> ToSuccess()
        {
            return Success.Create<string, Unit>(Unit.Default);
        }

        private static IResult<string, Unit> ToFailure(string message)
        {
            return Failure.Create<string, Unit>(message);
        }
    }
}
