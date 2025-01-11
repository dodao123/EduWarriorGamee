using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public Animator anim;
    private bool isCollected = false;

    void Start()
    {
        // Kiểm tra Animator được gán
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu collider là Player và gem chưa được thu thập
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {
            if (anim != null)
            {
                anim.SetBool("isCollected", true);
                isCollected = true;

                // Huỷ gem sau khi hoạt ảnh hoàn tất
                StartCoroutine(DestroyAfterAnimation());
            }
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Chờ đến khi hoạt ảnh bắt đầu
        yield return new WaitForSeconds(0.1f); // Thời gian ngắn để đảm bảo hoạt ảnh bắt đầu

        // Lấy thời gian dài của hoạt ảnh hiện tại
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Đợi cho đến khi hoạt ảnh hoàn tất
        yield return new WaitForSeconds(animationLength);

        // Huỷ gem sau khi hoạt ảnh hoàn tất
        Destroy(gameObject);
    }
}
