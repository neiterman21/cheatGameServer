// Decompiled with JetBrains decompiler
// Type: CheatGame.Extensions
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Net;
using System.Xml;

namespace CheatGame
{
  public static class Extensions
  {
    public static void SetAttribute(this XmlElement element, string name, object value)
    {
      element.SetAttribute(name, value == null ? "null" : value.ToString());
    }

    public static void FromString(this IPEndPoint ipEndPoint, string s)
    {
      string[] strArray = s.Split(':');
      ipEndPoint.Address = IPAddress.Parse(strArray[0]);
      ipEndPoint.Port = int.Parse(strArray[1]);
    }

    public static string ToCash(this double cash)
    {
      return "$" + cash.ToString("N2");
    }

    public static string ToStringX(this TimeSpan timeSpan, string format)
    {
      return format.ToLower().Replace("dd", timeSpan.Days.ToString("00")).Replace("hh", timeSpan.Hours.ToString("00")).Replace("mm", timeSpan.Minutes.ToString("00")).Replace("ss", timeSpan.Seconds.ToString("00")).Replace("fff", timeSpan.Milliseconds.ToString("000"));
    }

    public static string ToTimeString(this TimeSpan timeSpan)
    {
      return string.Format("{0:00}:{1:00}", (object) timeSpan.Minutes, (object) timeSpan.Seconds);
    }

    public static bool GetParamBoolean(this XmlDocument doc, string nodeName)
    {
      return bool.Parse(doc.GetParamString(nodeName));
    }

    public static string GetParamString(this XmlDocument doc, string nodeName)
    {
      return doc.DocumentElement.GetElementsByTagName(nodeName)[0].Attributes["value"].Value;
    }

    public static int GetParamInt32(this XmlDocument doc, string nodeName)
    {
      return int.Parse(doc.GetParamString(nodeName));
    }

    public static double GetParamDouble(this XmlDocument doc, string nodeName)
    {
      return double.Parse(doc.GetParamString(nodeName));
    }

    public static TimeSpan GetParamTimeSpan(this XmlDocument doc, string nodeName)
    {
      return TimeSpan.FromSeconds((double) int.Parse(doc.GetParamString(nodeName)));
    }
  }
}
