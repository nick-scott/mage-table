using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageDummyHeadshotTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
//        Debug.Log(other + " Entered headshot collider of the mage");
        animator.SetTrigger("Crit1");
    }

    void OnTriggerStay(Collider other)
    {
//        Debug.Log(other + " Is within headshot collider of the mage");
    }

    void OnTriggerExit(Collider other)
    {
//        Debug.Log(other + " Left the headshot collider of the mage");
    }
}