// Decompiled with JetBrains decompiler
// Type: CheatGame.Session
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Network.Messages;
using System;
using System.Linq;
using System.Xml;

namespace CheatGame
{
  public class Session : Container<Turn>
  {
    public Game ParentGame { get; private set; }

    public int Index { get; private set; }

    public Session PreviousSession
    {
      get
      {
        return this.Index == 0 ? (Session) null : this.ParentGame.DerivedItemsList[this.Index - 1];
      }
    }

    public Turn LastTurn
    {
      get
      {
        return this.DerivedItemsList == null || this.DerivedItemsList.Count == 0 ? (Turn) null : this.DerivedItemsList[this.DerivedItemsList.Count - 1];
      }
    }

    public int PlayMoveTurnsCount
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => t.MoveType == MoveType.PlayMove));
      }
    }

    public int FalseClaimsCount
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => t.MoveType == MoveType.PlayMove && !t.IsTrueClaim));
      }
    }

    public int TakeCardTurnsCount
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => (t.MoveType == MoveType.TakeCard || t.MoveType == MoveType.TimeUp)));
      }
    }

    public int PlayMoveTurnsCountP0
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => t.MoveType == MoveType.PlayMove && t.PlayerIndex == 0));
      }
    }

    public int FalseClaimsCountP0
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => t.MoveType == MoveType.PlayMove && !t.IsTrueClaim && t.PlayerIndex == 0));
      }
    }

    public int TakeCardTurnsCountP0
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => (t.MoveType == MoveType.TakeCard || t.MoveType == MoveType.TimeUp) && t.PlayerIndex == 0));
      }
    }

    public int PlayMoveTurnsCountP1
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => t.MoveType == MoveType.PlayMove && t.PlayerIndex == 1));
      }
    }

    public int FalseClaimsCountP1
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => t.MoveType == MoveType.PlayMove && !t.IsTrueClaim && t.PlayerIndex == 1));
      }
    }

    public int TakeCardTurnsCountP1
    {
      get
      {
        return this.DerivedItemsList.Count<Turn>((Func<Turn, bool>) (t => (t.MoveType == MoveType.TakeCard || t.MoveType == MoveType.TimeUp) && t.PlayerIndex == 1));
      }
    }

    public Session(Game parent)
    {
      this.ParentGame = parent;
      this.Index = parent.DerivedItemsList.Count;
      parent.DerivedItemsList.Add(this);
    }

    public override void SaveSummary(XmlElement Root)
    {
      try
      {
        base.SaveSummary(Root);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed in Session.SaveSummary(). Error: " + ex.Message);
      }
    }
  }
}
