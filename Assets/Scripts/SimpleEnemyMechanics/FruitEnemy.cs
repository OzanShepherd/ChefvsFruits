using UnityEngine;

public class FruitEnemy : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 1f;
    private float currentHealth;

    [Header("Death Effects")]
    public ParticleSystem deathEffect;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
