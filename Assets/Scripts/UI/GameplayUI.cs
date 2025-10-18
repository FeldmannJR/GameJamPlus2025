using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameplayUI : GameUI
    {
        [SerializeField] private CommunicationSystem _communicationSystem;

        private TextField _textField;
        private Button _button;
        private VisualElement _messagesContainer;


        public override void OnOpen(VisualElement root)
        {
            _textField = root.Q<TextField>("Text");
            _button = root.Q<Button>("Button");
            _messagesContainer = root.Q<VisualElement>("MessagesContainer");
            _button.clicked += OnButtonClicked;
            _communicationSystem.OnMessage += OnReceivedMessage;
            _messagesContainer.Clear();
        }

        private void OnReceivedMessage(string obj)
        {
            var ve = new Label(obj);
            ve.AddToClassList("message");
            _messagesContainer.Add(ve);
        }

        private void OnButtonClicked()
        {
            _communicationSystem.SendTextMessage(_textField.value);
            _textField.value = "";
        }
    }
}