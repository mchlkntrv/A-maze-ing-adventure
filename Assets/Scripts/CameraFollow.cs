using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target != null)
        {
            Vector3 newPosition = new(target.position.x, target.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
