using UnityEngine;

public class ToxicAura : MonoBehaviour
{
    public float auraRadius = 2f;
    public float auraDamage = 0.5f;
    public float tickInterval = 1f;

    private float timer;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (!player) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= auraRadius)
            {
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    ph.TakeDamage(auraDamage);
                }
            }

            timer = tickInterval;
        }
    }
}
