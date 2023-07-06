using UnityEngine;

public class Attack : ActionBase
{
    public override bool IsDone { get; protected set; }

    private LayerMask enemyLayer;

    private Transform enemy;

    public Attack(Transform enemy, LayerMask enemyLayer) : base(Vector3.zero)
    {
        this.enemyLayer = enemyLayer;
        this.enemy = enemy;
    }

    public override void Execute(ActionManager actionManager)
    {
        var directionToTarget = enemy.position - actionManager.transform.position;

        var ray = new Ray(actionManager.transform.position, directionToTarget);

        if (Physics.Raycast(ray, out var hit, 100f, enemyLayer))
        {
            if (hit.collider.TryGetComponent<ActionManager>(out var enemyManager))
            {
                enemyManager.TakeDamage(directionToTarget);
            }
        }

        IsDone = true;
    }
}
