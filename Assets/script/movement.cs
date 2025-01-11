using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        // Tìm AudioManager bằng tag "Audio"
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public Rigidbody2D rb;
    public float speed = 12f;
    public bool canMove = true;
    public float jumpForce = 7f;
    public bool is_FacingRight = true;
    public Vector3 fixedRotation;
    public Animator anim;
    public GameObject panelQuestion;
    public GameObject failAnswer;
    private bool isCorrect = false;
    public GameObject correctAnswer;
    public GameObject panelQuestion1;
    public GameObject panelQuestion2;
    public GameObject EndGame;

    private float moveHorizontal;
    private bool isGrounded = true; // Kiểm tra trạng thái mặt đất
    private int jumpCount = 0;
    public int maxJumpCount = 2;
    private bool isCrouch = false;
    public Collider2D Standing;
    public Collider2D Crouched;
    public float HeathPlayer = 6f;
    public GameObject KnifeSpawnPoint; // Object trống để ném dao từ đó
    public float ownKnife = 3f;
    public GameObject Knife;
    public float throwForce = 10f;

    private GameObject currentGameItem; // Lưu trữ GameItem hiện tại
    public float CheckGetItem = 0;

    private bool isOnLadder = false; // Kiểm tra xem nhân vật có trên cầu thang không
    public Transform leftEdge;  // Object trống bên trái
    public Transform rightEdge; // Object trống bên phải
    public Button AnswerButton;
    public Button AnswerButton1;
    public Button AnswerButton2;
    public GameObject Answer;
    public GameObject Answer1;
    public GameObject Answer2;
    public Button Return;
    public Button Continue;
    public Button Return1;
    public Button Continue1;
    public Button Return2;
    public Button Continue2;



    void Start()
    {
        transform.rotation = Quaternion.Euler(fixedRotation);
        rb.gravityScale = 5f;
        leftEdge = GameObject.FindGameObjectWithTag("LeftEdge").transform; // Gán cho object bên trái
        rightEdge = GameObject.FindGameObjectWithTag("RightEdge").transform; // Gán cho object bên phải
        AnswerButton.onClick.AddListener(OnAnswerButtonClick);
        Return.onClick.AddListener(backToStart);
        Continue.onClick.AddListener(continueGamePlay);
    }

    void Update()
    {
        Move();
        Crouch();
        Reborn();
        Throw();
        Answering();

        if (isOnLadder)
        {
            ClimbLadder();
        }
    }

    void Move()
    {
        if (!canMove) return; // Ngừng di chuyển nếu không được phép

        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || jumpCount < maxJumpCount)
            {
                Jump();
            }
        }

        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        // Flip nhân vật
        if (moveHorizontal > 0 && !is_FacingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && is_FacingRight)
        {
            Flip();
        }

        float speeds = Mathf.Abs(moveHorizontal);
        anim.SetFloat("speed", speeds);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    void Answering()
    {
        // Kiểm tra nếu bất kỳ panel nào đang hoạt động
        if (panelQuestion.activeSelf || panelQuestion1.activeSelf || panelQuestion2.activeSelf)
        {
            canMove = false; // Nếu có panel hoạt động, ngừng di chuyển
        }
        else
        {
            canMove = true; // Nếu không có panel nào hoạt động, cho phép di chuyển
        }
    }


    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Thay đổi vận tốc theo trục Y để nhảy
        jumpCount++; // Tăng số lần nhảy
        isGrounded = false; // Đặt trạng thái không còn trên mặt đất
        anim.SetBool("isJumping", isGrounded);
    }

    void Flip()
    {
        is_FacingRight = !is_FacingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Nhân vật đã chạm đất
            jumpCount = 0; // Đặt lại số lần nhảy khi chạm đất
            rb.gravityScale = 5f;
            anim.SetBool("isJumping", isGrounded);
        }
        if (collision.gameObject.CompareTag("GroundSpell"))
        {
            rb.gravityScale = 5f;
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded) // Kiểm tra nếu nhân vật đang đứng và nhấn Shift
        {
            SwitchToCrouch();
            anim.SetBool("isCrouch", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) // Khi nhả Shift
        {
            SwitchToStanding();
            anim.SetBool("isCrouch", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Nhân vật rời khỏi mặt đất
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            anim.SetBool("isLadder", true);
            isOnLadder = true; // Nhân vật vào cầu thang
            rb.gravityScale = 0; // Vô hiệu hóa trọng lực khi leo
            transform.position = new Vector2((leftEdge.position.x + rightEdge.position.x) / 2, transform.position.y);
        }

        if (collision.gameObject.CompareTag("GameItem"))
        {
            currentGameItem = collision.gameObject; // Lưu GameItem hiện tại
            panelQuestion.SetActive(true);
            AnswerButton.gameObject.SetActive(false);
            CheckGetItem+=1;
        }
        else if (collision.gameObject.CompareTag("GameItem1"))
        {
            currentGameItem = collision.gameObject;
            panelQuestion1.SetActive(true); // Hiện câu hỏi
            CheckGetItem+=1;
        }
        if (collision.gameObject.CompareTag("GameItem2"))
        {
            currentGameItem = collision.gameObject; // Lưu GameItem hiện tại
            panelQuestion2.SetActive(true);
            CheckGetItem += 1;
        }
        else if (collision.gameObject.CompareTag("Success"))
        {
            if(CheckGetItem >= 5)
            {
                canMove = false;
                EndGame.SetActive(true);
                audioManager.PlaySFX(audioManager.Jumping);
            }
            //EndGame.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Animal"))
        {
            HeathPlayer -= 1;
            anim.SetBool("Hurt", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Animal"))
        {
            anim.SetBool("Hurt", false);
        }
        if (collision.CompareTag("Ladder"))
        {
            anim.SetBool("isLadder", false);
            isOnLadder = false; // Nhân vật rời khỏi cầu thang
            rb.gravityScale = 5; // Khôi phục trọng lực
        }
    }

    public void Fail()
    {
        if (panelQuestion)
        {
            panelQuestion.SetActive(true);
            

            AnswerButton.gameObject.SetActive(true);
            if (Answer)
            {
                Return.gameObject.SetActive(true);
            }
        }

        if (panelQuestion1.activeSelf)
        {
            panelQuestion.SetActive(false);
            panelQuestion1.SetActive(true);

            AnswerButton1.gameObject.SetActive(true);
            if (Answer1)
            {
                Return.gameObject.SetActive(true);
            }
        }
        if (panelQuestion2.activeSelf)
        {   
            panelQuestion.SetActive(false);
            panelQuestion2.SetActive(true);
            AnswerButton2.gameObject.SetActive(true);
            if (Answer2)
            {
                Return2.gameObject.SetActive(true);
                
            }
        }
        failAnswer.SetActive(true);
        Invoke("DeactivateFailAnswer", 0.8f);
        isCorrect = false;
        currentGameItem = null; // Reset GameItem hiện tại
    }

    public void OnAnswerButtonClick()
    {
        // Logic khi nút được nhấn
        if (panelQuestion.activeSelf)
        {
            Answer.SetActive(true);
            Return.gameObject.SetActive(true);
        }
        if (panelQuestion1.activeSelf)
        {
            Answer1.SetActive(true);
            Return1.gameObject.SetActive(true);
        }
        if (panelQuestion2.activeSelf)
        {
            Answer2.SetActive(true);
            Return2.gameObject.SetActive(true);
        }
    }

    public void backToStart()
    {
        // Khôi phục sức khỏe tối đa
        HeathPlayer = 0f; // Giả sử 6 là sức khỏe tối đa của nhân vật
        Answer.SetActive(false);
        Answer1.SetActive(false);
        Answer2.SetActive(false);
    }

    public void continueGamePlay()
    {
        if (Return) { 
        panelQuestion.SetActive(false);
        }
        if (Return1)
        {
            panelQuestion1.SetActive(false);
        }
        if (Return2)
        {
            panelQuestion2.SetActive(false);
        }

    }



    public void correct()
    {
        if (panelQuestion)
        {
            panelQuestion.SetActive(true);
            AnswerButton.gameObject.SetActive(true);
            if (Answer)
            {
                Continue.gameObject.SetActive(true);
            }
        }   
        if (panelQuestion1.activeSelf)
        {
            panelQuestion.SetActive(false);
            panelQuestion1.SetActive(true);
            AnswerButton1.gameObject.SetActive(true);
            if (Answer1)
            {
                Continue1.gameObject.SetActive(true);
            }
        }
        if (panelQuestion2.activeSelf)
        {
            panelQuestion.SetActive(false) ;
            panelQuestion2.SetActive(true);
            AnswerButton2.gameObject.SetActive(true);
            if (Answer2)
            {
                Continue2.gameObject.SetActive(true);
            }
        }
        correctAnswer.SetActive(true);
        Invoke("DeactivateCorrectAnswer", 0.8f);
        isCorrect = true; // Đặt trạng thái là đúng

        // Xóa GameItem nếu đáp án đúng
        if (currentGameItem != null)
        {
            Destroy(currentGameItem); // Xóa đối tượng va chạm
            currentGameItem = null; // Reset GameItem hiện tại
        }
        ownKnife += 3f;
    }

    private void DeactivateCorrectAnswer()
    {
        correctAnswer.SetActive(false);
    }

    private void DeactivateFailAnswer()
    {
        failAnswer.SetActive(false);
    }

    void SwitchToCrouch()
    {
        Standing.enabled = false;
        Crouched.enabled = true;
        isCrouch = true;
    }

    void SwitchToStanding()
    {
        Standing.enabled = true;
        Crouched.enabled = false;
        isCrouch = false;
    }

    void Reborn()
    {
        if (HeathPlayer <= 0)
        {
            // Đặt lại vị trí và trạng thái của nhân vật
            transform.position = new Vector3(-27.1f, -5.8f, 0); // Ví dụ: đặt lại vị trí về điểm bắt đầu
            rb.velocity = Vector2.zero; // Đặt lại vận tốc
            HeathPlayer = 5f; // Khôi phục điểm sức khỏe

            // Khôi phục các trạng thái khác của nhân vật nếu cần
            is_FacingRight = true;
            isGrounded = true;
            jumpCount = 0;
            anim.SetBool("isJumping", false);
            anim.SetBool("isCrouch", false);
            SwitchToStanding(); // Khôi phục trạng thái đứng

            // Ẩn hoặc tắt các yếu tố không cần thiết
            failAnswer.SetActive(false);
            correctAnswer.SetActive(false);
            panelQuestion.SetActive(false);
            panelQuestion1.SetActive(false);
            panelQuestion2.SetActive(false);
            EndGame.SetActive(false);
        }
    }

    void Throw()
    {
        if (ownKnife > 0 && Input.GetKeyDown(KeyCode.E))
        {
            audioManager.PlaySFX(audioManager.walk);
            // Tạo dao từ KnifeSpawnPoint
            GameObject thrownKnife = Instantiate(Knife, KnifeSpawnPoint.transform.position, Quaternion.identity);
            Rigidbody2D knifeRb = thrownKnife.GetComponent<Rigidbody2D>();

            // Thiết lập hướng ném
            if (is_FacingRight)
            {
                knifeRb.AddForce(Vector2.right * throwForce, ForceMode2D.Impulse);
            }
            else
            {
                knifeRb.AddForce(Vector2.left * throwForce, ForceMode2D.Impulse);
            }

            ownKnife--; // Giảm số dao
            StartCoroutine(DestroyKnifeAfterDistance(thrownKnife));
        }
    }

    private IEnumerator DestroyKnifeAfterDistance(GameObject knife)
    {
        float distanceTraveled = 0f;
        float maxDistance = 15f;

        while (distanceTraveled < maxDistance)
        {
            distanceTraveled += Time.deltaTime * knife.GetComponent<Rigidbody2D>().velocity.magnitude;
            yield return null;
        }

        Destroy(knife);
    }
    void ClimbLadder()
    {
        // Khóa di chuyển ngang
        float moveVertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(0, moveVertical * speed); // Di chuyển lên/xuống
        anim.SetBool("isMove", true);

        // Dừng lại nếu không có phím nào được nhấn
        if (moveVertical == 0)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isMove", false);
        }
    }

   
}
