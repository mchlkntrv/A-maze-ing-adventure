using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;

    private void Update()
    {
        if (target != null)
        {
            Vector3 newPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
