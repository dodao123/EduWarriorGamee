using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform target1; // Đối tượng trống thứ nhất
    public Transform target2; // Đối tượng trống thứ hai
    public float speed = 2.0f; // Tốc độ di chuyển
    public float reachThreshold = 0.1f; // Ngưỡng để xác định khi nào đến gần mục tiêu

    private Transform currentTarget; // Mục tiêu hiện tại
    private bool movingToTarget1 = true; // Biến kiểm soát hướng di chuyển
    private bool isMoving = true; // Biến kiểm soát việc di chuyển
    public Animator anim;

    void Start()
    {
        // Bắt đầu di chuyển về target1
        currentTarget = target1;
        UpdateScale(); // Đặt hướng di chuyển ban đầu
    }

    void Update()
    {
        // Chỉ di chuyển nếu isMoving là true
        if (isMoving && currentTarget != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        // Tính toán bước di chuyển dựa trên tốc độ và thời gian khung hình
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, step);

        // Kiểm tra xem NPC đã đến gần mục tiêu chưa
        if (Vector3.SqrMagnitude(transform.position - currentTarget.position) < reachThreshold * reachThreshold)
        {
            // Đổi mục tiêu và hướng di chuyển
            currentTarget = movingToTarget1 ? target2 : target1;
            movingToTarget1 = !movingToTarget1;
            UpdateScale(); // Cập nhật hướng di chuyển
        }
    }

    private void UpdateScale()
    {
        // Thay đổi dấu hiệu trên trục X để thay đổi hướng di chuyển
        Vector3 newScale = transform.localScale;
        newScale.x = movingToTarget1 ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            Destroy(collision.gameObject); // Xóa đối tượng Knife
            isMoving = false; // Dừng NPC di chuyển
            anim.SetBool("isDeath", true); // Bật animation chết
            // Bạn có thể thêm logic để xóa NPC sau một khoảng thời gian nếu cần
            Destroy(this.gameObject, 1f); // Xóa NPC sau 1 giây
        }
    }
}
