using UnityEngine;

public class OnHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena"))
        {
            Destroy(gameObject);
        }
    }
}
