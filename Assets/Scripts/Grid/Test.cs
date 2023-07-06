using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Bounds bounds;

    public Transform[] walls;

    private void OnValidate()
    {
        if (walls.Length != 4) return;

        //walls[0].position = bounds.extents;
        walls[0].localScale = new Vector3(bounds.size.x, 5, 1);
        walls[1].localScale = new Vector3(bounds.size.x, 5, 1);
        walls[2].localScale = new Vector3(1, 5, bounds.size.z);
        walls[3].localScale = new Vector3(1, 5, bounds.size.z);

        walls[0].position = new Vector3(0, 2.5f, bounds.extents.z);
        walls[1].position = new Vector3(0, 2.5f, -bounds.extents.z);
        walls[2].position = new Vector3(bounds.extents.x, 2.5f, 0);
        walls[3].position = new Vector3(-bounds.extents.x, 2.5f, 0);
    }
}
