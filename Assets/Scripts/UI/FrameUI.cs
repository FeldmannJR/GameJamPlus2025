using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FrameUI : GameUI
    {
        private VisualElement _frame;
        private Button _close;
        private Button _interact;

        private Action _onInteract;

        public override void OnOpen(VisualElement root)
        {
            _interact.clicked += OnConfirm;
            _close.clicked += Close;
        }

        private void OnConfirm()
        {
            _onInteract?.Invoke();
            Close();
        }

        private void Close()
        {
            this.gameObject.SetActive(false);
        }

        public void SetData(Sprite sprite, Action onAccept)
        {
            _onInteract = onAccept;
            _frame.style.backgroundImage = new StyleBackground(sprite);
        }
    }
}