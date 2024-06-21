using System.Collections.Generic;
using UnityEngine;

namespace QT.Runtime
{
    /// <summary>
    /// Quadrants are the one of four subdivisions of a bigger quadrant.
    /// </summary>
    public class Quadrant
    {
        // The bounds of the quadrant, its volume in space.
        private readonly Bounds _bounds;

        // How many points can be stored inside this quadrant at maximum?
        private readonly byte _positionRegistryCapacity;

        // Positions that are inside of this quadrant's boundaries.
        private readonly List<Vector3> _positionRegistry;

        // Reference to each child quadrant, they will be allocated during subdivision operation.
        private Quadrant _northWest;
        private Quadrant _northEast;
        private Quadrant _southWest;
        private Quadrant _southEast;

        // Basic flag to see if the quadrant is already subdivided.
        private bool _isSubdivided;

        public Quadrant(Bounds bounds, byte positionRegistryCapacity)
        {
            _bounds = bounds;
            _positionRegistryCapacity = positionRegistryCapacity;
            _positionRegistry = new List<Vector3>();
            _isSubdivided = false;
        }

        public void InsertPosition(Vector3 position)
        {
            // Check if given position is inside the bounds of the quadrant.
            if (!_bounds.Contains(position)) return;

            // Check if we have enough space in registry to save/add given position.
            if (_positionRegistry.Count < _positionRegistryCapacity)
            {
                _positionRegistry.Add(position);
            }
            else
            {
                // We don't have enough space in registry, it is time to subdivide the quadrant.
                // But we need to check if it is subdivided before.
                if (!_isSubdivided)
                {
                    Subdivide();
                }

                // Try to add given position to each quadrant.
                _northWest.InsertPosition(position);
                _northEast.InsertPosition(position);
                _southWest.InsertPosition(position);
                _southEast.InsertPosition(position);
            }
        }

        private void Subdivide()
        {
            // Calculate the bounds for each child.
            var originalCenter = _bounds.center;
            var originalSize = _bounds.size;
            var newSize = originalSize / 2;

            var northWestCenter = originalCenter + new Vector3(-newSize.x / 2, 0, newSize.z / 2);
            var northEastCenter = originalCenter + new Vector3(newSize.x / 2, 0, newSize.z / 2);
            var southWestCenter = originalCenter + new Vector3(-newSize.x / 2, 0, -newSize.z / 2);
            var southEastCenter = originalCenter + new Vector3(newSize.x / 2, 0, -newSize.z / 2);

            var northWestBounds = new Bounds(northWestCenter, newSize);
            var northEastBounds = new Bounds(northEastCenter, newSize);
            var southWestBounds = new Bounds(southWestCenter, newSize);
            var southEastBounds = new Bounds(southEastCenter, newSize);

            // Allocate every single children quadtree reference.
            _northWest = new Quadrant(northWestBounds, _positionRegistryCapacity);
            _northEast = new Quadrant(northEastBounds, _positionRegistryCapacity);
            _southEast = new Quadrant(southEastBounds, _positionRegistryCapacity);
            _southWest = new Quadrant(southWestBounds, _positionRegistryCapacity);

            _isSubdivided = true;
        }
    }
}