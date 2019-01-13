// Decompiled with JetBrains decompiler
// Type: CheatGame.CardsStruct
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace CheatGame
{
  public class CardsStruct : IContainer, ICloneable, IComparable
  {
    private static CardsStruct _emptyStruct = new CardsStruct(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    private ObservableCollection<CardsStruct.DataObject> _cards = new ObservableCollection<CardsStruct.DataObject>();
    private Random random = new Random();

    public void SaveSummary(XmlElement parent)
    {
      parent.SetAttribute("CardSet", this.ToString());
    }

    public void SignalEnd()
    {
    }

    public CardsStruct.DataObject Cards
    {
      get
      {
        return this._cards[0];
      }
      set
      {
        this._cards[0] = value;
      }
    }

    public int CardsNum
    {
      get
      {
        return this.Cards.Sum();
      }
    }

    public CardsStruct()
    {
      this._cards.Add(new CardsStruct.DataObject());
    }

    public CardsStruct(int ace, int two, int three, int four, int five, int six, int seven, int eight, int nine, int ten, int jack, int queen, int king)
    {
      this._cards.Add(new CardsStruct.DataObject(ace, two, three, four, five, six, seven, eight, nine, ten, jack, queen, king));
    }

    public CardsStruct(CardsStruct dealtCards)
    {
      this._cards = dealtCards._cards;
    }

    public static CardsStruct EmptyStruct
    {
      get
      {
        return CardsStruct._emptyStruct;
      }
    }

    public int this[int index]
    {
      get
      {
        return this.Cards[index];
      }
      set
      {
        this.Cards[index] = value;
      }
    }

    public ObservableCollection<CardsStruct.DataObject> Model
    {
      get
      {
        return this._cards;
      }
    }

    public void reset()
    {
      this.Cards.reset();
    }

    public void add(CardsStruct add)
    {
      for (int index = 0; index < 13; ++index)
        this[index] += add[index];
    }

    public void remove(CardsStruct sub)
    {
      for (int index = 0; index < 13; ++index)
        this[index] -= sub[index];
    }

    public void transferRandom(CardsStruct to, int cardsNum)
    {
    }

    public CardsStruct getRandomCards(int cardsNum)
    {
      CardsStruct cardsStruct1 = (CardsStruct) this.Clone();
      CardsStruct cardsStruct2 = new CardsStruct();
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < 13; ++index)
      {
        if (cardsStruct1[index] > 0)
          arrayList.Add((object) index);
      }
      for (; cardsNum > 0 && arrayList.Count > 0; --cardsNum)
      {
        int index1 = this.chooseRandom(this.random, arrayList.Count);
        int index2 = (int) arrayList[index1];
        --cardsStruct1[index2];
        ++cardsStruct2[index2];
        if (cardsStruct1[index2] == 0)
          arrayList.RemoveAt(index1);
      }
      return cardsStruct2;
    }

    private int chooseRandom(Random random, int p)
    {
      return random.Next(0, p);
    }

    public object Clone()
    {
      return (object) new CardsStruct(this.Cards[0], this.Cards[1], this.Cards[2], this.Cards[3], this.Cards[4], this.Cards[5], this.Cards[6], this.Cards[7], this.Cards[8], this.Cards[9], this.Cards[10], this.Cards[11], this.Cards[12]);
    }

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < 13; ++index)
        str = str + (object) this.Cards[index] + " ";
      return str;
    }

    public override bool Equals(object obj)
    {
      return this.Cards.Equals((object) ((CardsStruct) obj).Cards);
    }

    public int CompareTo(object obj)
    {
      CardsStruct cardsStruct = (CardsStruct) obj;
      return this.Cards[0] <= cardsStruct.Cards[0] && this.Cards[1] <= cardsStruct.Cards[1] && (this.Cards[2] <= cardsStruct.Cards[2] && this.Cards[3] <= cardsStruct.Cards[3]) && (this.Cards[4] <= cardsStruct.Cards[4] && this.Cards[5] <= cardsStruct.Cards[5] && (this.Cards[6] <= cardsStruct.Cards[6] && this.Cards[7] <= cardsStruct.Cards[7])) && (this.Cards[8] <= cardsStruct.Cards[8] && this.Cards[9] <= cardsStruct.Cards[9] && (this.Cards[10] <= cardsStruct.Cards[10] && this.Cards[11] <= cardsStruct.Cards[11])) && this.Cards[12] <= cardsStruct.Cards[12] ? 1 : -1;
    }

    public string getType()
    {
      for (int index = 0; index < 13; ++index)
      {
        if (this.Cards[index] > 0)
        {
          switch (index)
          {
            case 0:
              return "Ace";
            case 1:
              return "Two";
            case 2:
              return "Three";
            case 3:
              return "Four";
            case 4:
              return "Five";
            case 5:
              return "Six";
            case 6:
              return "Seven";
            case 7:
              return "Eight";
            case 8:
              return "Nine";
            case 9:
              return "Ten";
            case 10:
              return "Jack";
            case 11:
              return "Queen";
            case 12:
              return "King";
            default:
              return "Ace";
          }
        }
      }
      return "None";
    }

    internal string ToCommaDelimitedString()
    {
      string str = "";
      for (int index1 = 0; index1 < 13; ++index1)
      {
        for (int index2 = 0; index2 < this[index1]; ++index2)
          str = str + (index1 + 1).ToString() + ",";
      }
      return str;
    }

    public class DataObject : INotifyPropertyChanged
    {
      private int _ace;
      private int _two;
      private int _three;
      private int _four;
      private int _five;
      private int _six;
      private int _seven;
      private int _eight;
      private int _nine;
      private int _ten;
      private int _jack;
      private int _queen;
      private int _king;

      public event PropertyChangedEventHandler PropertyChanged;

      protected void Notify(string propertyName)
      {
        if (this.PropertyChanged == null)
          return;
        this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
      }

      public int Ace
      {
        get
        {
          return this._ace;
        }
        set
        {
          if (value == this._ace)
            return;
          this._ace = value;
          this.Notify(nameof (Ace));
        }
      }

      public int Two
      {
        get
        {
          return this._two;
        }
        set
        {
          if (value == this._two)
            return;
          this._two = value;
          this.Notify(nameof (Two));
        }
      }

      public int Three
      {
        get
        {
          return this._three;
        }
        set
        {
          if (value == this._three)
            return;
          this._three = value;
          this.Notify(nameof (Three));
        }
      }

      public int Four
      {
        get
        {
          return this._four;
        }
        set
        {
          if (value == this._four)
            return;
          this._four = value;
          this.Notify(nameof (Four));
        }
      }

      public int Five
      {
        get
        {
          return this._five;
        }
        set
        {
          if (value == this._five)
            return;
          this._five = value;
          this.Notify(nameof (Five));
        }
      }

      public int Six
      {
        get
        {
          return this._six;
        }
        set
        {
          if (value == this._six)
            return;
          this._six = value;
          this.Notify(nameof (Six));
        }
      }

      public int Seven
      {
        get
        {
          return this._seven;
        }
        set
        {
          if (value == this._seven)
            return;
          this._seven = value;
          this.Notify(nameof (Seven));
        }
      }

      public int Eight
      {
        get
        {
          return this._eight;
        }
        set
        {
          if (value == this._eight)
            return;
          this._eight = value;
          this.Notify(nameof (Eight));
        }
      }

      public int Nine
      {
        get
        {
          return this._nine;
        }
        set
        {
          if (value == this._nine)
            return;
          this._nine = value;
          this.Notify(nameof (Nine));
        }
      }

      public int Ten
      {
        get
        {
          return this._ten;
        }
        set
        {
          if (value == this._ten)
            return;
          this._ten = value;
          this.Notify(nameof (Ten));
        }
      }

      public int Jack
      {
        get
        {
          return this._jack;
        }
        set
        {
          if (value == this._jack)
            return;
          this._jack = value;
          this.Notify(nameof (Jack));
        }
      }

      public int Queen
      {
        get
        {
          return this._queen;
        }
        set
        {
          if (value == this._queen)
            return;
          this._queen = value;
          this.Notify(nameof (Queen));
        }
      }

      public int King
      {
        get
        {
          return this._king;
        }
        set
        {
          if (value == this._king)
            return;
          this._king = value;
          this.Notify(nameof (King));
        }
      }

      public DataObject()
      {
        this.Ace = 0;
        this.Two = 0;
        this.Three = 0;
        this.Four = 0;
        this.Five = 0;
        this.Six = 0;
        this.Seven = 0;
        this.Eight = 0;
        this.Nine = 0;
        this.Ten = 0;
        this.Jack = 0;
        this.Queen = 0;
        this.King = 0;
      }

      public void reset()
      {
        this.Ace = 0;
        this.Two = 0;
        this.Three = 0;
        this.Four = 0;
        this.Five = 0;
        this.Six = 0;
        this.Seven = 0;
        this.Eight = 0;
        this.Nine = 0;
        this.Ten = 0;
        this.Jack = 0;
        this.Queen = 0;
        this.King = 0;
      }

      public int Sum()
      {
        return this.Ace + this.Two + this.Three + this.Four + this.Five + this.Six + this.Seven + this.Eight + this.Nine + this.Ten + this.Jack + this.Queen + this.King;
      }

      public DataObject(int ace, int two, int three, int four, int five, int six, int seven, int eight, int nine, int ten, int jack, int queen, int king)
      {
        this.Ace = ace;
        this.Two = two;
        this.Three = three;
        this.Four = four;
        this.Five = five;
        this.Six = six;
        this.Seven = seven;
        this.Eight = eight;
        this.Nine = nine;
        this.Ten = ten;
        this.Jack = jack;
        this.Queen = queen;
        this.King = king;
      }

      public int this[int index]
      {
        get
        {
          switch (index)
          {
            case 0:
              return this.Ace;
            case 1:
              return this.Two;
            case 2:
              return this.Three;
            case 3:
              return this.Four;
            case 4:
              return this.Five;
            case 5:
              return this.Six;
            case 6:
              return this.Seven;
            case 7:
              return this.Eight;
            case 8:
              return this.Nine;
            case 9:
              return this.Ten;
            case 10:
              return this.Jack;
            case 11:
              return this.Queen;
            case 12:
              return this.King;
            default:
              return this.Ace;
          }
        }
        set
        {
          switch (index)
          {
            case 0:
              this.Ace = value;
              break;
            case 1:
              this.Two = value;
              break;
            case 2:
              this.Three = value;
              break;
            case 3:
              this.Four = value;
              break;
            case 4:
              this.Five = value;
              break;
            case 5:
              this.Six = value;
              break;
            case 6:
              this.Seven = value;
              break;
            case 7:
              this.Eight = value;
              break;
            case 8:
              this.Nine = value;
              break;
            case 9:
              this.Ten = value;
              break;
            case 10:
              this.Jack = value;
              break;
            case 11:
              this.Queen = value;
              break;
            case 12:
              this.King = value;
              break;
            default:
              this.Ace = value;
              break;
          }
        }
      }

      public override bool Equals(object obj)
      {
        CardsStruct.DataObject dataObject = (CardsStruct.DataObject) obj;
        return this.Ace == dataObject.Ace && this.Two == dataObject.Two && (this.Three == dataObject.Three && this.Four == dataObject.Four) && (this.Five == dataObject.Five && this.Six == dataObject.Six && (this.Seven == dataObject.Seven && this.Eight == dataObject.Eight)) && (this.Nine == dataObject.Nine && this.Ten == dataObject.Ten && (this.Jack == dataObject.Jack && this.Queen == dataObject.Queen)) && this.King == dataObject.King;
      }
    }
  }
}
