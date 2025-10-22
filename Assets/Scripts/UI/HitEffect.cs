using UnityEngine;

namespace TDLogic
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private float ttl;

        private void Start()
        {
            Destroy(gameObject, ttl);
        }
    }
}
