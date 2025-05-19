public class Timer
{
  private int minute;
  private int second;
  private int centisecond;

  public Timer(int min, int sec, int centisec)
  {
    this.minute = min > 60 ? 60 : (min < 0 ? 0 : min);
    this.second = sec > 60 ? 60 : (sec < 0 ? 0 : sec);
    this.centisecond = centisec > 100 ? 100 : (centisec < 0 ? 0 : centisec);
  }

  public override string ToString()
  {
    return minute.ToString() + ":" + second.ToString() + ":" + centisecond.ToString();
  }

  public int GetMinute()
  {
    return minute;
  }
  public int GetSecond()
  {
    return second;
  }
  public int GetCentisecond()
  {
    return centisecond;
  }

  public int InCentiseconds()
  {
    return 6000 * minute + 100 * second + centisecond;
  }

}