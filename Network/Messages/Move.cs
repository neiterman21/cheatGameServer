// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.Move
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CheatGame;
using System;
using System.Collections.ObjectModel;
using NAudio.Wave;

namespace CentipedeModel.Network.Messages
{
  public class Move
  {
    public TimeSpan MoveTime { get; set; }

    public MoveType MoveType { get; set; }

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

    public int AceC { get; set; }

    public int TwoC { get; set; }

    public int ThreeC { get; set; }

    public int FourC { get; set; }

    public int FiveC { get; set; }

    public int SixC { get; set; }

    public int SevenC { get; set; }

    public int EightC { get; set; }

    public int NineC { get; set; }

    public int TenC { get; set; }

    public int JackC { get; set; }

    public int QueenC { get; set; }

    public int KingC { get; set; }

    public ObservableCollection<CardsStruct.DataObject> GetRealMoveCards()
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

    public void SetRealMoveCards(ObservableCollection<CardsStruct.DataObject> PlayerCards)
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

    public ObservableCollection<CardsStruct.DataObject> GetClaimMoveCards()
    {
      ObservableCollection<CardsStruct.DataObject> observableCollection = new ObservableCollection<CardsStruct.DataObject>();
      CardsStruct.DataObject dataObject = new CardsStruct.DataObject();
      observableCollection.Add(dataObject);
      observableCollection[0].Ace = this.AceC;
      observableCollection[0].Two = this.TwoC;
      observableCollection[0].Three = this.ThreeC;
      observableCollection[0].Four = this.FourC;
      observableCollection[0].Five = this.FiveC;
      observableCollection[0].Six = this.SixC;
      observableCollection[0].Seven = this.SevenC;
      observableCollection[0].Eight = this.EightC;
      observableCollection[0].Nine = this.NineC;
      observableCollection[0].Ten = this.TenC;
      observableCollection[0].Jack = this.JackC;
      observableCollection[0].Queen = this.QueenC;
      observableCollection[0].King = this.KingC;
      return observableCollection;
    }

    public void SetClaimMoveCards(ObservableCollection<CardsStruct.DataObject> PlayerCards)
    {
      this.AceC = PlayerCards[0].Ace;
      this.TwoC = PlayerCards[0].Two;
      this.ThreeC = PlayerCards[0].Three;
      this.FourC = PlayerCards[0].Four;
      this.FiveC = PlayerCards[0].Five;
      this.SixC = PlayerCards[0].Six;
      this.SevenC = PlayerCards[0].Seven;
      this.EightC = PlayerCards[0].Eight;
      this.NineC = PlayerCards[0].Nine;
      this.TenC = PlayerCards[0].Ten;
      this.JackC = PlayerCards[0].Jack;
      this.QueenC = PlayerCards[0].Queen;
      this.KingC = PlayerCards[0].King;
    }
  }
}
