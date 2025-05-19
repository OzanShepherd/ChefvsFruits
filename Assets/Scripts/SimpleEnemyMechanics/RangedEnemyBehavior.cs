using UnityEngine;

public class RangedEnemyBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float minShootInterval = 1f;
    public float maxShootInterval = 3f;
    public float projectileSpeed = 10f;

    private float shootTimer;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        ResetShootTimer();
    }

    void Update()
    {
        if(player)
        {
            RotateTowardPlayer();
        }
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            ShootAtPlayer();
            ResetShootTimer();
        }
    }

    void ResetShootTimer()
    {
        shootTimer = Random.Range(minShootInterval, maxShootInterval);
    }

    void ShootAtPlayer()
    {
        if (!player || !projectilePrefab || !firePoint) return;

        Vector3 direction = (player.position - firePoint.position).normalized;

        // Rotate enemy to face the player
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f; // keep upright
        transform.rotation = Quaternion.LookRotation(lookDirection);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }

    void RotateTowardPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f; // Stay upright
        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

}
