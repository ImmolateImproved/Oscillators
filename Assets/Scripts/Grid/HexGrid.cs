using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int gridRadius;
    public float tileSpacing;
    public float tileSize;
    public int nodesCount;

    private float tileSlotRadiusPrev;
    private float tileSizePrev;

    public Tile tilePrefab;

    private Grid grid;

    public float SlotSize => tileSize * tileSpacing;

    public int2 node;

    private void Awake()
    {
        tileSlotRadiusPrev = tileSpacing;
        tileSizePrev = tileSize;

        nodesCount = HexTileNeighbors.CalculateTilesCount(gridRadius);

        var nodes = BuildSquareGrid(gridRadius, Allocator.Persistent);//BuildGridBFS(nodesCount, Allocator.Persistent);

        grid = new Grid(nodes);

        foreach (var node in nodes)
        {
            var tile = Instantiate(tilePrefab, transform);
            var position = GridUtils.NodeToPosition(node, SlotSize);

            tile.Init(position, tileSize, node);

            tile.GetComponent<SpringBehaviour>().Init(position);

            grid.AddTile(node, tile);
        }
    }

    private void OnDestroy()
    {
        grid.Dispose();
    }

    private void Update()
    {
        var slotRadiusChange = !Mathf.Approximately(tileSpacing, tileSlotRadiusPrev);
        var tileSizeChange = !Mathf.Approximately(tileSize, tileSizePrev);

        if (!(slotRadiusChange || tileSizeChange)) return;

        tileSlotRadiusPrev = tileSpacing;
        tileSizePrev = tileSize;

        foreach (var tile in grid.GetTiles())
        {
            var position = GridUtils.NodeToPosition(tile.Node, SlotSize);

            tile.SetPositionAndScale(position, tileSize);
        }
    }

    private static NativeHashSet<int2> BuildSquareGrid(int nodesCount, Allocator allocator)
    {
        var nodes = new NativeHashSet<int2>(nodesCount, allocator);

        for (int i = 0; i < nodesCount; i++)
        {
            for (int j = 0; j < nodesCount; j++)
            {
                var node = new int2(i, j);
                node = GridUtils.OddrToAxial(node);
                nodes.Add(node);
            }
        }

        return nodes;
    }

    private static NativeHashSet<int2> BuildGridBFS(int nodesCount, Allocator allocator)
    {
        var neighbors = HexTileNeighbors.Neighbors;

        var queue = new NativeQueue<int2>(Allocator.Temp);
        var visited = new NativeHashSet<int2>(nodesCount, allocator);

        queue.Enqueue(0);
        visited.Add(0);

        while (visited.Count() < nodesCount)
        {
            var node = queue.Dequeue();

            for (int i = 0; i < neighbors.Length; i++)
            {
                var neighborNode = HexTileNeighbors.GetNeighborNode(node, neighbors[i]);

                if (visited.Add(neighborNode))
                {
                    queue.Enqueue(neighborNode);
                }
            }
        }

        queue.Dispose();

        return visited;
    }
}

public class Grid
{
    private NativeHashSet<int2> nodes;

    private Dictionary<int2, Tile> tiles;

    public Grid(NativeHashSet<int2> nodes)
    {
        this.nodes = nodes;
        tiles = new Dictionary<int2, Tile>(nodes.Count());
    }

    public bool AddTile(int2 node, Tile tile)
    {
        return tiles.TryAdd(node, tile);
    }

    public bool GetTile(int2 node, out Tile tile)
    {
        return tiles.TryGetValue(node, out tile);
    }

    public IEnumerable<Tile> GetTiles()
    {
        foreach (var tile in tiles)
        {
            yield return tile.Value;
        }
    }

    public int2 GetNeighborNodeFromDirection(int2 currentNode, HexDirections direction)
    {
        var dir = HexTileNeighbors.Neighbors[(int)direction];

        var nextNode = currentNode + dir;

        return nextNode;
    }

    public NativeList<int2> GetNeighborNodes(int2 startNode)
    {
        var neighbors = new NativeList<int2>(6, Allocator.Temp);

        var direction = HexDirections.BottomLeft;

        for (int i = 0; i < HexDirectionsExtentions.DIRECTIONS_COUNT; i++)
        {
            var nextNode = GetNeighborNodeFromDirection(startNode, direction);
            direction = direction.GetNextDirection();

            if (HasNode(nextNode))
            {
                neighbors.Add(nextNode);
            }
        }
        return neighbors;
    }

    public bool AddNode(int2 node)
    {
        return nodes.Add(node);
    }

    public bool RemoveNode(int2 node)
    {
        return nodes.Remove(node);
    }

    public bool HasNode(int2 nodeIndex)
    {
        return nodes.Contains(nodeIndex);
    }

    public void Dispose()
    {
        nodes.Dispose();
    }
}