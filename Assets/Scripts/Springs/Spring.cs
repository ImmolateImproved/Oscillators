using UnityEngine;

[System.Serializable]
public struct Spring
{
    public float stiffness;
    public float damper;

    public float length;

    public Vector3 HookesLaw(Vector3 displacement, Vector3 velocity)
    {
        var force = (stiffness * displacement) + (damper * velocity);

        return -force;
    }
    
    public readonly Vector2 CalculateForce(Vector2 displacement, Vector2 velocity)
    {
        var force = stiffness * displacement - damper * velocity;

        return force;
    }
}
