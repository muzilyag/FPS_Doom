using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class ZombiePatrolingState : StateMachineBehaviour
{
    private List<Transform> waypointsList = new List<Transform>();

    private Transform player;
    private NavMeshAgent agent;
    private float timer;

    public float patrolingTime = 10f;
    public float patrolSpeed = 18f;
    public float detectionArea = 2f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0;

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach(Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }
        int ind = Random.Range(0, waypointsList.Count);
        //Debug.Log($"{ind}");
        Vector3 nexPosition = waypointsList[ind].position;
        agent.SetDestination(nexPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(SoundManager.Instance.ZombieChannel.isPlaying == false)
        {
            SoundManager.Instance.ZombieChannel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.ZombieChannel.PlayDelayed(1f);
        }


        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            int ind = Random.Range(0, waypointsList.Count);
            //Debug.Log($"{ind}");
            agent.SetDestination(waypointsList[ind].position);
        }

        timer += Time.deltaTime;
        if (timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        SoundManager.Instance.ZombieChannel.Stop();
    }
}
