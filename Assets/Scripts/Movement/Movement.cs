using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;

    public float jumpForce;

    public Spring movementSpring;
    public Spring rotationSpring;

    [Tooltip("The local rotation about which oscillations are centered.")]
    public Vector3 localEquilibriumRotation = Vector3.zero;

    [Tooltip("The axes over which the oscillator applies torque. Within range [0, 1].")]
    public Vector3 torqueScale = Vector3.one;

    private Vector3 velocity;
    private bool jumpInput;

    private float angularDisplacementMagnitude;
    private Vector3 _rotAxis;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        velocity = new Vector3(h, 0, v).normalized * moveSpeed;

        jumpInput = Input.GetKeyDown(KeyCode.Space) || jumpInput;
    }

    private void FixedUpdate()
    {
        Move();
        Jump();

        AddSpringForce();
        AddTorqueForce();
    }

    private void Move()
    {
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    private void Jump()
    {
        var jump = jumpInput ? jumpForce : 0;
        rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        jumpInput = false;
    }

    private void AddSpringForce()
    {
        var displacement = Vector3.zero;
        displacement.y = rb.position.y - movementSpring.length;

        var force = movementSpring.HookesLaw(displacement, rb.velocity);
        rb.AddForceAtPosition(force, rb.position - transform.up);
    }

    private void AddTorqueForce()
    {
        var restoringTorque = CalculateRestoringTorque();
        
        rb.AddTorque(Vector3.Scale(restoringTorque, torqueScale));
    }

    private Vector3 CalculateRestoringTorque()
    {
        Quaternion deltaRotation = MathUtils.ShortestRotation(transform.localRotation, Quaternion.Euler(localEquilibriumRotation));
        deltaRotation.ToAngleAxis(out angularDisplacementMagnitude, out _rotAxis);

        Vector3 angularDisplacement = angularDisplacementMagnitude * Mathf.Deg2Rad * _rotAxis.normalized;

        var torque = rotationSpring.HookesLaw(angularDisplacement, rb.angularVelocity);

        return (torque);
    }
}