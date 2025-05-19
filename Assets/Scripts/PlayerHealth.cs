using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    public float invincibilityTime = 1.0f;

    private float currentHeatlh;
    private bool isInvincible = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHeatlh = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        if(isInvincible)
        {
            Debug.Log("Player is invincible. No damage taken.");
            return;
        }

        currentHeatlh -= amount;
        Debug.Log("Player took damage. Current health: " + currentHeatlh);

        if(currentHeatlh <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        //Disable movement or trigger game over
        gameObject.SetActive(false);
    }

    private IEnumerator InvincibilityFlash()
    {
        isInvincible = true;

        Debug.Log("Player is now invincible");

        //Here is reserved for invincibility frame effects

        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;
        Debug.Log("Player is now vulnerable again.");
    }
}
