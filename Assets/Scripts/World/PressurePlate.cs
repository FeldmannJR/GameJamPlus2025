using System;
using Audio;
using AYellowpaper.SerializedCollections;
using DefaultNamespace;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class PressurePlate : InteractableObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private GameAudioBehaviour _press;
        [SerializeField] private GameAudioBehaviour _leave;

        [Serializable]
        public enum PPColors
        {
            White,
            Blue,
            Yellow,
            Orange,
            Pink,
            Purple,
            Green
        }

        [Serializable]
        public class ColorConfig
        {
            public GameObject PressedGameObject;
            public GameObject IdleGameObject;
        }

        public SerializedDictionary<PPColors, ColorConfig> Colors = new();




        public PPColors Color = PPColors.White;
        public UnityEvent OnTrigerred;

        protected override void OnValidate()
        {
            SetColor(false);
            base.OnValidate();
        }

        private void Start()
        {
            UpdateState();
        }


        private void UpdateState()
        {
            if (!this.gameObject.activeSelf) return;
            SetColor(On);
        }

        public void SetColor(bool active)
        {
            foreach (var (_, v) in Colors)
            {
                v.IdleGameObject.SetActive(false);
                v.PressedGameObject.SetActive(false);
            }

            if (Colors.Count == 0) return;
            var obj = Colors[Color];
            obj.PressedGameObject.SetActive(active);
            obj.IdleGameObject.SetActive(!active);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!this.gameObject.activeSelf) return;
            if (TryGetPlayer(other, out var pl))
            {
                SetOn(true);
                UpdateState();
                if (pl.IsOwner)
                    if (Time.time > 3f) // hack for not playing when starting
                    {
                        _press?.PlayOneShot();
                    }
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!this.gameObject.activeSelf) return;
            if (TryGetPlayer(other, out var pl))
            {
                OnTrigerred?.Invoke();
                SetOn(false);
                UpdateState();
                if (pl.IsOwner)
                    if (Time.time > 3f) // hack for not playing when starting
                    {
                        _leave?.PlayOneShot();
                    }
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