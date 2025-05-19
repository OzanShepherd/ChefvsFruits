using UnityEngine;

public class AppleBoss : MonoBehaviour
{
    public float maxHealth = 10f;
    public float moveSpeed = 1.5f;

    private float currentHealth;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        //basic movement towards player
        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;

        //face the player
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 5f * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Apple boss took damage: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Apple Boss defeated!");
        Destroy(gameObject);
    }
}
