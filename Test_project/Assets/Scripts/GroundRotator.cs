using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotator : MonoBehaviour
{
    
    [Header("Rotation Settings")]
    public float maxRotationSpeed = 45f;      
    public float rotationAcceleration = 60f;  

    private float currentAngularVelocity = 0f;
    private float currentRotationInput = 0f; 

    

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        currentRotationInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentRotationInput = 1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentRotationInput = -1f;
        }

        float targetAngularVelocity = currentRotationInput * maxRotationSpeed;

        currentAngularVelocity = Mathf.MoveTowards(
            currentAngularVelocity,
            targetAngularVelocity,
            rotationAcceleration * Time.fixedDeltaTime 
        );

        if (!Mathf.Approximately(currentAngularVelocity, 0f))
        {
            float rotationAmount = currentAngularVelocity * Time.fixedDeltaTime;
            transform.Rotate(Vector3.forward, rotationAmount, Space.World);

        }
    }

   
    public Vector3 GetAngularVelocity()
    {       
        return Vector3.forward * currentAngularVelocity * Mathf.Deg2Rad;
    }
    
    
}
