using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public Queue<ActionBase> actions;

    public Transform enemy;

    public float moveSpeed;
    public float moveDistance;
    public bool defence;

    public Vector3 defenceDirection;
    public LayerMask enemyLayer;

    private void Awake()
    {
        actions = new Queue<ActionBase>(4);

        actions.Enqueue(new Move(Vector3.forward, moveSpeed, moveDistance));
        actions.Enqueue(new Move(Vector3.left, moveSpeed, moveDistance));
        actions.Enqueue(new Move(transform.right, moveSpeed, moveDistance));
        actions.Enqueue(new Defence(defenceDirection));
        actions.Enqueue(new Attack(enemy, enemyLayer));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ExecuteActions());
        }
    }

    public void TakeDamage(Vector3 fromDirection)
    {
        if (!defence) return;

        if (Vector3.Dot(fromDirection, defenceDirection) < -0.8f)
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    public IEnumerator ExecuteActions()
    {
        while (true)
        {
            foreach (var action in actions)
            {
                action.Init(this);
                while (!action.IsDone)
                {
                    action.Execute(this);

                    yield return null;
                }
            }

            foreach (var item in actions)
            {
                var rng = Random.Range(0, 1f);
                if (rng > 0.5f)
                {
                    item.Reset();
                }
            }

            yield return new WaitForSeconds(2f);
        }

        //while (actions.Count > 0)
        //{
        //    var action = actions.Dequeue();
        //    action.Init(this);
        //    while (!action.IsDone)
        //    {
        //        action.Execute(this);

        //        yield return null;
        //    }
        //}
    }
}