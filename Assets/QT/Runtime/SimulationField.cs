using UnityEngine;
using UnityEngine.Profiling;

namespace QT.Runtime
{
    /// <summary>
    /// Basic universe/scene for entities in it.
    /// </summary>
    [DisallowMultipleComponent]
    public class SimulationField : MonoBehaviour
    {
        public Quadtree Quadtree { get; private set; }

        [SerializeField] private Bounds bounds;
        [SerializeField] private GameObject simulationEntityPrefab;
        [SerializeField] private GameObject[] simulationEntityInstances;
        [SerializeField] private byte maximumNumberOfAllowedSimulationEntities;

        private void OnEnable()
        {
            // Allocate the positions array.
            simulationEntityInstances = new GameObject[maximumNumberOfAllowedSimulationEntities];

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

                // Register the position of entity for further usage.
                simulationEntityInstances[i] = simulationEntityInstance;
            }

            // Generate a new quadtree.
            Profiler.BeginSample(ProfilerConstants.ProfilerSampleLabel);
            Quadtree = new Quadtree(bounds, 1);

            // Register simulation positions to the quadtree for construction.
            var simulationEntityInstancePositions = new Vector3[simulationEntityInstances.Length];
            for (byte i = 0; i < simulationEntityInstances.Length; i++)
            {
                simulationEntityInstancePositions[i] = simulationEntityInstances[i].transform.position;
            }

            Quadtree.Construct(simulationEntityInstancePositions);
            Profiler.EndSample();
        }

        private void OnDisable()
        {
            // Destroy the existing sim. entities.
            for (byte i = 0; i < simulationEntityInstances.Length; i++)
            {
                Destroy(simulationEntityInstances[i]);
            }

            simulationEntityInstances = null;
        }

        private void OnDrawGizmosSelected()
        {
            // Rendering the extents of the simulation field.
            Quadtree?.OnDrawGizmosSelected();
        }
    }
}