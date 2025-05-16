using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    private float timer;
    private Transform player;
    
    public float idleTime = 0f;
    public float detectionAreaRadius = 18f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(timer > idleTime)
        {
            animator.SetBool("isPatroling", true);
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if(distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
