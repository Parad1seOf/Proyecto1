using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectodeSonido : MonoBehaviour
{
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}
