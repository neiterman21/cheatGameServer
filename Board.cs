// Decompiled with JetBrains decompiler
// Type: CheatGame.Board
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

namespace CheatGame
{
  internal class Board : CardHolderInterface
  {
    private CardsStruct _lastClaim = (CardsStruct) null;
    private CardsStruct _playedCards;
    private CardsStruct _lastMove;
    private string _lastClaimType;

    public CardsStruct LastMove
    {
      get
      {
        return this._lastMove;
      }
      set
      {
        this._lastMove = value;
      }
    }

    public CardsStruct LastClaim
    {
      get
      {
        return this._lastClaim;
      }
      set
      {
        this._lastClaim = value;
      }
    }

    public int LastClaimIndex
    {
      get
      {
        if (this._lastClaim != null)
        {
          for (int index = 0; index < 13; ++index)
          {
            if (this._lastClaim[index] > 0)
              return index;
          }
          return -1;
        }
        if (this.LastClaimType == "Ace")
          return 0;
        if (this.LastClaimType == "Two")
          return 1;
        if (this.LastClaimType == "Three")
          return 2;
        if (this.LastClaimType == "Four")
          return 3;
        if (this.LastClaimType == "Five")
          return 4;
        if (this.LastClaimType == "Six")
          return 5;
        if (this.LastClaimType == "Seven")
          return 6;
        if (this.LastClaimType == "Eight")
          return 7;
        if (this.LastClaimType == "Nine")
          return 8;
        if (this.LastClaimType == "Ten")
          return 9;
        if (this.LastClaimType == "Jack")
          return 10;
        if (this.LastClaimType == "Queen")
          return 11;
        return this.LastClaimType == "King" ? 12 : -1;
      }
    }

    public string LastClaimType
    {
      get
      {
        if (this._lastClaim != null)
          return this._lastClaim.getType();
        return this._lastClaimType;
      }
      set
      {
        this._lastClaimType = value;
      }
    }

    public string LastClaimType2
    {
      get
      {
        if (this._lastClaim != null)
          return this._lastClaim.getType();
        return "None";
      }
    }

    public int PlayedCardsNum
    {
      get
      {
        return this._playedCards.CardsNum;
      }
    }

    public int LastClaimNum
    {
      get
      {
        if (this._lastClaim != null)
        {
          for (int index = 0; index < 13; ++index)
          {
            if (this._lastClaim[index] > 0)
              return this._lastClaim[index];
          }
        }
        return -1;
      }
    }

    public Board()
    {
      this._cards = new CardsStruct(new CardsStruct(4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4));
      this._playedCards = new CardsStruct();
    }

    public override void addCards(CardsStruct add)
    {
      this._playedCards.add(add);
    }

    public override void removeCards(CardsStruct sub)
    {
      this._cards.remove(sub);
    }

    public CardsStruct emptyPlayedStack()
    {
      CardsStruct playedCards = this._playedCards;
      this._playedCards = new CardsStruct();
      return playedCards;
    }

    public CardsStruct setBeginCard()
    {
      CardsStruct cardsStruct = this.chooseRandomCards(1);
      this.removeCards(cardsStruct);
      this.addCards(cardsStruct);
      this.LastClaimType = cardsStruct.getType();
      this._lastClaim = (CardsStruct) null;
      return cardsStruct;
    }
  }
}
