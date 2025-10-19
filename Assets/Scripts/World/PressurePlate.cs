using DefaultNamespace;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class PressurePlate : InteractableObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Sprite ActiveSprite;
        public Sprite DeactivatedSprite;

        public UnityEvent OnTrigerred;

        private void Start()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (!this.gameObject.activeSelf) return;
            if (ActiveSprite == null || DeactivatedSprite == null) return;
            _spriteRenderer.sprite = On ? ActiveSprite : DeactivatedSprite;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!this.gameObject.activeSelf) return;
            if (TryGetPlayer(other, out _))
            {
                SetOn(true);
                UpdateState();
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!this.gameObject.activeSelf) return;
            if (TryGetPlayer(other, out _))
            {
                OnTrigerred?.Invoke();
                SetOn(false);
                UpdateState();
            }
        }

        private bool TryGetPlayer(Collider2D collider2D, out PlayerControls retu)
        {
            if (collider2D.TryGetComponent<PlayerControls>(out var pl))
            {
                retu = pl;
                return true;
            }

            if (collider2D.transform.parent.TryGetComponent<PlayerControls>(out var pl2))
            {
                retu = pl2;
                return true;
            }

            retu = null;
            return false;
        }
    }
}