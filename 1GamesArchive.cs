// Decompiled with JetBrains decompiler
// Type: CheatGame.Turn
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Network.Messages;
using System;
using System.Xml;

namespace CheatGame
{
  public class Turn : Container<CardsStruct>
  {
    public Player Player { get; set; }

    public Session ParentSession { get; private set; }

    public MoveType MoveType { get; set; }

    public TimeSpan MoveTime { get; set; }

    public string PlayerName { get; set; }

    public int PlayerIndex { get; set; }

    public int Index { get; private set; }

    public Turn PreviousTurn
    {
      get
      {
        return this.Index == 0 ? (Turn) null : this.ParentSession.DerivedItemsList[this.Index - 1];
      }
    }

    public Turn GamePreviousTurn
    {
      get
      {
        return this.PreviousTurn == null ? (this.ParentSession.PreviousSession == null ? (Turn) null : this.ParentSession.PreviousSession.LastTurn) : this.PreviousTurn;
      }
    }

    public bool IsTrueClaim
    {
      get
      {
        return this.DerivedItemsList != null && this.DerivedItemsList.Count == 2 && this.DerivedItemsList[0].ToString() == this.DerivedItemsList[1].ToString();
      }
    }

    public Turn(Session parent)
    {
      this.ParentSession = parent;
      this.Index = parent.DerivedItemsList.Count;
      parent.DerivedItemsList.Add(this);
    }

    public int GetAllClaimsCount()
    {
      int num = 0;
      try
      {
        Turn turn = this;
        do
        {
          if (turn.MoveType == MoveType.PlayMove)
            num += turn.GetNoneZeroCard(turn.DerivedItemsList[1]).Item2;
          else if (turn.MoveType == MoveType.StartPressed)
            ++num;
          turn = turn.PreviousTurn;
        }
        while (turn != null);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed in GetAllClaimsCount(). Error: " + ex.Message);
      }
      return num;
    }

    public Tuple<int, int> GetLastClaim()
    {
      Tuple<int, int> tuple = new Tuple<int, int>(-1, -1);
      try
      {
        tuple = this.MoveType != MoveType.PlayMove ? this.GetLastClaimRecursive() : this.GamePreviousTurn.GetLastClaimRecursive();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed in GetLastClaim(). Error: " + ex.Message);
      }
      return tuple;
    }

    private Tuple<int, int> GetLastClaimRecursive()
    {
      Tuple<int, int> tuple = new Tuple<int, int>(-1, -1);
      try
      {
        tuple = this.MoveType != MoveType.PlayMove ? (this.MoveType != MoveType.StartPressed ? this.GamePreviousTurn.GetLastClaimRecursive() : this.GetNoneZeroCard(this.DerivedItemsList[0])) : this.GetNoneZeroCard(this.DerivedItemsList[1]);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed in GetLastClaimRecursive(). Error: " + ex.Message);
      }
      return tuple;
    }

    private Tuple<int, int> GetNoneZeroCard(CardsStruct cards)
    {
      for (int index = 0; index < 13; ++index)
      {
        if (cards.Cards[index] != 0)
          return new Tuple<int, int>(index, cards.Cards[index]);
      }
      return new Tuple<int, int>(-1, -1);
    }

    public override void SaveSummary(XmlElement Root)
    {
      base.SaveSummary(Root);
      try
      {
        Root.RemoveAttribute("CardsStructsCount");
        Root.SetAttribute("MoveType", this.MoveType.ToString());
        Root.SetAttribute("MoveTime", this.MoveTime.ToStringX("hh:mm:ss.fff"));
        Root.SetAttribute("ByPlayer", this.PlayerName);
        Root.SetAttribute("PlayerIndex", (object) this.PlayerIndex);
        Root.SetAttribute("PlayerIndex", (object) this.PlayerIndex);
        Tuple<int, int> lastClaim = this.GetLastClaim();
        Root.SetAttribute("LastClaimCard", (object) (lastClaim.Item1 + 1));
        Root.SetAttribute("LastClaimCount", (object) lastClaim.Item2);
        Root.SetAttribute("AllClaimsCount", (object) this.GetAllClaimsCount());
        switch (this.MoveType)
        {
          case MoveType.PlayMove:
            Root.SetAttribute("IsTrueClaim", (object) this.IsTrueClaim);
            (Root.ChildNodes[0] as XmlElement).SetAttribute("Type", "Real");
            (Root.ChildNodes[1] as XmlElement).SetAttribute("Type", "Claim");
            break;
          case MoveType.TakeCard:
            if (this.DerivedItemsList.Count == 0)
              break;
            Root.SetAttribute("ReceivedCard", (object) (this.GetNoneZeroCard(this.DerivedItemsList[0]).Item1 + 1));
            break;
          case MoveType.CallCheat:
            Root.SetAttribute("CheatCallCorrect", (object) !this.PreviousTurn.IsTrueClaim);
            break;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed in SaveSummary(). Error: " + ex.Message);
      }
    }
  }
}
