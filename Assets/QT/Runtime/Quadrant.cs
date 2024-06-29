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
                if (!_isSubdivided) Subdivide();

                // Try to add given position to each quadrant.
                _northWest.InsertPosition(position);
                _northEast.InsertPosition(position);
                _southWest.InsertPosition(position);
                _southEast.InsertPosition(position);
            }
        }

        public IReadOnlyList<Vector3> GetPositionsNearby(Vector3 origin)
        {
            // Check if given 'origin' is inside the bounds of current quadrant.
            if (!_bounds.Contains(origin)) return new List<Vector3>().AsReadOnly();

            // Check if current quadrant subdivided before.
            if (!_isSubdivided) return _positionRegistry.AsReadOnly();

            // The quadrant is subdivided, query each child quadrant it has.
            var nearbyPositions = new List<Vector3>();

            nearbyPositions.AddRange(_northWest.GetPositionsNearby(origin));
            nearbyPositions.AddRange(_northEast.GetPositionsNearby(origin));
            nearbyPositions.AddRange(_southWest.GetPositionsNearby(origin));
            nearbyPositions.AddRange(_southEast.GetPositionsNearby(origin));

            return nearbyPositions.AsReadOnly();
        }

        // Debugging and demo purposes only, not the actual part of quadtree API.
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);

            // Also render the subdivision quadrants.
            if (!_isSubdivided) return;

            _northWest.OnDrawGizmosSelected();
            _northEast.OnDrawGizmosSelected();
            _southWest.OnDrawGizmosSelected();
            _southEast.OnDrawGizmosSelected();
        }

        private void Subdivide()
        {
            // Calculate the size for each child quadrant.
            var originalCenter = _bounds.center;
            var originalSize = _bounds.size;
            var newSize = new Vector3(originalSize.x / 2, originalSize.y / 2, originalSize.z);

            // Calculate half sizes for easier calculations.
            var halfHorizontalSize = newSize.x / 2;
            var halfVerticalSize = newSize.y / 2;

            // Calculate centers for the four child quadrants.
            var northWestCenter = new Vector3(originalCenter.x - halfHorizontalSize,
                originalCenter.y + halfVerticalSize, originalCenter.z);
            var northEastCenter = new Vector3(originalCenter.x + halfHorizontalSize,
                originalCenter.y + halfVerticalSize, originalCenter.z);
            var southWestCenter = new Vector3(originalCenter.x - halfHorizontalSize,
                originalCenter.y - halfVerticalSize, originalCenter.z);
            var southEastCenter = new Vector3(originalCenter.x + halfHorizontalSize,
                originalCenter.y - halfVerticalSize, originalCenter.z);

            // Create bounds for each child quadrant.
            var northWestBounds = new Bounds(northWestCenter, newSize);
            var northEastBounds = new Bounds(northEastCenter, newSize);
            var southWestBounds = new Bounds(southWestCenter, newSize);
            var southEastBounds = new Bounds(southEastCenter, newSize);

            // Allocate every single child quadrant reference.
            _northWest = new Quadrant(northWestBounds, _positionRegistryCapacity);
            _northEast = new Quadrant(northEastBounds, _positionRegistryCapacity);
            _southWest = new Quadrant(southWestBounds, _positionRegistryCapacity);
            _southEast = new Quadrant(southEastBounds, _positionRegistryCapacity);

            _isSubdivided = true;
        }
    }
}