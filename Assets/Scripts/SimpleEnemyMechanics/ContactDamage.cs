using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public float damage = 1f;
    public float damageCooldown = 1f; // time between hits in seconds

    private bool canDamage = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (!canDamage) return;

        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player takes " + damage + " damage from " + gameObject.name);
                canDamage = false;
                Invoke(nameof(ResetCooldown), damageCooldown);
            }
        }
    }

    private void ResetCooldown()
    {
        canDamage = true;
    }
}
