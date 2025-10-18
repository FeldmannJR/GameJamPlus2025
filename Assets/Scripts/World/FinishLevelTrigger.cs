using DefaultNamespace;
using UnityEngine;

namespace Scripts
{
    public class FinishLevelTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerControls>(out var pl))
            {
                LevelSystem.Instance.Finish(pl);
            }
        }
    }
}