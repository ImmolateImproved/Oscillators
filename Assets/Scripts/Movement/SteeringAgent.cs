using TMPro;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    private Rigidbody rb;

    public float maxSpeed;
    public float maxForce;

    public float fleeForceCoef;
    public float minFleeRange;
    public float maxFleeRange;
    public AnimationCurve fleeForceCurve;

    public Transform obstacleHolder;
    private Transform[] obstacles;

    private Vector3 targetDirection;

    public Transform target;
    public float dot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        obstacles = new Transform[obstacleHolder.childCount];
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i] = obstacleHolder.GetChild(i);
        }
    }

    private void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out var enter))
        {
            var targetPos = ray.GetPoint(enter);
            targetPos.y = transform.position.y;

            targetDirection = targetPos - transform.position;

            Seek(targetPos);
        }

        for (int i = 0; i < obstacles.Length; i++)
        {
            var targetPos = obstacles[i].position;

            var dirToTarget = targetPos - transform.position;

            var distance = Vector3.Distance(transform.position, targetPos);
            var dot = Vector3.Dot(targetDirection.normalized, dirToTarget.normalized);
            this.dot = dot;
            target = null;
            if (distance < maxFleeRange && dot > 0.7f)
            {
                target = obstacles[i];

                Flee(targetPos, distance);
            }
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        transform.forward = targetDirection;
    }

    private void Seek(Vector3 targetPosition)
    {
        var desired = targetPosition - transform.position;
        desired = SetMagnitude(desired, maxSpeed);

        var steering = desired - rb.velocity;

        ApplyForce(steering);
    }

    private void Flee(Vector3 targetPosition, float distance)
    {
        var iLerp = Mathf.InverseLerp(maxFleeRange, minFleeRange, distance);
        var coef = fleeForceCurve.Evaluate(iLerp) * fleeForceCoef;

        var dirToTarget = targetPosition - transform.position;
        var cross = Vector3.Cross(dirToTarget, Vector3.up);
        var desired = cross;

        desired = SetMagnitude(desired, maxSpeed);

        var steering = desired - rb.velocity;

        steering *= fleeForceCoef;// coef;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        ApplyForce(steering);
    }

    private void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    private Vector3 SetMagnitude(Vector3 desired, float maxSpeed)
    {
        return desired.normalized * maxSpeed;
    }
}