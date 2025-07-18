using UnityEngine;

namespace Emesefe.MonoBehaviours
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private GameObject player;

        [Tooltip("If enabled, move the camera with the arrow keys")]
        [SerializeField] private bool manualCameraMovement;
        private Vector3 cameraFollowPosition;
        
        [SerializeField] private bool zoomWithMouseWheelOn;
        [Tooltip("If enabled, zoom In / Out with Q and E keys")]
        [SerializeField] private bool zoomOn;
        private float zoom = 20f;
        private float zoomStep = 10f;
        private float zoomMin = 10f;
        private float zoomMax = 100f;
        
        [Tooltip("If enabled, move the camera when the cursor gets close to the edges of the screen")]
        [SerializeField] private bool edgeScrollingOn;
        private float edgeSize = 20f; // 20 pixels
        private float moveAmount = 20f;

        private void Start()
        {
            cameraFollow.Setup(() => cameraFollowPosition, () => zoom);
        }

        private void Update()
        {
            HandleManualCameraMovement();
            // CameraFollowPlayer();

            HandleEdgeScrolling();

            HandleZoom();
            HandleZoomWithMouseWheel();
        }

        private void CameraFollowPlayer()
        {
            cameraFollowPosition = player.transform.position;
        }

        private void HandleEdgeScrolling()
        {
            if (!edgeScrollingOn) return;

            if (Input.mousePosition.x > Screen.width - edgeSize)
            {
                cameraFollowPosition.x += moveAmount * Time.deltaTime;
            }

            if (Input.mousePosition.x < edgeSize)
            {
                cameraFollowPosition.x -= moveAmount * Time.deltaTime;
            }

            if (Input.mousePosition.y > Screen.height - edgeSize)
            {
                cameraFollowPosition.y += moveAmount * Time.deltaTime;
            }

            if (Input.mousePosition.y < edgeSize)
            {
                cameraFollowPosition.y -= moveAmount * Time.deltaTime;
            }
        }

        private void HandleManualCameraMovement()
        {
            if (!manualCameraMovement) return;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                cameraFollowPosition.y += moveAmount * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                cameraFollowPosition.y -= moveAmount * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                cameraFollowPosition.x += moveAmount * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cameraFollowPosition.x -= moveAmount * Time.deltaTime;
            }
        }
        
        private void HandleZoom()
        {
            if (!zoomOn) return;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ZoomIn();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ZoomOut();
            }
        }

        private void HandleZoomWithMouseWheel()
        {
            if (!zoomWithMouseWheelOn) return;

            if (Input.mouseScrollDelta.y > 0)
            {
                ZoomIn(0.05f);
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                ZoomOut(0.05f);
            }
        }

        private void ZoomIn(float zoomSpeedMultiplier = 1)
        {
            zoom -= zoomStep * zoomSpeedMultiplier;
            if (zoom < zoomMin)
            {
                zoom = zoomMin;
            }
        }

        private void ZoomOut(float zoomSpeedMultiplier = 1)
        {
            zoom += zoomStep * zoomSpeedMultiplier;
            if (zoom > zoomMax)
            {
                zoom = zoomMax;
            }
        }
    }
}