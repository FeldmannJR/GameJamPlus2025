using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class GameUI : MonoBehaviour
    {
        
        public virtual void OnOpen(VisualElement root)
        {
        }


        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.AssignElementResults(this);
            OnOpen(root);
        }
    }
}