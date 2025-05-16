using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public void TakenDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP < 1)
        {
            print("Player Dead");
        }
        else
        {
            print("Player Hit");
            //animator.SetTrigger("DAMAGE");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            print("o");
        if(other.CompareTag("ZombieAttackHand"))
        {
            TakenDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
