# Unity Quadtree

Implementation of quadtree data structure in a 2D top-down environment with Unity engine. Please
see [this](https://en.wikipedia.org/wiki/Quadtree) page to get detailed information about quadtree.

## Preview

![First preview](https://github.com/iozsaygi/unity-quadtree/blob/main/Media/Preview-0.png)
![Second preview](https://github.com/iozsaygi/unity-quadtree/blob/main/Media/Preview-1.png)
![Third preview](https://github.com/iozsaygi/unity-quadtree/blob/main/Media/Preview-2.png)

## Included API implementations

- `public void InsertPosition(Vector3 position);`
    - Adds the given position to the quadtree, considers the position capacity allowed for each quadrant, and subdivides
      the root quadrant if it is required.

## License

[MIT License](https://github.com/iozsaygi/unity-quadtree/blob/main/LICENSE)