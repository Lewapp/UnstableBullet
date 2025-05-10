using UnityEngine;

public class MimicPosition : MonoBehaviour
{
    public GameObject copyObject;

    private void Update()
    {
        if (!copyObject)
            return;

        transform.position = new Vector3(-copyObject.transform.position.x, -copyObject.transform.position.y, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, -copyObject.transform.eulerAngles.z);
    }
}
