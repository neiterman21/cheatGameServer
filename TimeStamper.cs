// Decompiled with JetBrains decompiler
// Type: CheatGame.TimeStamper
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Diagnostics;

namespace CheatGame
{
  public static class TimeStamper
  {
    private static DateTime _baseTimeStamp = DateTime.Now;
    private static Stopwatch _stopWatch = Stopwatch.StartNew();

    public static DateTime BaseDateTime
    {
      get
      {
        return TimeStamper._baseTimeStamp;
      }
    }

    public static void Start()
    {
      TimeStamper._stopWatch.Start();
    }

    public static void Stop()
    {
      TimeStamper._stopWatch.Stop();
    }

    public static TimeSpan Time
    {
      get
      {
        return TimeStamper._baseTimeStamp.TimeOfDay + TimeStamper._stopWatch.Elapsed;
      }
    }

    public static TimeSpan GetTime()
    {
      return TimeStamper.Time;
    }
  }
}
