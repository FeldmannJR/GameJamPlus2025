using UnityEngine.UIElements;

namespace UI
{
    public class GameEndUI : GameUI
    {
        private Label _label;

        public override void OnOpen(VisualElement root)
        {
            _label = root.Q<Label>("Label");
        }


        public void Show(string message)
        {
            gameObject.SetActive(true);// todo: ui service
            //_label.text = message;
        }
    }
}