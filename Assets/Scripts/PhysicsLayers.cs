using UnityEngine;

namespace DefaultNamespace
{
    public class PhysicsLayers
    {
        public static string Interactable = "Interactable";
        public static LayerMask Mask_Interactable = LayerMask.GetMask(Interactable);
    }
}