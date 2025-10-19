using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class Lever : InteractableObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _imageToShowRenderer;


        public Sprite ImageToShow;


        public Sprite ActiveSprite;
        public Sprite DeactivatedSprite;

        [SerializeField] private bool _canTurnOffManualy = true;

        private Sprite GetCurrentSprite()
        {
            return On ? ActiveSprite : DeactivatedSprite;
        }

        public override void OnStartClient()

        {
            this.OnInteracted += OnInteracted_O;
            _spriteRenderer.sprite = GetCurrentSprite();
            this.OnStateChanged += OnStateChangedImpl;
        }

        private void OnStateChangedImpl()
        {
            _spriteRenderer.sprite = GetCurrentSprite();
        }

        private void UpdateState()
        {
            _spriteRenderer.sprite = GetCurrentSprite();
        }


        public void OnInteracted_O()
        {
            Debug.Log("INTERACTED");
            if (On && !_canTurnOffManualy)
            {
                return;
            }

            SetOn(!On);
        }

        public void ShowFrame(bool show)
        {
            if (ImageToShow == null) return;

            _imageToShowRenderer.sprite = ImageToShow;
            _imageToShowRenderer.DOColor(show ? Color.white : Color.clear, 0.3f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!this.gameObject.activeSelf) return;
            if (other.TryGetComponent<PlayerControls>(out _))
            {
                ShowFrame(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!this.gameObject.activeSelf) return;
            if (other.TryGetComponent<PlayerControls>(out _))
            {
                ShowFrame(false);
            }
        }
    }
}