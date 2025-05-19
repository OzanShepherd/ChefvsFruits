using UnityEngine;

public class ToxicCreep : MonoBehaviour
{
    public float damage = 1f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage * Time.deltaTime);
            }
        }
    }
}
