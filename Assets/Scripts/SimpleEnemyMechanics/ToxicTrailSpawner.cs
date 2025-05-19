using UnityEngine;

public class ToxicTrailSpawner : MonoBehaviour
{
    public GameObject creepPrefab;
    public float dropInterval = 1f;
    public Transform dropPoint;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Instantiate(creepPrefab, dropPoint.position, Quaternion.identity);
            timer = dropInterval;
        }
    }
}
