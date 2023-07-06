
using UnityEngine;

public class Move : ActionBase
{
    public override bool IsDone { get; protected set; }

    public float moveSpeed;
    public float moveDistance;
    public Vector3 targetPosition;

    public Move(Vector3 direction, float moveSpeed, float moveDistance) : base(direction)
    {
        this.moveSpeed = moveSpeed;
        this.moveDistance = moveDistance;
    }

    public override void Init(ActionManager actionManager)
    {
        var myTransform = actionManager.transform;
        targetPosition = myTransform.position + direction * moveDistance;
    }

    public override void Execute(ActionManager actionManager)
    {
        var myTransform = actionManager.transform;

        myTransform.position = Vector3.MoveTowards(myTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        IsDone = Vector3.Distance(myTransform.position, targetPosition) < 0.01f;
    }
}
