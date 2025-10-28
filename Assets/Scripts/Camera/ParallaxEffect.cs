using UnityEngine;

namespace KingdomScratch
{
    public class ParallaxBackground : MonoBehaviour
    {
        public float parallaxEffectMultiplier; // Adjust this for each layer
        private Transform cameraTransform;
        private Vector3 lastCameraPosition;

        void Start()
        {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;
        }

        void LateUpdate()
        {
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);
            lastCameraPosition = cameraTransform.position;
        }
    }
}