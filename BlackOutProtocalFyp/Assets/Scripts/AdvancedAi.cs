using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class AdvancedAi : MonoBehaviour
{

    public enum States { Idle, Chasing, Evading, Shooting, Patrolling, AiReturn };
    public States m_States;

    [Header("Searchpoints List")]
    public List<Transform> SearchPoints = new List<Transform>();

    [Header("Ai And Player")]
    public NavMeshAgent AI;
    public Transform Player;
    public LayerMask IsObsticale, IsPlayer;
    

    [Header("Attacking Vectors")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("States")]
    public float sightRange;
    public float attackRange;
    public bool playerInsight;
    public bool playerInAttackRange;
    public bool TopRaycast;
    public bool MiddleRaycast;
    public bool BottomRaycast;
    public bool ObsticaleRaycast;

    private bool TimerStart = false;
    private bool TimerDone = false;
    private NavMeshHit Hit;

    private int RndNum = 0;
    private float Timer = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        AI = GetComponent<NavMeshAgent>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_States)
        {
            case States.Idle:
                Idle();
                break;
            case States.Chasing:
                Chasing();
                break;
            case States.Evading:
                break;
            case States.Shooting:
                Shooting();
                break;
            case States.Patrolling:
                Patrolling();
                break;
            default:
                AiReturn();
                break;

        }
        Vector3 Toptransform = transform.position + new Vector3(0, 3, 0);
        Vector3 Middletranaform = transform.position + new Vector3(0,2,0);
        Vector3 Bottomtransform = transform.position - new Vector3(0, 0, 0);


        playerInsight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        TopRaycast = Physics.Raycast(Toptransform, transform.forward, 15, IsPlayer );
        MiddleRaycast = Physics.Raycast(Middletranaform, transform.forward, 15, IsPlayer);
        BottomRaycast = Physics.Raycast(Bottomtransform, transform.forward, 15, IsPlayer);
        ObsticaleRaycast = Physics.Raycast(Bottomtransform, transform.forward, 10, IsObsticale);

        Debug.DrawRay(Toptransform, transform.forward, Color.red);

        if (!playerInsight && !playerInAttackRange) m_States = States.Patrolling;
        if (playerInsight /*&& !playerInAttackRange*/) 
        {
            if (TopRaycast && MiddleRaycast && BottomRaycast && !ObsticaleRaycast) 
            {
                m_States = States.Chasing;
            }

            if (TopRaycast && MiddleRaycast && !BottomRaycast && !ObsticaleRaycast)
            {
                m_States = States.Chasing;
            }

            if (!TopRaycast && !MiddleRaycast && !BottomRaycast && ObsticaleRaycast)
            {
                m_States = States.Patrolling;
            }
        } 
        //if (playerInsight && playerInAttackRange && AI.Raycast(Player.position, out Hit) == false) m_States = States.Shooting;

        if (TimerStart == true)
        {
            print(Timer);
            Timer -= Time.deltaTime;

        }

        if (TimerDone == true)
        {
            Timer = 10.0f;
            TimerDone = false;
        }



    }

    public void Idle()
    {
        if (Timer == 0)
        {
            TimerDone = true;
            TimerStart = false;
            m_States = States.Patrolling;

        }
    }

    public void Chasing()
    {
        AI.SetDestination(Player.position);
        transform.LookAt(Player);

        
    }

    public void Shooting()
    {
        AI.SetDestination(Player.position);
        transform.LookAt(Player);

        if (!alreadyAttacked)
        {
            //Attack Code//

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void Evading()
    {

    }

    public void Patrolling()
    {

        AI.SetDestination(SearchPoints[RndNum].position);

    }

    public void AiReturn()
    {
        AI.SetDestination(SearchPoints[1].position);
    }

    private void OnTriggerEnter(Collider Coll)
    {

        if (Coll.gameObject.tag != "AiBoundary")
        {
            m_States = States.AiReturn;
        }

        if (Coll.gameObject.tag == "Search_Point")
        {
            m_States = States.Idle;
            TimerStart = true;
            print("Triggered");
            RndNum = UnityEngine.Random.Range(0, SearchPoints.Count);
        }

    }

    







}
