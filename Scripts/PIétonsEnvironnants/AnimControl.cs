using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimControl : MonoBehaviour
{
    void Start()
    {
        Animator anim = this.GetComponent<Animator>();
        anim.SetBool("isWalking", true);
    }
}
