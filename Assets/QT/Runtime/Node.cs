using System.Collections.Generic;
using UnityEngine;

namespace QT.Runtime
{
    /// <summary>
    /// Basic node struct to represent each element in quadtree.
    /// </summary>
    public readonly struct Node
    {
        public readonly Bounds Bounds;
        public readonly Node[] Children;
        public readonly IReadOnlyList<Vector3> PositionsWithInBounds;

        public Node(Bounds bounds, Node[] children, IReadOnlyList<Vector3> positionsWithInBounds)
        {
            Bounds = bounds;
            Children = children;
            PositionsWithInBounds = positionsWithInBounds;
        }
    }
}