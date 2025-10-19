using System;
using Audio;
using AYellowpaper.SerializedCollections;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class Lever : InteractableObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _imageToShowRenderer;

        [SerializeField] private GameAudioBehaviour _turnOn;
        [SerializeField] private GameAudioBehaviour _turnOff;

        public Sprite CustomOnSprite;
        public Sprite CustomOffSprite;

        [Serializable]
        public enum LeverColor
        {
            Green,
            Orange,
            Blue
        }

        [Serializable]
        public class ColorConfig
        {
            public Sprite On;
            public Sprite Off;
        }

        public SerializedDictionary<LeverColor, ColorConfig> Colors = new();


        public Sprite ImageToShow;


        public LeverColor CurrentColor;


        [SerializeField] private bool _canTurnOffManualy = true;


        protected override void OnValidate()
        {
            SetColor(CurrentColor, false);
        }

        public void SetColor(LeverColor color, bool on)
        {
            if (CustomOnSprite && on)
            {
                _spriteRenderer.sprite = CustomOnSprite;
                return;
            }

            if (CustomOffSprite && !on)
            {
                _spriteRenderer.sprite = CustomOffSprite;
                return;
            }

            var obj = Colors[color];
            _spriteRenderer.sprite = on ? obj.On : obj.Off;
        }


        public void UpdateColor()
        {
            SetColor(CurrentColor, On);
        }

        public override void OnStartClient()

        {
            this.OnInteracted += OnInteracted_O;
            this.OnStateChanged += OnStateChangedImpl;
            this.OnLocalInteracted += OnLocalImpl;
            UpdateColor();
        }

        private void OnLocalImpl()
        {
            if (Time.time > 3f) // hack for not playing when starting
            {
                if (On)
                {
                    _turnOff?.PlayOneShot();
                }
                else
                {
                    _turnOn?.PlayOneShot();
                }
            }
        }

        private void OnStateChangedImpl()
        {
            UpdateColor();
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
            if (_imageToShowRenderer == null) return;
            if (ImageToShow != null)
            {
                _imageToShowRenderer.sprite = ImageToShow;
            }

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