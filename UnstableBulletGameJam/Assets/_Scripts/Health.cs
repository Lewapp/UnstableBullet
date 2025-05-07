using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        HealthCheck();
    }

    public void Heal(float amount)
    {
        health += amount;
        HealthCheck();
    }

    private void HealthCheck()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetHealthPercent()
    {
        return health / maxHealth;
    }
}

public interface IDamageable
{
    public void TakeDamage(float amount);
    public void Heal(float amount);
    public float GetHealth();
    public float GetHealthPercent();
}

