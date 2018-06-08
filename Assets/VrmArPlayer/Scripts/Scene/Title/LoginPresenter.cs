using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace VrmArPlayer
{
    public class LoginPresenter : MonoBehaviour
    {
        [Inject] private LoginManager _manager;

        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Text _errorMessage;

        void Start()
        {
            var gate =
            _inputField.OnValueChangedAsObservable()
                .Select(x => x.Length > 0)
                .ToReactiveProperty();

            // ログインボタンが押されたときの処理
            // AsyncReactiveCommandを使って、非同期処理中はボタンを無効化する
            _loginButton.BindToOnClick(gate, _ =>
            {
                _errorMessage.text = "";
                return _manager
                    .Login(_inputField.text) //ログインする（返り値はTask<IResult<L,R>>）
                    .ToObservable(Scheduler.MainThread)
                    .ForEachAsync(result =>
                    {
                        if (result.IsFailure)
                        {
                            _errorMessage.text = result.TryGetFailureValue;
                        }
                        else
                        {
                            // ログイン完了したらシーン遷移
                            _manager.MoveScene();
                        }
                    });
            });
        }
    }
}
