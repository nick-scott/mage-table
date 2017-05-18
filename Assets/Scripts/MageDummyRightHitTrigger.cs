using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageDummyRightHitTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + " Entered right collider of the mage");
        animator.enabled = true;
        animator.SetTrigger("HitRight");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other + " Is within right collider of the mage");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other + " Left the right collider of the mage");
    }
}