using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        // Tìm AudioManager bằng tag "Audio"
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Phát âm thanh khi người chơi va chạm với item
            audioManager.PlaySFX(audioManager.GetItem);
        }
    }
}
