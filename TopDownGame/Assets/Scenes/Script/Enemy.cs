using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int hp=100;
    [SerializeField]
    private Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("weapon"))
        { 
            Debug.Log("test");
            animator.SetTrigger("Hurt");

        }

    }
}
