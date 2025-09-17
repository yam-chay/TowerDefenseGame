using UnityEngine;

namespace TDLogic
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float xOffSet;
        [SerializeField] private float yOffSet;
        [SerializeField] private float zOffSet;
        [SerializeField] private float smoothSpeed;
        private Transform target;

        private Vector3 velocity = Vector3.zero;

        private void Awake()
        {
            if(Player.Instance != null)
            {
                target = Player.Instance.transform;
            }
        }
        void LateUpdate()
        {
            if (Player.Instance != null)
            {
                target = Player.Instance.transform;
                Vector3 desiredPosition = new Vector3(target.position.x + xOffSet, target.position.y + yOffSet, zOffSet);
                Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                mainCamera.transform.position = smoothedPosition;
            }
        }
    }
}
