using UnityEngine;

namespace QT.Runtime
{
    public class Quadtree
    {
        // The root quadrant of the tree. Contains every other smaller quadrants.
        private readonly Quadrant _root;

        public Quadtree(Bounds bounds, byte positionRegistryCapacityPerQuadrant)
        {
            _root = new Quadrant(bounds, positionRegistryCapacityPerQuadrant);
        }

        public void InsertPosition(Vector3 position)
        {
            _root.InsertPosition(position);
        }

        public void OnDrawGizmosSelected()
        {
            _root.OnDrawGizmosSelected();
        }
    }
}