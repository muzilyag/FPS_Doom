using System;
using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{
    public float timeForDestructin;
    private void Start()
    {
        StartCoroutine(DestroySelf(timeForDestructin));
    }

    private IEnumerator DestroySelf(float timeForDestructin)
    {
        yield return new WaitForSeconds(timeForDestructin);

        Destroy(gameObject);
    }
}
