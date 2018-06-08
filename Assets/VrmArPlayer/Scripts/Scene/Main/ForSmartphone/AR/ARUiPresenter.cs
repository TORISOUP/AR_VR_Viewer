using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace VrmArPlayer
{
    public class ARUiPresenter : MonoBehaviour
    {
        [Inject] private ArViewManager _manager;

        [SerializeField] private Slider _scaleSlider;
        [SerializeField] private Text _scaleLabel;

        [SerializeField] private Slider _rotateSlider;
        [SerializeField] private Text _rotateLabel;

        void Start()
        {
            _scaleSlider.OnValueChangedAsObservable()
                .Subscribe(x =>
                {
                    var f = x * 0.1f;
                    _scaleLabel.text = $"x{f}";
                    _manager.SetViewScale(f);
                });

        }

    }
}
