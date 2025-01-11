using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Tham chiếu đến đối tượng nhân vật
    public float smoothSpeed = 0.125f; // Tốc độ làm mượt di chuyển camera
    public Vector3 offset; // Khoảng cách giữa camera và nhân vật

    void LateUpdate()
    {
        // Tính toán vị trí mới của camera
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
