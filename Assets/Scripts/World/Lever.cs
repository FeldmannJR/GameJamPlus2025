using UnityEngine;

namespace Scripts
{
    public class Lever : InteractableObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

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
    }
}