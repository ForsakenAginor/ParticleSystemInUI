using System.Collections;
using UnityEngine;

namespace ParticleSystemInUi
{
    public class UIEffectAdapter : MonoBehaviour
    {
        [SerializeField] private RectTransform _defaultEffectParent;
        [SerializeField] private RectTransform _effectTexture;
        [SerializeField] private UIEffect _interactEffect;
        [SerializeField] private UIEffect _highlightEffect;
        [SerializeField] private UIEffect _clickEffect;

        private WaitWhileCached _waitWhile;
    
        private void Awake()
        {
            _waitWhile = new WaitWhileCached(() => false);
        }

        /// <summary>
        /// Plays an effect under a UI element, useful when working with layout groups.
        /// Temporarily reparents the effect texture to avoid layout group interference.
        /// </summary>
        /// <param name="parent">The UI element transform that will be the center of the effect.</param>
        /// <param name="endParent">The parent transform to reparent the effect to after positioning, preventing it from being affected by layout groups.</param>
        /// <param name="size">The size of the effect. Default is 400f.</param>
        public void PlayEffect(RectTransform parent, Transform endParent, float size = 400f)
        {
            gameObject.SetActive(true);
            _effectTexture.SetParent(parent);
            _effectTexture.anchoredPosition = Vector3.zero;
            _effectTexture.anchorMax = Vector2.one * 0.5f;
            _effectTexture.anchorMin = Vector2.one * 0.5f;
            _effectTexture.SetParent(endParent);
            _effectTexture.sizeDelta = new Vector2(size, size);
            _interactEffect.Play();
            StartCoroutine(WaitWhilePlaying());
        }

        /// <summary>
        /// Plays an effect centered on the specified UI element.
        /// </summary>
        /// <param name="parent">The UI element transform that will be the center of the effect.</param>
        /// <param name="size">The size of the effect. Default is 400f.</param>
        public void PlayEffect(RectTransform parent, float size = 400f)
        {
            gameObject.SetActive(true);
            _effectTexture.SetParent(parent);
            _effectTexture.anchoredPosition = Vector3.zero;
            _effectTexture.anchorMax = Vector2.one * 0.5f;
            _effectTexture.anchorMin = Vector2.one * 0.5f;
            _effectTexture.sizeDelta = new Vector2(size, size);
            _interactEffect.Play();
            StartCoroutine(WaitWhilePlaying());
        }

        /// <summary>
        /// Plays the highlight effect under a UI element, useful when working with layout groups.
        /// Temporarily reparents the highlight effect texture to avoid layout group interference.
        /// </summary>
        /// <param name="parent">The UI element transform that will be the center of the highlight effect.</param>
        /// <param name="endParent">The parent transform to reparent the highlight effect to after positioning, preventing it from being affected by layout groups.</param>
        /// <param name="size">The size of the highlight effect. Default is 400f.</param>
        public void PlayHighlightEffect(RectTransform parent, Transform endParent, float size = 400f)
        {
            gameObject.SetActive(true);
            _effectTexture.SetParent(parent);
            _effectTexture.anchoredPosition = Vector3.zero;
            _effectTexture.anchorMax = Vector2.one * 0.5f;
            _effectTexture.anchorMin = Vector2.one * 0.5f;
            _effectTexture.SetParent(endParent);
            _effectTexture.sizeDelta = new Vector2(size, size);
            _highlightEffect.Play();
            StartCoroutine(WaitWhilePlaying());
        }

        /// <summary>
        /// Plays the highlight effect centered on the specified UI element.
        /// </summary>
        /// <param name="parent">The UI element transform that will be the center of the highlight effect.</param>
        /// <param name="size">The size of the highlight effect. Default is 400f.</param>
        public void PlayHighlightEffect(RectTransform parent, float size = 400f)
        {
            gameObject.SetActive(true);
            _effectTexture.SetParent(parent);
            _effectTexture.anchoredPosition = Vector3.zero;
            _effectTexture.anchorMax = Vector2.one * 0.5f;
            _effectTexture.anchorMin = Vector2.one * 0.5f;
            _effectTexture.sizeDelta = new Vector2(size, size);
            _highlightEffect.Play();
            StartCoroutine(WaitWhilePlaying());
        }

        /// <summary>
        /// Plays a click effect at the specified position relative to the given parent.
        /// </summary>
        /// <param name="parent">The UI element transform that will be the parent of the effect.</param>
        /// <param name="position">The anchored position where the click effect should appear.</param>
        /// <param name="size">The size of the click effect. Default is 50f.</param>
        public void PlayClickEffect(RectTransform parent, Vector2 position, float size = 50f)
        {
            gameObject.SetActive(true);
            _effectTexture.SetParent(_defaultEffectParent);
            _effectTexture.anchoredPosition = position;

            _effectTexture.anchorMax = Vector2.zero;
            _effectTexture.anchorMin = Vector2.zero;
        
            _effectTexture.SetParent(parent);
            _effectTexture.sizeDelta = new Vector2(size, size);
            _clickEffect.Play();
            StartCoroutine(WaitWhilePlaying());
        }

        /// <summary>
        /// Stops all currently playing effects immediately.
        /// This includes click, interact, and highlight effects.
        /// </summary>
        public void StopAllEffects()
        {
            _clickEffect.Stop();
            _interactEffect.Stop();
            _highlightEffect.Stop();
        }

        private IEnumerator WaitWhilePlaying()
        {
            _waitWhile.UpdateCondition(() => _clickEffect.IsActive() || _interactEffect.IsActive() || _highlightEffect.IsActive());
            yield return _waitWhile;
            yield return null;
            gameObject.SetActive(false);
        }
    }
}