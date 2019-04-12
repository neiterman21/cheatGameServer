// Decompiled with JetBrains decompiler
// Type: CheatGame.BoardState
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System.Collections.ObjectModel;

namespace CheatGame
{
  public class BoardState
  {
    public string ComputerMsg { get; set; }

    public string PlayerMsg { get; set; }

    public string BoardMsg { get; set; }

    public int AgentCardsNum { get; set; }

    public int PlayedCardsNum { get; set; }

    public int BoardCardsNum { get; set; }

    public bool AgentStartPressed { get; set; }

    public bool IsServerTurn { get; set; }

    public bool TakeCardEnable { get; set; }

    public bool CallCheatEnable { get; set; }

    public string LastClaimType { get; set; }

    public string LastClaimNum { get; set; }

    public string LastClaimType2 { get; set; }

    public string LastClaimPlayerName { get; set; }

    public bool IsRevealing { get; set; }

    public bool CanDispute { get; set; }

    public string UsedCardsNumbers { get; set; }

    public int Ace { get; set; }

    public int Two { get; set; }

    public int Three { get; set; }

    public int Four { get; set; }

    public int Five { get; set; }

    public int Six { get; set; }

    public int Seven { get; set; }

    public int Eight { get; set; }

    public int Nine { get; set; }

    public int Ten { get; set; }

    public int Jack { get; set; }

    public int Queen { get; set; }

    public int King { get; set; }

    public ObservableCollection<CardsStruct.DataObject> GetCards()
    {
      ObservableCollection<CardsStruct.DataObject> observableCollection = new ObservableCollection<CardsStruct.DataObject>();
      CardsStruct.DataObject dataObject = new CardsStruct.DataObject();
      observableCollection.Add(dataObject);
      observableCollection[0].Ace = this.Ace;
      observableCollection[0].Two = this.Two;
      observableCollection[0].Three = this.Three;
      observableCollection[0].Four = this.Four;
      observableCollection[0].Five = this.Five;
      observableCollection[0].Six = this.Six;
      observableCollection[0].Seven = this.Seven;
      observableCollection[0].Eight = this.Eight;
      observableCollection[0].Nine = this.Nine;
      observableCollection[0].Ten = this.Ten;
      observableCollection[0].Jack = this.Jack;
      observableCollection[0].Queen = this.Queen;
      observableCollection[0].King = this.King;
      return observableCollection;
    }

    public void SetCards(ObservableCollection<CardsStruct.DataObject> PlayerCards)
    {
      this.Ace = PlayerCards[0].Ace;
      this.Two = PlayerCards[0].Two;
      this.Three = PlayerCards[0].Three;
      this.Four = PlayerCards[0].Four;
      this.Five = PlayerCards[0].Five;
      this.Six = PlayerCards[0].Six;
      this.Seven = PlayerCards[0].Seven;
      this.Eight = PlayerCards[0].Eight;
      this.Nine = PlayerCards[0].Nine;
      this.Ten = PlayerCards[0].Ten;
      this.Jack = PlayerCards[0].Jack;
      this.Queen = PlayerCards[0].Queen;
      this.King = PlayerCards[0].King;
    }

    public BoardState()
    {
      this.ComputerMsg = "New Game";
      this.PlayerMsg = "New Game";
      this.BoardMsg = "New Game";
      this.BoardCardsNum = 52;
      this.IsServerTurn = false;
    }
  }
}
