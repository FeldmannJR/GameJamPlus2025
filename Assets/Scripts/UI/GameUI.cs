using System;
using DefaultNamespace;
using UnityEditor.Callbacks;
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
            OnOpen(GetComponent<UIDocument>().rootVisualElement);
        }
    }
}