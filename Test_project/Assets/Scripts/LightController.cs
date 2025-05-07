using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    public GameObject Player;
    public GameObject light1;
    public GameObject light2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        light1.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        light2.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z+10);
    }
}
