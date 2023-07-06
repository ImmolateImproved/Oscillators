using UnityEngine;

public abstract class ActionBase
{
    public Vector3 direction;

    public abstract bool IsDone { get; protected set; }

    protected ActionBase(Vector3 direction)
    {
        this.direction = direction;
    }

    public virtual void Init(ActionManager actionManager)
    {

    }

    public virtual void Reset()
    {
        IsDone = false;

        var x = Random.Range(-1, 1f);
        var z = Random.Range(-1, 1f);

        direction = new Vector3(x, 0, z);
    }

    public abstract void Execute(ActionManager actionManager);
}