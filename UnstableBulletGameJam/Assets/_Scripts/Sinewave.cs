using UnityEngine;

public class Sinewave : MonoBehaviour
{
    public float frequency = 1f;
    public float amplitude = 0.5f;

    private Vector3 previousOffset;
    private Vector2 sideways;
    private float totalTime = 0f;

    void Start()
    {
        sideways = new Vector2(-transform.up.y, transform.up.x).normalized;
    }

    void LateUpdate()
    {
        // Remove the last frame's offset before applying the new one
        transform.position -= previousOffset;

        float sine = Mathf.Sin(totalTime * frequency) * amplitude;
        previousOffset = (Vector3)(sideways * sine);

        transform.position += previousOffset;

        totalTime += Time.deltaTime;
    }
}
