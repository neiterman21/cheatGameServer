// Decompiled with JetBrains decompiler
// Type: CheatGame.CardHolderInterface
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

namespace CheatGame
{
  public abstract class CardHolderInterface
  {
    protected CardsStruct _cards;

    public abstract void addCards(CardsStruct add);

    public abstract void removeCards(CardsStruct sub);

    public int getCardsNum()
    {
      return this._cards.CardsNum;
    }

    public CardsStruct chooseRandomCards(int cardsNum)
    {
      return this._cards.getRandomCards(cardsNum);
    }
  }
}
