using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHand;

    public int zombieDamage;

    private void Start()
    {
        zombieHand.damage = zombieDamage;        

    }
}
