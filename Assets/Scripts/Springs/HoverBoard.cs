using UnityEngine;

public class HoverBoard : MonoBehaviour
{
    public Spring spring;

    public LayerMask raysLayer;

    public Transform[] rayPoints;

    private float[] lastHitDistance;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastHitDistance = new float[rayPoints.Length];
    }

    private void FixedUpdate()
    {
        Hover();
    }

    private void Hover()
    {
        var i = 0;
        foreach (var rayPoint in rayPoints)
        {
            if (Physics.Raycast(rayPoint.position, -rayPoint.up, out var hit, float.PositiveInfinity, raysLayer))
            {
                var displacement = spring.length - hit.distance;
                var velocity = lastHitDistance[i] - hit.distance;

                var force = spring.HookesLaw(new Vector3(0, displacement, 0), new Vector3(0, velocity, 0));

                lastHitDistance[i] = hit.distance;

                rb.AddForceAtPosition(-force, rayPoint.position);
            }

            i++;
        }
    }
}