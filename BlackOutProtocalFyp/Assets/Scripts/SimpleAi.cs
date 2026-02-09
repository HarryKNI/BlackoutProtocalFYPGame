using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAi : MonoBehaviour
{
    
    public enum States { Idle, Chasing, Evading, Shooting, Finding, AiReturn };
    public States m_States;

    [Header("Searchpoints List")]
    public List<Transform> SearchPoints = new List<Transform>();

    [Header("Ai And Player")]
    public NavMeshAgent AI;
    public Transform Player;
    

    private bool TimerStart = false;
    private bool TimerDone = false;
    private NavMeshHit Hit;

    private int RndNum = 0;
    private float Timer = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            case States.Finding:
                Finding();
                break;
            default:
                AiReturn();
                break;

        }

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
            m_States = States.Finding;

        }
    }

    public void Chasing()
    {
        AI.SetDestination(Player.position);
    }

    public void Evading()
    {
        
    }

    public void Finding()
    {

        AI.SetDestination(SearchPoints[RndNum].position);

    }

    public void AiReturn()
    {
        AI.SetDestination(SearchPoints[1].position);
    }

    private void OnTriggerEnter(Collider Coll)
    {
        
        if (Coll.gameObject.tag == "Player" && AI.Raycast(Player.position, out Hit) == false)
        {
            TimerStart = false;
            TimerDone = true;
            m_States = States.Chasing;
        }

        if (Coll.gameObject.tag == "AiBoundary")
        {
            m_States = States.AiReturn;
        }

    }

    private void OnTriggerExit(Collider Coll)
    {
        if (Coll.gameObject.tag == "Player")
        {
            TimerStart = true;
            m_States = States.Idle;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Search_Point")
        {
            m_States = States.Idle;
            TimerStart = true;
            print("Triggered");
            RndNum = UnityEngine.Random.Range(0, SearchPoints.Count);
        }
    }

   





}
