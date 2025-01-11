using UnityEngine;

public class FlyingEdge : MonoBehaviour
{
    public Transform pointA;          // Điểm bắt đầu
    public Transform pointB;          // Điểm kết thúc
    public float speed = 2f;          // Tốc độ di chuyển
    public Vector3 fixedRotation;     // Góc quay cố định
    public bool is_FacingRight = true; // Biến xác định hướng của chim

    private bool movingToB = true;    // Đang di chuyển đến điểm B

    private void Start()
    {
        transform.rotation = Quaternion.Euler(fixedRotation);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(fixedRotation);

        // Di chuyển con chim đến điểm mục tiêu
        Transform target = movingToB ? pointB : pointA;
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Quay đầu khi di chuyển
        if (direction.x < 0 && !is_FacingRight)
        {
            Flip();
        }
        else if (direction.x > 0 && is_FacingRight)
        {
            Flip();
        }

        // Kiểm tra nếu đã đến điểm mục tiêu
        if ((Vector2)transform.position == (Vector2)target.position)
        {
            // Chuyển sang điểm mục tiêu khác
            movingToB = !movingToB;
        }
    }

    void Flip()
    {
        is_FacingRight = !is_FacingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }


}
