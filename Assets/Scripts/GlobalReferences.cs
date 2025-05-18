using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance {  get; set; }

    public GameObject bulletImpactEffectPrefab;

    public GameObject bloodSprayEffectPrefab;

    public PlayerData playerData;
    public int waveNumber;
    //public int killedZombie;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
