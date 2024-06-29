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
                var mousePositionInWorldCoordinates = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var positionsNearbyMouse = simulationField.Quadtree.GetPositionsNearby(mousePositionInWorldCoordinates);

                foreach (var positionNearbyMouse in positionsNearbyMouse)
                {
                    Debug.Log(
                        $"{positionNearbyMouse} is nearby of current mouse position, total count of positions that are nearby of mouse is {positionsNearbyMouse.Count}");
                }
            }
        }
    }
}