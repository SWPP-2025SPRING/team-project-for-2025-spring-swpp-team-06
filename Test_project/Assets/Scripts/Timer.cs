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

  public Timer(int centiseconds)
  {
    this.minute = centiseconds / 6000 > 60 ? 60 : centiseconds / 6000;
    this.second = (centiseconds - (centiseconds / 6000) * 6000) / 100 > 60 ? 60 : (centiseconds - (centiseconds / 6000) * 6000);
    this.centisecond = centiseconds % 100 > 100 ? 100 : centiseconds % 100;
  }

  public override string ToString()
  {
    return $"{minute:D2}:{second:D2}:{centisecond:D2}";
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