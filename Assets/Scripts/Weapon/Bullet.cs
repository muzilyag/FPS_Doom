using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if(objectWeHit.gameObject.CompareTag("Target"))
        {
            print($"hit a target!");

            CreateBulletEffect(objectWeHit);

            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            print($"hit a wall !");

            CreateBulletEffect(objectWeHit);

            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            print($"hit a enemy !");

            if(objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
            {
                objectWeHit.gameObject.GetComponent<Enemy>().TakenDamage(bulletDamage);
            }

            CreateBloodEffect(objectWeHit);

            Destroy(gameObject);
        }
    }

    private void CreateBloodEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSpray = Instantiate(
            GlobalReferences.Instance.bloodSprayEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        bloodSpray.transform.SetParent(objectWeHit.gameObject.transform);
    }

    private void CreateBulletEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
