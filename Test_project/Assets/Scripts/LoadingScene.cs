using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider progressBar;
    public Canvas canvas;
    
    IEnumerator LoadScene(){
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(TitleSceneController.loadTo);
        TitleSceneController.loadTo = "";
        operation.allowSceneActivation = true;
        while(!operation.isDone){
            yield return null;
            if(progressBar.value < 0.9f){
                progressBar.value = Mathf.MoveTowards(progressBar.value, 0.9f, Time.deltaTime);
            }
            else if(progressBar.value >= 0.9f){
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
            }
        }
    }
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
