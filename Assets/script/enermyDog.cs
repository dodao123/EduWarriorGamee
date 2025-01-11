using UnityEngine;

public class EnemyDog : MonoBehaviour
{
    public Transform target;          // Đối tượng mục tiêu
    public float detectionRadius = 10f;  // Bán kính phát hiện mục tiêu, có thể điều chỉnh từ Unity Editor
    public float moveSpeed = 5f;      // Tốc độ di chuyển của đối tượng
    private Vector3 scaleOriginal;
    public Rigidbody2D rb;
    public Vector3 fixedRotation;
    public Animator anim;
    public float direct;

    float distance;                  // Khoảng cách hiện tại giữa đối tượng và mục tiêu

    // Start is called before the first frame update
    void Start()
    {
        // Khởi tạo nếu cần
        scaleOriginal = transform.localScale;
        transform.rotation = Quaternion.Euler(fixedRotation);
        rb.gravityScale = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Tính khoảng cách từ đối tượng đến mục tiêu
            distance = Vector3.Distance(transform.position, target.position);

            // Kiểm tra nếu khoảng cách nhỏ hơn hoặc bằng bán kính phát hiện
            if (distance <= detectionRadius)
            {
                // Di chuyển đối tượng về phía mục tiêu
                direct = 1;
                MoveTowardsTarget();
            }
            else
            {
                direct = 0;
            }
            Debug.Log("Speed: " + direct); // Để kiểm tra giá trị direct
            anim.SetFloat("speed", direct);
        }
        transform.rotation = Quaternion.Euler(fixedRotation);
    }

    // Phương thức để di chuyển đối tượng về phía mục tiêu
    void MoveTowardsTarget()
    {
        // Tính hướng di chuyển từ vị trí hiện tại tới vị trí mục tiêu
        Vector3 direction = (target.position - transform.position).normalized;

        // Tính vị trí tiếp theo của đối tượng
        Vector3 nextPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        // Di chuyển đối tượng đến vị trí tiếp theo
        transform.position = nextPosition;

        // Tuỳ chọn: Cập nhật hướng đối tượng để hướng về phía mục tiêu
        // transform.LookAt(target); // Nếu bạn đang sử dụng 3D. Đối với 2D, sử dụng cách khác.
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Bắt đầu animation "death"
            anim.SetBool("is_death", true);

            // Hủy đối tượng sau một khoảng thời gian nhỏ để animation có thể hoàn tất
            Destroy(gameObject, 1f); // Thay đổi thời gian nếu cần
        }
    }
}
