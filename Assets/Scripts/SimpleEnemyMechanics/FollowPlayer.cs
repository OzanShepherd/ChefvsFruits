using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowPlayer : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!player) return;

        // Calculate direction to player
        Vector3 direction = (player.position - transform.position);
        direction.y = 0f; // Keep movement horizontal
        direction.Normalize();

        // Always move forward — no matter the rotation
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // Smooth rotation (optional, doesn't affect movement)
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }
}
