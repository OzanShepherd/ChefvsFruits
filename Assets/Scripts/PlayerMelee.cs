using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    public float damage = 1f;

    private bool isAttacking = false;
    private PlayerInputActions input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        input = new PlayerInputActions();
        input.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Player.Attack.triggered)
        {
            Debug.Log("Mouse1 pressed via input action");
            isAttacking = true;
            Invoke(nameof(ResetAttack), 0.2f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isAttacking) return;

        FruitEnemy enemy = other.GetComponent<FruitEnemy>();
        if(enemy != null)
        {
            Debug.Log("hit " + other.name);
            enemy.TakeDamage(damage);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}
