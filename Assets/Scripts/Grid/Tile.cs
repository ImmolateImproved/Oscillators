using Unity.Mathematics;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int2 Node { get; private set; }

    private Transform myTransform;

    private Material material;

    private void Awake()
    {
        myTransform = transform;
        material = GetComponent<MeshRenderer>().material;
    }

    public void Select(bool select)
    {
        material.color = select ? Color.red : Color.white;
    }

    public void Init(Vector3 position, float tileSize, int2 node)
    {
        Node = node;
        SetPositionAndScale(position, tileSize);
    }

    public void SetPositionAndScale(Vector3 position, float tileSize)
    {
        myTransform.position = position;
        myTransform.localScale = Vector3.one * tileSize;
    }
}