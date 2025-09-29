using UnityEngine;

namespace TDLogic
{
    public class PopUps : MonoBehaviour
    {
        [SerializeField] private float ttl;

        private void Start()
        {
            Destroy(gameObject, ttl);
        }
    }
}
