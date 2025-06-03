using System.Diagnostics;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using UnityEngine.AI;

public enum GPA
{
  F = 12,
  Dminus = 11, Dzero = 10, Dplus = 9,
  Cminus = 8, Czero = 7, Cplus = 6,
  Bminus = 5, Bzero = 4, Bplus = 3,
  Aminus = 2, Azero = 1, Aplus = 0
}

public class EndingSceneInfos
{

  private GPA gpa;
  private int currentCentiseconds;
  private int aPlusCentiseconds;
  private int fCentiseconds;

  private string mapName;

  private Timer timer;

  private float gaugeFill;
  private bool isTutorial;


  public EndingSceneInfos(int currentCentiseconds_, int aPlusCentiseconds_, int fCentiseconds_, string mapName_)
  {
    // basic datas
    mapName = mapName_;
    currentCentiseconds = currentCentiseconds_;
    if (aPlusCentiseconds_ > fCentiseconds_)
    {
      aPlusCentiseconds = fCentiseconds_;
      fCentiseconds = aPlusCentiseconds_;
    }
    else
    {
      aPlusCentiseconds = aPlusCentiseconds_;
      fCentiseconds = fCentiseconds_;
    }

    //calculated datas
    gpa = CalculateGPA(currentCentiseconds_, aPlusCentiseconds_, fCentiseconds_);
    gaugeFill = CalculateFillAmount(currentCentiseconds_, aPlusCentiseconds_, fCentiseconds_);
    timer = new Timer(currentCentiseconds_);
    isTutorial = false;
  }
  public void SetInfos(int currentCentiseconds_, int aPlusCentiseconds_, int fCentiseconds_, string mapName_)
  {
    // basic datas
    mapName = mapName_;
    currentCentiseconds = currentCentiseconds_;
    if (aPlusCentiseconds_ > fCentiseconds_)
    {
      aPlusCentiseconds = fCentiseconds_;
      fCentiseconds = aPlusCentiseconds_;
    }
    else
    {
      fCentiseconds = aPlusCentiseconds_;
      aPlusCentiseconds = fCentiseconds_;
    }

    //calculated datas
    gpa = CalculateGPA(currentCentiseconds_, aPlusCentiseconds_, fCentiseconds_);
    gaugeFill = CalculateFillAmount(currentCentiseconds_, aPlusCentiseconds_, fCentiseconds_);
    timer = new Timer(currentCentiseconds_);
    isTutorial = false;
  }

  private GPA CalculateGPA(int currentScore, int aPlusScore, int fScore)
  {

    // index == 0 is A+, index == 12 is F
    int range = fScore - aPlusScore;
    range = range >= 0 ? range : -range;
    // if (range <= 0)
    // {
    //   UnityEngine.Debug.LogWarning("Invalid centisecond range for GPA calculation.");
    //   return GPA.F;
    // }
    if (currentScore <= aPlusScore) return GPA.Aplus;
    else if (currentScore > fScore) return GPA.F;
    else
    {
      float interval = (float)range / 11;
      float relativeScore = currentScore - aPlusScore;
      int index = Mathf.CeilToInt(relativeScore / interval);
      index = index >= 12 ? 12 : (index < 1 ? 1 : index); // Ensure 0 <= index <= 12

      return (GPA)index;

    }
  }

  private float CalculateFillAmount(int currentScore, int aPlusScore, int fScore)
  {
    if (currentScore >= fScore)
    {

      return 0f;
    }
    // 1/13 = 0.0769
    else if (currentScore <= aPlusScore)
    {
      if (aPlusScore <= 0)
      {
        UnityEngine.Debug.LogWarning("A+ cutline is 0. Should adjust it.");
        return (float)0.95f;
      }
      return 1f - (float)currentScore / aPlusScore * 0.0769f;
    }
    else if (currentScore <= fScore)
    {
      return ((float)fScore - currentScore) / (fScore - aPlusScore) * 0.8462f + 0.0769f;
    }
    else
    {
      // below Fcut 
      return 0.0f;
    }
  }

  public string GetTimerString()
  {
    return timer.ToString();
  }

  public int GetFCentiseconds()
  {
    return fCentiseconds;
  }
  public int GetAPlusCentiseconds()
  {
    return aPlusCentiseconds;
  }

  public int GetCurrentScore()
  {
    return currentCentiseconds;
  }

  public GPA GetGPA()
  {
    return gpa;
  }

  public string GetGPAString()
  {

    //Caution! might be slow. Don't use on update
    return gpa.ToString();
  }

  public float GetFillAmount()
  {
    return gaugeFill;
  }

  public string GetMapName()
  {
    return mapName;
  }

  public int GetMapIndex()
  {
    Match match = Regex.Match(mapName, @"\d+$");

    if (match.Success)
    {
      return int.Parse(match.Value);
    }
    return -1;
  }

  public bool IsTutorial()
  {
    return isTutorial;
  }

}