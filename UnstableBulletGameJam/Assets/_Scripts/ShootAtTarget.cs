using UnityEngine;

public class ShootAtTarget : MonoBehaviour
{
    public GameObject projectile;
    public float tickRate;
    public float projectileSpeed;

    private float currentTime;

    public void Update()
    {
        if (!projectile) 
            return;

        currentTime += Time.deltaTime;

        if (currentTime >= tickRate)
        {
            currentTime = 0;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, angle));
            ApplyProjectileForce(spawnedProjectile);
        }
    }

    private void ApplyProjectileForce(GameObject sp)
    {
        Rigidbody2D rb = sp.GetComponent<Rigidbody2D>();

        if (rb == null)
            return;

        rb.linearVelocity = sp.transform.right * projectileSpeed;
    }
}
