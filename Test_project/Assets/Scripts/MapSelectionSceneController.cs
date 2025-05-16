using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSelectionSceneController : MonoBehaviour
{
    // Start is called before the first frame update

    public Button backButton;
    public GameObject canvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBackButton(){

        SceneManager.LoadScene("TitleScene");
    }

    public void UnlockMap(int mapIndex){
        GameObject mapUnlocked =  canvas.transform.Find("MapSelection"+mapIndex.ToString())?.gameObject;
        GameObject mapLocked = canvas.transform.Find("Locked"+mapIndex.ToString())?.gameObject;
        if(mapUnlocked != null && mapLocked != null){
            mapUnlocked.SetActive(true);
            mapLocked.SetActive(false);
        }
        else{
            Debug.Log("MapSelection"+mapIndex.ToString() + " or Locked"+mapIndex.ToString() + " not found");
        }
    }

    public void OnClickTestButton(){
        UnlockMap(2);
    }
}
