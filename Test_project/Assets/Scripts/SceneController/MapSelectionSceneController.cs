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
    public GameObject diamondParticlePrefab;

    private const int mapCount = 6;
    void Start()
    {
        PlayerPrefs.SetInt("MapUnlocked" + 1, 1);

        System.Diagnostics.Debug.Assert(Time.timeScale > 0.5f);

        RefreshMapUnlock();

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

        unlockAudio.volume = PlayerPrefs.GetFloat("Volume", 0.5f);

        if (SceneLoadManager.Instance.LoadTo == null) SceneLoadManager.Instance.LoadTo = "TitleScene";
    }

    private void RefreshMapUnlock()
    {
        // Caution! Do not use this function in frequent calls, like Update().
        for (int i = 1; i <= 6; i++)
        {
            if (PlayerPrefs.GetInt("MapUnlocked" + i, 0) == 1)
            {
                //Debug.Log("Map " + i + " is already unlocked.");
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
        SceneLoadManager.Instance.LoadTo = "TitleScene";
        SceneManager.LoadScene("Loading");
    }

    public void UnlockMap(int mapIndex)
    {
        PlayerPrefs.SetInt("MapUnlocked" + mapIndex, 1);
        PlayerPrefs.Save();

        GameObject imageToUnlock = GetUnlockedImageByIndex(mapIndex);
        GameObject imageToLock = GetLockedImageByIndex(mapIndex);


        // if(PlayerPrefs.GetInt("Best"+ mapIndex, -1) > 0){
        //     Debug.LogWarning("Map " + mapIndex + " already has a best score,"+PlayerPrefs.GetInt("Best: "+ mapIndex, -1)+" but was not unlocked yet.");
        // }

        if (imageToUnlock != null && imageToLock != null)
        {
            imageToUnlock.SetActive(true);
            imageToLock.SetActive(false);
        }
        else
        {
            Debug.LogError("MapSelection" + mapIndex.ToString() + " or Locked" + mapIndex.ToString() + " not found");
        }
    }

    public void LockMap(int mapIndex)
    {

        GameObject imageToUnlock = GetUnlockedImageByIndex(mapIndex);
        GameObject imageToLock = GetLockedImageByIndex(mapIndex);
        if (imageToUnlock != null && imageToLock != null)
        {
            imageToUnlock.SetActive(false);
            imageToLock.SetActive(true);
        }
        else
        {
            Debug.LogWarning("MapSelection" + mapIndex.ToString() + " or Locked" + mapIndex.ToString() + " not found");
        }
    }

    public void UnlockNewMap(int index)
    {
        if (PlayerPrefs.GetInt("MapUnlocked" + index, 0) == 1)
        {
            // already unlocked
            //Debug.LogWarning("Map " + index + " is already unlocked.");
            return;
        }
        unlockAudio.Play();
        PlayerPrefs.SetInt("MapUnlocked" + index, 1);
        Transform transformToPlay = GetLockedImageByIndex(index).transform;
        PlayDiamondParticles(30, transformToPlay); // particle effect
        StartCoroutine(WaitAndUnlock(index, unlockAudio.clip.length));
    }

    private IEnumerator WaitAndUnlock(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnlockMap(index);
    }

    public void OnClickTestButton()
    {
        for (int i = 1; i <= 6; i++)
        {
            if (PlayerPrefs.GetInt("MapUnlocked" + i, 0) == 0)
            {
                //unlock i map
                
                PlayerPrefs.SetInt("MapUnlocked" + i, 1);
                UnlockMap(i);
                break;
            }
        }
    }

    public void ResetAllButton()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Volume", 0.5f);
        PlayerPrefs.SetInt("MapUnlocked1", 1);
        PlayerPrefs.Save();
        RefreshMapUnlock();
    }

    public void PlayDiamondParticles(int count, Transform transform)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject p = Instantiate(diamondParticlePrefab, transform);
            RectTransform rt = p.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;

            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            p.GetComponent<DiamondParticleEffect>().direction = dir;
        }
    }

    private GameObject GetUnlockedImageByIndex(int mapIndex)
    {
        return canvas.transform.Find("MapSelection" + mapIndex.ToString())?.gameObject;
    }
    private GameObject GetLockedImageByIndex(int mapIndex)
    {
        return canvas.transform.Find("Locked" + mapIndex.ToString())?.gameObject;
    }
}
