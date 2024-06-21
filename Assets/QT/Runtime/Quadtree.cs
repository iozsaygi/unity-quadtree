using System.Collections.Generic;
using UnityEngine;

namespace QT.Runtime
{
    public class Quadtree
    {
        private readonly Bounds _bounds;
        private readonly byte _boundsPositionRegistryCapacity;
        private readonly List<Vector3> _positionRegistry;

        private Quadtree _northWest;
        private Quadtree _northEast;
        private Quadtree _southWest;
        private Quadtree _southEast;
        private bool _isSubdivided;

        public Quadtree(Bounds bounds, byte boundsPositionRegistryCapacity)
        {
            _bounds = bounds;
            _boundsPositionRegistryCapacity = boundsPositionRegistryCapacity;
            _positionRegistry = new List<Vector3>();
            _isSubdivided = false;
        }

        public void InsertPosition(Vector3 position)
        {
            if (!IsPositionWithInBounds(position)) return;

            if (_positionRegistry.Count < _boundsPositionRegistryCapacity)
            {
                _positionRegistry.Add(position);
            }
            else
            {
                if (!_isSubdivided)
                {
                    Subdivide();
                }

                _northWest.InsertPosition(position);
                _northEast.InsertPosition(position);
                _southWest.InsertPosition(position);
                _southEast.InsertPosition(position);
            }
        }

        public void Render()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);

            // Recursively render each child.
            if (!_isSubdivided) return;
            _northWest.Render();
            _northEast.Render();
            _southWest.Render();
            _southEast.Render();
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
            _northWest = new Quadtree(northWestBounds, _boundsPositionRegistryCapacity);
            _northEast = new Quadtree(northEastBounds, _boundsPositionRegistryCapacity);
            _southEast = new Quadtree(southEastBounds, _boundsPositionRegistryCapacity);
            _southWest = new Quadtree(southWestBounds, _boundsPositionRegistryCapacity);

            _isSubdivided = true;
        }

        private bool IsPositionWithInBounds(Vector3 position)
        {
            return _bounds.Contains(position);
        }
    }
}