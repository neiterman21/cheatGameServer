// Decompiled with JetBrains decompiler
// Type: CheatGame.HumanPlayer
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

namespace CheatGame
{
  internal class HumanPlayer : Player
  {
    public HumanPlayer()
    {
    }

    public HumanPlayer(CardsStruct dealtCards)
      : base(dealtCards)
    {
    }

    public override void decideCallCheat()
    {
      this._callCheat = true;
    }

    public void decideMove(bool isTakeCard, CardsStruct realMove, CardsStruct claimMove)
    {
      if (isTakeCard)
      {
        this._takeCard = true;
        this._playMove = false;
      }
      else
      {
        this._takeCard = false;
        this._playMove = true;
        this.realMove = realMove;
        this.claimMove = claimMove;
      }
    }
  }
}
