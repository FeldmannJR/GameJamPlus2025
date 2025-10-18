using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PositionConverterSystem : MonoBehaviour
    {
        public static PositionConverterSystem Instance;
        public BoxCollider2D BoxA;
        public BoxCollider2D BoxB;


        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Returns true if a conversion occured, otherwise returl false and the same position
        /// </summary>
        /// <param name="vector3"></param>
        /// <param name="convertedPosition"></param>
        /// <returns></returns>
        public bool TryConvert(Vector3 vector3, out Vector3 convertedPosition)
        {
            if (BoxA.bounds.Contains(vector3))
            {
                var diff = vector3 - BoxA.bounds.min;

                
                convertedPosition =  BoxB.bounds.min + diff;
                return true;
            }

            if (BoxB.bounds.Contains(vector3))
            {
                var diff = vector3 - BoxB.bounds.min;

                convertedPosition =  BoxA.bounds.min + diff;
                return true;
            }

            convertedPosition = vector3;
            return false;
        }
    }
}