using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerControl))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerControl playerControl;
    private Rigidbody playerRb;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float speed = Input.GetAxis("Vertical");

        animator.SetBool("Static_b", true);
        animator.SetFloat("Speed_f", speed);
    }
}
