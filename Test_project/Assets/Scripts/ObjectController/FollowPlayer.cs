using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new  Vector3(0, 1, -10);
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        
    }

    void Update()
    {
        transform.position = offset + player.transform.position;
    }
}
