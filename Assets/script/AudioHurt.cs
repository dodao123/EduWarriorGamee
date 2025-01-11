using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHurt : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        // Tìm AudioManager bằng tag "Audio"
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Phát âm thanh khi người chơi va chạm với item
            audioManager.PlaySFX(audioManager.hurt);
        }
        if (collision.CompareTag("Knife"))
        {
            // Phát âm thanh khi người chơi va chạm với item
            audioManager.PlaySFX(audioManager.ThrowKnife);
        }

    }
}
