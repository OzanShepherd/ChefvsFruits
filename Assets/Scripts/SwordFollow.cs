using UnityEngine;

public class SwordFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 localOffset = new Vector3(0f, 1f, 1f); // in front of player

    void LateUpdate()
    {
        if (player == null) return;

        // Update position and rotation to stay in front
        transform.position = player.position + player.forward * localOffset.z + Vector3.up * localOffset.y;
        transform.rotation = Quaternion.LookRotation(player.forward, Vector3.up);
    }
}
