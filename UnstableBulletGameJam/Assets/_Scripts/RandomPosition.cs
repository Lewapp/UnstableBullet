using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public Vector2 outterBorderL;
    public Vector2 outterBorderU;
    public Vector2 innerBorderL;
    public Vector2 innerBorderU;

    private float moveSpeed = 1;
    private bool success;
    private int maxAttempts = 100;
    private Vector2 startPos;
    private Vector2 newPos;
    private float currentTime;

    private void Start()
    {
        startPos = transform.position;
        newPos = Vector2.zero;
    }

    private void Update()
    {
        int attempts = 0;
        while (!success)
        {
            newPos = new Vector2(Random.Range(outterBorderL.x, outterBorderU.x), Random.Range(outterBorderL.y, outterBorderU.y));

            if ((newPos.x < innerBorderL.x || newPos.x > innerBorderU.x) && (newPos.y < innerBorderL.y || newPos.y > innerBorderU.y))
            {
                success = true;
            }

            if (attempts >= maxAttempts)
                break;

            attempts++;
        }

        if (success)
        {
            currentTime += Time.deltaTime;
            transform.position = new Vector2(Mathf.Lerp(startPos.x, newPos.x, currentTime * moveSpeed), Mathf.Lerp(startPos.y, newPos.y, currentTime * moveSpeed));
        }

        if (currentTime >= 1)
        {
            transform.position = newPos;
            Destroy(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(outterBorderL, new Vector2(outterBorderU.x, outterBorderL.y));
        Gizmos.DrawLine(outterBorderU, new Vector2(outterBorderU.x, outterBorderL.y));
        Gizmos.DrawLine(outterBorderU, new Vector2(outterBorderL.x, outterBorderU.y));
        Gizmos.DrawLine(outterBorderL, new Vector2(outterBorderL.x, outterBorderU.y));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(innerBorderL, new Vector2(innerBorderU.x, innerBorderL.y));
        Gizmos.DrawLine(innerBorderU, new Vector2(innerBorderU.x, innerBorderL.y));
        Gizmos.DrawLine(innerBorderU, new Vector2(innerBorderL.x, innerBorderU.y));
        Gizmos.DrawLine(innerBorderL, new Vector2(innerBorderL.x, innerBorderU.y));
    }
}
