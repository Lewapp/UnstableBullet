using UnityEngine;

public class ShootAtTarget : MonoBehaviour
{
    public GameObject projectile;
    public Transform spawnTransform;
    public float tickRate;
    public float projectileSpeed;

    private float currentTime;

    private void Start()
    {
        if (spawnTransform == null)
            spawnTransform = transform;
    }

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

            GameObject spawnedProjectile = Instantiate(projectile, spawnTransform.position, Quaternion.Euler(0f, 0f, angle));
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
