using UnityEngine;

public class SpringBehaviour : MonoBehaviour
{
    public Spring spring;

    private Vector3 startPos;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
    }

    public void Init(Vector3 startPosition)
    {
        startPos = startPosition;
    }

    private void FixedUpdate()
    {
        AddSpringForce();
    }

    public void AddSpringForce()
    {
        startPos.y = spring.length;
        var displacement = rb.position - startPos;

        var force = spring.HookesLaw(displacement, rb.velocity);
        rb.AddForce(force);
    }
}