using UnityEngine;

public class SpinAround : MonoBehaviour
{
    public float spinSpeed;

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + spinSpeed * Time.deltaTime);
    }
}
