// Decompiled with JetBrains decompiler
// Type: CheatGame.Game
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Linq;
using System.Xml;

namespace CheatGame
{
  public class Game : Container<Session>
  {
    public Archive ParentArchive { get; private set; }

    public int PlayerWonId { get; set; }

    public int Index { get; private set; }

    public Game PreviousGame
    {
      get
      {
        return this.Index == 0 ? (Game) null : this.ParentArchive.DerivedItemsList[this.Index - 1];
      }
    }

    public int PlayMoveTurnsCount
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.PlayMoveTurnsCount));
      }
    }

    public int FalseClaimsCount
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.FalseClaimsCount));
      }
    }

    public int TakeCardTurnsCount
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.TakeCardTurnsCount));
      }
    }

    public int PlayMoveTurnsCountP0
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.PlayMoveTurnsCountP0));
      }
    }

    public int FalseClaimsCountP0
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.FalseClaimsCountP0));
      }
    }

    public int TakeCardTurnsCountP0
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.TakeCardTurnsCountP0));
      }
    }

    public int PlayMoveTurnsCountP1
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.PlayMoveTurnsCountP1));
      }
    }

    public int FalseClaimsCountP1
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.FalseClaimsCountP1));
      }
    }

    public int TakeCardTurnsCountP1
    {
      get
      {
        return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (s => s.TakeCardTurnsCountP1));
      }
    }

    public Game(Archive parent)
    {
      this.ParentArchive = parent;
      this.Index = parent.DerivedItemsList.Count;
      parent.DerivedItemsList.Add(this);
    }

    private int GetTotalTurnsCount()
    {
      return this.DerivedItemsList.Sum<Session>((Func<Session, int>) (session => session.DerivedItemsList.Count)) - 1;
    }

    public override void SaveSummary(XmlElement Root)
    {
      try
      {
        base.SaveSummary(Root);
        Root.SetAttribute("PlayerWonIndex", this.PlayerWonId.ToString());
        Root.SetAttribute("TotalTurnsCount", (object) this.GetTotalTurnsCount());
        Root.SetAttribute("FalseClaimsCount", (object) this.FalseClaimsCount);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed in Game.SaveSummary(). Error: " + ex.Message);
      }
    }
  }
}
