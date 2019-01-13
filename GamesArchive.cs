// Decompiled with JetBrains decompiler
// Type: CheatGame.Container`1
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Collections.Generic;
using System.Xml;

namespace CheatGame
{
  public abstract class Container<D> : IContainer
  {
    public static int _totalItemsCounter = 0;
    public SpanMeasurement<TimeSpan> TimeMeasurement = new SpanMeasurement<TimeSpan>(new Func<TimeSpan>(TimeStamper.GetTime));
    public SpanMeasurement<int> ItemsMeasurement = new SpanMeasurement<int>(new Func<int>(Container<D>.GetItemsCounter));
    public List<D> DerivedItemsList = new List<D>();
    public const string TimeSpanFormat = "hh:mm:ss.fff";

    public static int GetItemsCounter()
    {
      return Container<D>._totalItemsCounter;
    }

    public Container()
    {
      ++Container<D>._totalItemsCounter;
    }

    public void SignalEnd()
    {
      this.TimeMeasurement.SignalEnd();
    }

    public XmlElement Root { get; private set; }

    public virtual void SaveSummary(XmlElement parentRoot)
    {
      parentRoot.SetAttribute(parentRoot.Name + "Index", this.ItemsMeasurement.OriginMeasurement.ToString());
      parentRoot.SetAttribute(typeof (D).Name + "sCount", this.DerivedItemsList.Count.ToString());
      parentRoot.SetAttribute("StartTime", this.TimeMeasurement.OriginMeasurement.ToStringX("hh:mm:ss.fff"));
      parentRoot.SetAttribute("Duration", this.TimeMeasurement.Duration.ToStringX("hh:mm:ss.fff"));
      int num = 0;
      foreach (D derivedItems in this.DerivedItemsList)
      {
        Turn turn = (object) derivedItems as Turn;
        if (turn == null || !(turn.PlayerName == "Board"))
        {
          this.Root = parentRoot.OwnerDocument.CreateElement(typeof (D).Name.ToString());
          this.Root.SetAttribute(this.Root.Name + "RelativeIndex", num.ToString());
          parentRoot.AppendChild((XmlNode) this.Root);
          ((IContainer) (object) derivedItems).SaveSummary(this.Root);
          ++num;
        }
      }
    }
  }
}
