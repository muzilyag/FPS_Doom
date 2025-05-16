using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakenDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP < 1)
        {
            int randomNumber = Random.Range(0, 2); // 0 или 1

            animator.SetTrigger(randomNumber == 0 ? "DIE1" : "DIE2");

            //animator.SetTrigger("DIE1");
            //Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); //Attcking or Stop sttacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); // Detection (Start Chasing)

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); // Stop Chasing
    }
}
