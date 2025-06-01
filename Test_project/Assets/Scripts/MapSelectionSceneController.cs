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
    public AudioSource unlockAudio;

    private const int mapCount = 6;
    void Start()
    {

        RefreshMapUnlock();

        //Debug.Log(PlayerPrefs.GetInt("ShouldUnlockNewMap", 0));

        if (PlayerPrefs.GetInt("ShouldUnlockNewMap", 0) == 1)
        {
            int index = PlayerPrefs.GetInt("NewMapToUnlock", -1);
            if (index >= 2 && index <= mapCount)
            {
                UnlockNewMap(index);
            }
        }

        PlayerPrefs.SetInt("ShouldUnlockNewMap", 0);
        PlayerPrefs.DeleteKey("NewMapToUnlock");
        PlayerPrefs.Save();
    }

    private void RefreshMapUnlock()
    {
        for (int i = 2; i <= 6; i++)
        {
            if (PlayerPrefs.GetInt("MapUnlocked" + i, 0) == 1)
            {
                UnlockMap(i); // 이미 언락된 맵은 자동 언락
            }
            else
            {
                LockMap(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickBackButton()
    {

        SceneManager.LoadScene("TitleScene");
    }

    public void UnlockMap(int mapIndex)
    {
        PlayerPrefs.SetInt("MapUnlocked" + mapIndex, 1);
        PlayerPrefs.Save();

        GameObject mapUnlocked = canvas.transform.Find("MapSelection" + mapIndex.ToString())?.gameObject;
        GameObject mapLocked = canvas.transform.Find("Locked" + mapIndex.ToString())?.gameObject;
        if (mapUnlocked != null && mapLocked != null)
        {
            mapUnlocked.SetActive(true);
            mapLocked.SetActive(false);
        }
        else
        {
            Debug.Log("MapSelection" + mapIndex.ToString() + " or Locked" + mapIndex.ToString() + " not found");
        }
    }

    public void LockMap(int mapIndex)
    {
        GameObject mapUnlocked = canvas.transform.Find("MapSelection" + mapIndex.ToString())?.gameObject;
        GameObject mapLocked = canvas.transform.Find("Locked" + mapIndex.ToString())?.gameObject;
        if (mapUnlocked != null && mapLocked != null)
        {
            mapUnlocked.SetActive(false);
            mapLocked.SetActive(true);
        }
        else
        {
            Debug.Log("MapSelection" + mapIndex.ToString() + " or Locked" + mapIndex.ToString() + " not found");
        }
    }

    public void UnlockNewMap(int index)
    {
        if (PlayerPrefs.GetInt("MapUnlocked" + index, 0) == 1)
        {
            // already unlocked
            return;
        }
        unlockAudio.Play();
        /*
        
        TODO

        새로 열리는 맵의 중심에 적절한 파티클 효과
        
        */
        StartCoroutine(WaitAndUnlock(index, unlockAudio.clip.length));
    }

    private IEnumerator WaitAndUnlock(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnlockMap(index);
    }

    public void OnClickTestButton()
    {
        UnlockNewMap(2);
    }

    public void OnClickResetMap2Button()
    {
        PlayerPrefs.SetInt("MapUnlocked" + 2, 0);
        PlayerPrefs.Save();
        RefreshMapUnlock();
    }
}
