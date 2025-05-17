using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [Header("Settings")]
    public List<AmmoBox> ammoBoxPrefabs;
    public float spawnInterval = 30f;
    public int maxSpawnedItems = 5;
    public Vector2 spawnArea = new Vector2(20f, 20f);

    private void Start()
    {
        StartCoroutine(SpawnAmmoRoutine());
    }

    private IEnumerator SpawnAmmoRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (transform.childCount < maxSpawnedItems && ammoBoxPrefabs.Count > 0)
            {
                SpawnAmmo();
            }
        }
    }

    private void SpawnAmmo()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            0.0f,
            Random.Range(-spawnArea.y / 2, spawnArea.y / 2)
        ) + transform.position;

        int randomIndex = Random.Range(0, ammoBoxPrefabs.Count);
        AmmoBox randomAmmoBox = ammoBoxPrefabs[randomIndex];

        Instantiate(randomAmmoBox.gameObject, spawnPosition, Quaternion.identity, transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0.1f, spawnArea.y));
    }
}