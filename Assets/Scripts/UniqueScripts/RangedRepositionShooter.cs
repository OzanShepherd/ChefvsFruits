using UnityEngine;

public class RangedRepositionShooter : MonoBehaviour
{
    [Header("Reposition Settings")]
    public float desiredRange = 3f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.3f;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float shootInterval = 3f;

    private Transform player;
    private float shootTimer;
    private bool isDashing = false;
    private Vector3 dashTarget;
    private float dashTimeRemaining;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        shootTimer = shootInterval;
    }

    void Update()
    {
        if (!player) return;

        shootTimer -= Time.deltaTime;

        if (isDashing)
        {
            DashMovement();
            return;
        }

        if (shootTimer <= 0f)
        {
            StartDashToIdealRange();
        }
    }

    void StartDashToIdealRange()
    {
        Vector3 toPlayer = player.position - transform.position;
        Vector3 moveDir = toPlayer.normalized;

        // Move to a point exactly desiredRange away from the player
        dashTarget = player.position - moveDir * desiredRange;

        // Ensure target stays level on Y
        dashTarget.y = transform.position.y;

        dashTimeRemaining = dashDuration;
        isDashing = true;
    }

    void DashMovement()
    {
        dashTimeRemaining -= Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

        if (dashTimeRemaining <= 0f || Vector3.Distance(transform.position, dashTarget) < 0.1f)
        {
            isDashing = false;

            float currentDistance = Vector3.Distance(transform.position, player.position);

            if(Mathf.Abs(currentDistance - desiredRange) <= 0.5f)
            {
                Shoot();
            }

            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        if (!projectilePrefab || !firePoint || !player) return;

        Vector3 direction = (player.position - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        // Optional: rotate enemy to face the player
        Vector3 faceDir = player.position - transform.position;
        faceDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(faceDir);
    }
}
