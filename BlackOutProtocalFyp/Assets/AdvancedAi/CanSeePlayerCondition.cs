using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Can See Player", story: "Agent Can See Player", category: "Conditions", id: "4541d0373b7c34af1d8efd968dbc0af7")]
public partial class CanSeePlayerCondition : Condition
{

    NavMeshAgent AI;
    NavMeshHit Hit;
    GameObject Enemy;
    Boolean CanSeePlayer = false;

    

    public override bool IsTrue()
    {
        

        if (CanSeePlayer == true)
        {
            return true;
        }

        else
        {
            return false;
        }
        
    }

    public override void OnStart()
    {
        
    }

    public override void OnEnd()
    {
    }

    private void OnTriggerEnter(Collider Coll)
    {
        Enemy = GameObject.FindWithTag("Enemy");
        AI = Enemy.GetComponent<NavMeshAgent>();
        GameObject PlayerObject = GameObject.FindWithTag("Player");
        if (Coll.gameObject.tag == "Player" && AI.Raycast(PlayerObject.transform.position, out Hit) == false)
        {
            CanSeePlayer = true;
        }
    }
}
