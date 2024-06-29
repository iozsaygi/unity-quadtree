using System.Collections.Generic;
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

        public void Construct(Vector3[] positions)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < positions.Length; i++)
            {
                _root.InsertPosition(positions[i]);
            }
        }

        public void InsertPosition(Vector3 position)
        {
            _root.InsertPosition(position);
        }

        public IReadOnlyList<Vector3> GetPositionsNearby(Vector3 origin)
        {
            return _root.GetPositionsNearby(origin);
        }

        // Debugging and demo purposes only, not the actual part of quadtree API.
        public void OnDrawGizmosSelected()
        {
            _root.OnDrawGizmosSelected();
        }
    }
}