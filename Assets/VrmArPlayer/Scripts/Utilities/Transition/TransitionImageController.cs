using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace VrmArPlayer
{
    /// <summary>
    /// 簡単なトランジションエフェクト管理
    /// </summary>
    class TransitionImageController : MonoBehaviour
    {
        private Image _image;

        private Color _onCoverColor = Color.white;
        private Color _offCoverColor = new Color(1, 1, 1, 0);

        [SerializeField] private float _animationSeconds = 1.0f;

        void Awake()
        {
            _image = GetComponent<Image>();
            _image.color = _offCoverColor;
            _image.raycastTarget = false; //イベント貫通
        }

        /// <summary>
        /// 画面を隠すアニメーションを実行する
        /// </summary>
        public async Task CloseTransition()
        {
            await CloseCoroutine();
        }

        private IEnumerator CloseCoroutine()
        {
            _image.color = _offCoverColor;
            _image.raycastTarget = true; //タッチイベントブロック

            var startTime = Time.time;

            while (true)
            {
                var delta = (Time.time - startTime);
                if (delta >= _animationSeconds) break;

                _image.color = Color.Lerp(_offCoverColor, _onCoverColor, delta / _animationSeconds);
                yield return null;
            }

            _image.color = _onCoverColor;
            _image.raycastTarget = true;
        }

        /// <summary>
        /// 画面を戻すアニメーションを実行する
        /// </summary>
        public async Task OpenTransition()
        {
            await OpenCoroutine();
        }

        private IEnumerator OpenCoroutine()
        {
            _image.color = _onCoverColor;
            _image.raycastTarget = true; //タッチイベントブロック

            var startTime = Time.time;

            while (true)
            {
                var delta = (Time.time - startTime);
                if (delta >= _animationSeconds) break;

                _image.color = Color.Lerp(_onCoverColor, _offCoverColor, delta / _animationSeconds);
                yield return null;
            }

            _image.color = _offCoverColor;
            _image.raycastTarget = false; //タッチイベント開放
        }
    }
}
