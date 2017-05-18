using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageDummyLeftHitTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + " Entered left collider of the mage");
        animator.enabled = true;
        animator.SetTrigger("HitLeft");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other + " Is within left collider of the mage");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other + " Left the left collider of the mage");
    }
}
