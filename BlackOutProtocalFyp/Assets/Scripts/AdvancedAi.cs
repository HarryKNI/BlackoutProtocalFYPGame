using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class AdvancedAi : MonoBehaviour, HearSound
{

    public enum States { Idle, Chasing, Evading, Shooting, Patrolling, AiReturn, SoundHeard };
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
    public DoorInteraction DoorInteract;

    [Header("Visibilty")]
    float visibility;
    
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
            case States.SoundHeard:
                SoundHeard();
                break;
            default:
                AiReturn();
                break;

        }
        Vector3 Toptransform = transform.position + new Vector3(0, 3, 0);
        Vector3 Middletranaform = transform.position + new Vector3(0,2,0);
        Vector3 Bottomtransform = transform.position + new Vector3(0, 0, 0);


        playerInsight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        TopRaycast = Physics.Raycast(Toptransform, transform.forward, 15, IsPlayer );
        MiddleRaycast = Physics.Raycast(Middletranaform, transform.forward, 15, IsPlayer);
        BottomRaycast = Physics.Raycast(Bottomtransform, transform.forward, 15, IsPlayer);
        ObsticaleRaycast = Physics.Raycast(Bottomtransform, transform.forward, 10, IsObsticale);

        //Debug.DrawRay(Toptransform, transform.forward, Color.red);

        //if (!playerInsight && !playerInAttackRange) m_States = States.Patrolling;
        if (playerInsight /*&& !playerInAttackRange*/) 
        {
            if (TopRaycast)
            {
                visibility = 0.5f;
            }

            if (MiddleRaycast)
            {
                visibility = 0.3f;
            }

            if (BottomRaycast)
            {
                visibility = 0.2f;
            }

            if (TopRaycast && MiddleRaycast) 
            {
                visibility = 1f;
            }

            if (MiddleRaycast && BottomRaycast)
            {
                visibility = 0.5f;
            }

            if (TopRaycast && BottomRaycast)
            {
                visibility = 0.6f;
            }

            /*if (TopRaycast && MiddleRaycast && BottomRaycast && !ObsticaleRaycast) 
            {
                m_States = States.Chasing;
            }

            if (TopRaycast && MiddleRaycast && !BottomRaycast && !ObsticaleRaycast)
            {
                m_States = States.Chasing;
            }

            if (!TopRaycast && !MiddleRaycast && !BottomRaycast)
            {
                m_States = States.Patrolling;
            }*/
        } 
        //if (playerInsight && playerInAttackRange && AI.Raycast(Player.position, out Hit) == false) m_States = States.Shooting;

        if (TimerStart == true)
        {
            Timer -= Time.deltaTime;

        }

        if (TimerDone == true)
        {
            Timer = 10.0f;
            TimerDone = false;
        }

        if (visibility == 0f)
        {
            m_States = States.Patrolling;
        }

        if (visibility > 0f && visibility < 3f)
        {
            // Suspicious state
        }

        if (visibility > 3f && visibility < 7f)
        {
            // Investigation State
        }

        if (visibility >7f &&  visibility < 11f)
        {
            // Chase State
        }

        
    }
    public void RespondToSound(Sound sound)
    {
        print(name + " Responding to sound");
        m_States = States.SoundHeard;
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

    public void SoundHeard()
    {
        AI.SetDestination(DoorInteract.SoundPos);
    }

    private void OnTriggerEnter(Collider Coll)
    {

        /*if (Coll.gameObject.tag != "AiBoundary")
        {
            m_States = States.AiReturn;
        }*/

        if (Coll.gameObject.tag == "Search_Point")
        {
            m_States = States.Idle;
            TimerStart = true;
            RndNum = UnityEngine.Random.Range(0, SearchPoints.Count);
        }

    }

    







}
