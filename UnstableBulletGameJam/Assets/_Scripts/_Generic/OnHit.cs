using UnityEngine;

public class OnHit : MonoBehaviour
{
    public string[] collisionTags;
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var tag in collisionTags)
        {
            if (!collision.CompareTag(tag))
                continue;

            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
