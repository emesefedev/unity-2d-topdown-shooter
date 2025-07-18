using System;
using UnityEngine;

namespace Emesefe.MonoBehaviours
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float zoomSpeed = 1f;
        
        private Camera camera;
        
        private Func<Vector3> _getCameraFollowPositionFunc;
        private Func<float> _getCameraZoomFunc;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        public void Setup(Func<Vector3> getCameraFollowPositionFunc, Func<float> getCameraZoomFunc)
        {
            _getCameraFollowPositionFunc = getCameraFollowPositionFunc;
            _getCameraZoomFunc = getCameraZoomFunc;
        }
        
        public void SetCameraFollowPosition(Vector3 cameraFollowPosition)
        {
            SetGetCameraFollowPositionFunc(() => cameraFollowPosition);
        }

        public void SetGetCameraFollowPositionFunc(Func<Vector3> getCameraFollowPositionFunc)
        {
            _getCameraFollowPositionFunc = getCameraFollowPositionFunc;
        }

        public void SetCameraZoom(float cameraZoom)
        {
            SetGetCameraZoomFunc(() => cameraZoom);
        }
        
        public void SetGetCameraZoomFunc(Func<float> getCameraZoomFunc)
        {
            _getCameraZoomFunc = getCameraZoomFunc;
        }

        private void HandleMovement()
        {
            Vector3 cameraFollowPosition = _getCameraFollowPositionFunc();
            cameraFollowPosition.z = transform.position.z;
            
            Vector3 cameraMoveDirection = (cameraFollowPosition - transform.position).normalized;
            float distance = Vector3.Distance(cameraFollowPosition, transform.position);
            
            // Avoid overshooting the target
            if (distance > 0)
            {
                Vector3 newCameraPosition = transform.position + cameraMoveDirection * (speed * distance * Time.deltaTime);
                
                float distanceAfterMovement = Vector3.Distance(newCameraPosition, cameraFollowPosition);
                if (distanceAfterMovement > distance)
                {
                    // We have overshot the target
                    newCameraPosition = cameraFollowPosition;
                }
                
                transform.position = newCameraPosition;
            }
        }

        private void HandleZoom()
        {
            float zoom = _getCameraZoomFunc();
            float zoomDifference = zoom - camera.orthographicSize;
            
            camera.orthographicSize += zoomDifference * speed * Time.deltaTime;

            if (zoomDifference > 0)
            {
                if (camera.orthographicSize > zoom)
                {
                    camera.orthographicSize = zoom;
                }
            }
            else
            {
                if (camera.orthographicSize < zoom)
                {
                    camera.orthographicSize = zoom;
                }
            }
        }
    }
}
