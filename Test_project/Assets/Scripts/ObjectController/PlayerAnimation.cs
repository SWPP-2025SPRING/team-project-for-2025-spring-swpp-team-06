using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerControl))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerControl playerControl;
    private GroundRotator groundRotator;
    private Rigidbody playerRb;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
        groundRotator = GameObject.FindWithTag("Ground")?.GetComponent<GroundRotator>();
        if (groundRotator == null) Debug.LogWarning("d");
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 groundVelocityAtPlayerPos = Vector3.zero;

        if (groundRotator != null)
        {
            Vector3 angularVelocity = groundRotator.GetAngularVelocity(); // rad/s
            groundVelocityAtPlayerPos = Vector3.Cross(angularVelocity, playerRb.position);
        }

        Vector3 relativeVelocity = playerRb.velocity - groundVelocityAtPlayerPos;
        float speed = relativeVelocity.magnitude;

        //Debug.Log(speed);

        animator.SetBool("Static_b", true);
        animator.SetFloat("Speed_f", speed);
    }
}