using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class SimpleAi : MonoBehaviour
{

    public enum States { Idle, Chasing, Evading, Shooting, Patrolling, AiReturn };
    public States m_States;

    [Header("Searchpoints List")]
    public List<Transform> SearchPoints = new List<Transform>();

    [Header("Ai And Player")]
    public NavMeshAgent AI;
    public Transform Player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Attacking Vectors")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("States")]
    public float sightRange;
    public float attackRange;
    public bool playerInsight;
    public bool playerInAttackRange;

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

        playerInsight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //if (!playerInsight && !playerInAttackRange ) m_States = States.Patrolling;
        if (playerInsight && !playerInAttackRange && AI.Raycast(Player.position, out Hit) == false) m_States = States.Chasing;
        if (playerInsight && playerInAttackRange && AI.Raycast(Player.position, out Hit) == false) m_States = States.Shooting;

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
        if (Timer >= 0)
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

    private void OnTriggerExit(Collider Coll)
    {
        //if (Coll.gameObject.tag == "Player")
        //{
        //    TimerStart = true;
        //    m_States = States.Idle;
        //}
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Search_Point")
        {
            m_States = States.Idle;
            Timer = 10;
            TimerStart = true;
            print("Triggered");
            RndNum = UnityEngine.Random.Range(0, SearchPoints.Count);
        }
    }

   





}
