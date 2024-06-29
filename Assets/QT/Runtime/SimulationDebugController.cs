using UnityEngine;

// ReSharper disable InvertIf

namespace QT.Runtime
{
    /// <summary>
    /// Aims to provide mouse and keyboard input to ease debugging of quadtree API.
    /// </summary>
    [DisallowMultipleComponent]
    public class SimulationDebugController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private SimulationField simulationField;

        private void Update()
        {
            // 'LMB' input callback.
            if (Input.GetMouseButtonDown(0))
            {
                // Find the positions that are nearby the mouse.
                var mouseScreenPosition = Input.mousePosition;
                mouseScreenPosition.z = mainCamera.nearClipPlane;

                // Convert the screen mouse position to world mouse position.
                var convertedMousePosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
                var mousePositionInWorldCoordinates = new Vector2(convertedMousePosition.x, convertedMousePosition.y);
                var positionsNearbyMouse = simulationField.Quadtree.GetPositionsNearby(mousePositionInWorldCoordinates);

                Debug.Log($"Found {positionsNearbyMouse.Count} position nearby mouse");
            }
        }
    }
}