using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Level1Responce : MonoBehaviour
{
    public BehaviorGraph Aigraph;
    public NavMeshAgent AI;
    NavMeshHit Hit;
    public GameObject Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Aigraph.BlackboardReference.SetVariableValue("ResponceLevel", 1);
        AI.SetDestination(Player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider Coll)
    {
        if (Coll.gameObject.tag == "Player")
        {
            Aigraph.BlackboardReference.SetVariableValue("ResponceLevel", 0);
        }
    }
}
