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
        [SerializeField] private GameObject simulationEntityPrefab;
        [SerializeField] private byte maximumNumberOfAllowedSimulationEntities;

        private void Start()
        {
            // Generate simulation entity prefabs at random positions within simulation bounds.
            for (byte i = 0; i < maximumNumberOfAllowedSimulationEntities; i++)
            {
                // Calculate random position within simulation bounds.
                var randomPositionWithinSimulationBounds = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y), 0.0f);

                var simulationEntityInstance = Instantiate(simulationEntityPrefab, randomPositionWithinSimulationBounds,
                    Quaternion.identity);

                simulationEntityInstance.transform.SetParent(transform, true);

                // Generate random color for simulation entity.
                var spriteRenderer = simulationEntityInstance.GetComponent<SpriteRenderer>();
                var randomizedColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f));
                spriteRenderer.color = randomizedColor;
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Rendering the extents of the simulation field.
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}