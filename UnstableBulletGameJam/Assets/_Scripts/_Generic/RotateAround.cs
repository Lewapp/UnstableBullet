using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform pivot;
    public Transform focus;
    public float speed;

    private void Update()
    {
        if (!pivot || !focus)
            return;

        Vector2 toSelf = transform.position - pivot.transform.position;
        Vector2 toPlayer = focus.position - pivot.transform.position;

        float angle = Vector2.SignedAngle(toSelf, toPlayer);
        float direction = Mathf.Sign(angle);
        float rotationStep = speed * Time.deltaTime * direction;

        if (Mathf.Abs(rotationStep) > Mathf.Abs(angle))
            rotationStep = angle;

        transform.RotateAround(pivot.transform.position, Vector3.forward, rotationStep);
    }
}
