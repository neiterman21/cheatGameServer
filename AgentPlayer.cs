// Decompiled with JetBrains decompiler
// Type: CheatGame.AgentPlayer
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;

namespace CheatGame
{
  internal class AgentPlayer : Player
  {
    public AgentPlayer()
    {
    }

    public AgentPlayer(CardsStruct dealtCards)
      : base(dealtCards)
    {
    }

    public override void decideCallCheat()
    {
      if (this.RandomNumber(0, 10) > 2)
        this._callCheat = false;
      else
        this._callCheat = true;
    }

    public void decideMove(Board board)
    {
      if (this.RandomNumber(0, 10) > 2)
        this.regularMove(board);
      else
        this.takeCard();
    }

    private void takeCard()
    {
      this._takeCard = true;
      this._playMove = false;
    }

    private void regularMove(Board board)
    {
      this._takeCard = false;
      this._timeEnded = false;
      this._playMove = true;
      this.realMove = this.Cards.getRandomCards(this.RandomNumber(1, 5));
      int cardsNum = this.realMove.CardsNum;
      this.claimMove = new CardsStruct();
      this.claimMove[board.LastClaimIndex] = cardsNum;
    }

    private int RandomNumber(int min, int max)
    {
      return new Random().Next(min, max);
    }
  }
}
