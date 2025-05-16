using UnityEngine;

public class AbstractManager : MonoBehaviour
{
    public static AbstractManager Instance { get; set; }
    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
