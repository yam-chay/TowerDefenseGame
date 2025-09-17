using UnityEngine;

namespace TDLogic
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject target;
        [SerializeField] private float xOffSet;
        [SerializeField] private float yOffSet;
        [SerializeField] private float zOffSet;
        [SerializeField] private float smoothSpeed;

        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            Vector3 desiredPosition = new Vector3(target.transform.position.x + xOffSet, target.transform.position.y + yOffSet, zOffSet);
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;

        }
    }
}
