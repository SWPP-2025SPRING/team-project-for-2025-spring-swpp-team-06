using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    private GameObject player;
    public GameObject light1;
    public GameObject light2;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) Debug.LogError("No Player Object in this map, by LightController.cs");
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        light1.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        light2.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z+10);
    }
}
