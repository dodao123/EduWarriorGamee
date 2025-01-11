using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator anim;
    private bool isOpen = false; // Trạng thái của rương

    void Update()
    {
        // Kiểm tra nếu nhấn phím Q và rương chưa mở
        if (isOpen && Input.GetKeyDown(KeyCode.Q))
        {
            CloseChest();
        }
        else if (!isOpen && Input.GetKeyDown(KeyCode.Q))
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Không làm gì ở đây nữa
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isOpen", false);
            anim.SetBool("opening",false);
        }
    }

    private void OpenChest()
    {
        anim.SetBool("isOpen", true);
        isOpen = true; // Đặt trạng thái là mở
    }

    private void CloseChest()
    {
        anim.SetBool("isOpen", false);
        isOpen = false; // Đặt trạng thái là đóng
    }
}
