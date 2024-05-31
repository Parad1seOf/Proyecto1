using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EfectodeSonido : MonoBehaviour
{
    [SerializeField] private AudioClip colectar1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ControladorSonido.Instance.EjecutarSonido(colectar1);
        }
    }
}
