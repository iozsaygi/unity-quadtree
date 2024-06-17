using UnityEngine;

namespace Quadtree.Runtime
{
    /// <summary>
    /// Basic universe/scene for entities in it.
    /// </summary>
    [DisallowMultipleComponent]
    public class SimulationField : MonoBehaviour
    {
        [SerializeField] private Bounds bounds;

        private void OnDrawGizmosSelected()
        {
            // Rendering the extents of the simulation field.
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bounds.center, bounds.extents);
        }
    }
}