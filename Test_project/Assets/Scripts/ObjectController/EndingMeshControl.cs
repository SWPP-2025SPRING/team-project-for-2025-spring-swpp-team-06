using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMeshControl : MonoBehaviour
{

    public int aPlusCentiseconds = 0;
    public int fCentiseconds = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (aPlusCentiseconds == 0) aPlusCentiseconds = 100;
        if (fCentiseconds == 0) fCentiseconds = 1500;
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
                EndingSceneDataHolder.endingSceneInfos = new EndingSceneInfos(-1, -1, -1, "TitleScene");
            }
            EndingSceneDataHolder.endingSceneInfos.SetInfos(centiseconds, aPlusCentiseconds, fCentiseconds, SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("EndingScene");
        }
    }
}
