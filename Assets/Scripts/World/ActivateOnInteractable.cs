using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class ActivateOnInteractable : InteractableObject
    {
        public List<InteractableObject> Required;
        public bool Invert;
        public bool ToggleOnce; // Unable to turn off if the conditions are not met
        private bool _toggledOn = false;

        public UnityEvent TurnOff;
        public UnityEvent TurnOn;

        private void Start()
        {
            foreach (var interactableObject in Required)
            {
                interactableObject.OnStateChanged += UpdateState;
            }
        }

        public void UpdateState()
        {
            var open = Required.All(req => req.On);
            if (Invert) open = !open;
            if (!open && ToggleOnce)
            {
                return;
            }

            if (open)
            {
                _toggledOn = true;
            }

            if (open)
            {
                TurnOn?.Invoke();
            }
            else
            {
                TurnOff.Invoke();
            }
        }
    }
}