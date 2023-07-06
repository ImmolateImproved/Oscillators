
using UnityEngine;

public class Defence : ActionBase
{
    public override bool IsDone { get; protected set; }

    public Defence(Vector3 direction) : base(direction)
    {
    }

    public override void Execute(ActionManager actionManager)
    {
        actionManager.defenceDirection = direction;
        actionManager.defence = true;
        IsDone = true;
    }
}