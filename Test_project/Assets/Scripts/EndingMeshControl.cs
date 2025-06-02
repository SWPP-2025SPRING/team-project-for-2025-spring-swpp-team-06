using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMeshControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            int centiseconds = InGameUIControl.timer.InCentiseconds();
            // Ending
            if (EndingSceneDataHolder.endingSceneInfos == null)
            {
                EndingSceneDataHolder.endingSceneInfos = new EndingSceneInfos(-1, false, -1, -1, -1, "TitleScene");
            }
            EndingSceneDataHolder.endingSceneInfos.SetInfos(centiseconds, false, 0, 100, 1500, SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("EndingScene");
        }
    }
}
