// Decompiled with JetBrains decompiler
// Type: CheatGame.Player
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;

namespace CheatGame
{
  public abstract class Player : CardHolderInterface
  {
    protected bool _takeCard = false;
    protected bool _playMove = false;
    protected bool _callCheat = false;
    protected bool _forfeited = false;
    protected bool _cheatyopponent = false;

    public string PlayerName { get; set; }

    public abstract void decideCallCheat();

    public CardsStruct realMove { get; set; }

    public CardsStruct claimMove { get; set; }

    public Player(CardsStruct dealtCards)
    {
      this._cards = new CardsStruct(dealtCards);
    }

    public Player()
    {
      this._cards = new CardsStruct();
    }

    public void Reset()
    {
      this.Cards.reset();
      this.CallCheat = false;
      this.PlayMove = false;
      this.TakeCard = false;
      this._forfeited = false;
      this.realMove = CardsStruct.EmptyStruct;
      this.claimMove = CardsStruct.EmptyStruct;
    }

    public CardsStruct Cards
    {
      get
      {
        return this._cards;
      }
      set
      {
        this._cards = value;
      }
    }

    public void PrintCards()
    {
      Console.WriteLine(this._cards.ToString());
    }

    public override void addCards(CardsStruct add)
    {
      this._cards.add(add);
    }

    public override void removeCards(CardsStruct sub)
    {
      this._cards.remove(sub);
    }

    public bool TakeCard
    {
      get
      {
        return this._takeCard;
      }
      set
      {
        this._takeCard = value;
      }
    }

    public bool PlayMove
    {
      get
      {
        return this._playMove;
      }
      set
      {
        this._playMove = value;
      }
    }

    public bool Forfeited
        {
        get
        {
            return this._forfeited;
        }
        set
        {
            this._forfeited = value;
        }
    }

    public bool CheatyOpponent
    {
        get
        {
            return this._cheatyopponent;
        }
        set
        {
            this._cheatyopponent = value;
        }
    }

        public bool CallCheat
    {
      get
      {
        return this._callCheat;
      }
      set
      {
        this._callCheat = value;
      }
    }

    public int getClaimTypesNum()
    {
      int num = 0;
      for (int index = 0; index < 13; ++index)
      {
        if (this.claimMove[index] > 0)
          ++num;
      }
      return num;
    }

    public int getClaimIndex()
    {
      int num = 0;
      for (int index = 0; index < 13; ++index)
      {
        if (this.claimMove[index] > 0)
          num = index;
      }
      return num;
    }
  }
}
