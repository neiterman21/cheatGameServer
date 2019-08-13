// Decompiled with JetBrains decompiler
// Type: CheatGame.CheatEngine
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Network;
using CentipedeModel.Network.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CheatGame
{
  public class CheatEngine
  {
    private CardsStruct realMove = new CardsStruct();
    private CardsStruct claimMove = new CardsStruct();
    private string _lastClaimMsg = "";
    private string _computerMsg = "";
    private string _playerMsg = "";
    private string _boardMsg = "";
    private bool _isGameOver = false;
    private bool _isHumanTurn = false;
    public Archive GamesArchive = new Archive("Liar");
    private bool firstGame = true;
    public bool[] PlayerStartPressed = new bool[Program.NUM_PLAYERS];
    private string[] _opponentFolders = new string[2];
    public Player[] Players = new Player[Program.NUM_PLAYERS];
    private string[] _fullPathPlayersFolders = new string[2];
    private Board _board;
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    private Player _nonCurrentPlayer;
    private string _lastClaimPlayerName;
    private Game _currGame;
    private Session _currSession;
    private Turn _currTurn;
    private int _numGames;
    private int _lastWinnerIndex;
    private int _numStartPressed;
    private MoveType _oppLastMoveType;

    public Player Player1
    {
      get
      {
        return this._player1;
      }
      set
      {
        this._player1 = value;
      }
    }

    public Player Player2
    {
      get
      {
        return this._player2;
      }
      set
      {
        this._player2 = value;
      }
    }

    public Player CurrentPlayer
    {
      get
      {
        return this._currentPlayer;
      }
      set
      {
        if (value == this._currentPlayer)
          return;
        this._currentPlayer = value;
      }
    }

    public Player NonCurrentPlayer
    {
      get
      {
        return this._nonCurrentPlayer;
      }
      set
      {
        if (value == this._nonCurrentPlayer)
          return;
        this._nonCurrentPlayer = value;
      }
    }

    public string LastClaimPlayerName
    {
      get
      {
        return this._lastClaimPlayerName;
      }
      set
      {
        if (!(value != this._lastClaimPlayerName))
          return;
        this._lastClaimPlayerName = value;
      }
    }

    public string ComputerMsg
    {
      get
      {
        return this._computerMsg;
      }
      set
      {
        if (!(value != this._computerMsg))
          return;
        this._computerMsg = value;
      }
    }

    public string LastClaimMsg
    {
      get
      {
        int cardsNum = this._board.LastClaim.CardsNum;
        string lastClaimType = this._board.LastClaimType;
        this._lastClaimMsg = "";
        return this._lastClaimMsg;
      }
    }

    public string PlayerMsg
    {
      get
      {
        return this._playerMsg;
      }
      set
      {
        if (!(value != this._playerMsg))
          return;
        this._playerMsg = value;
      }
    }

    public string BoardMsg
    {
      get
      {
        return this._boardMsg;
      }
      set
      {
        if (!(value != this._boardMsg))
          return;
        this._boardMsg = value;
      }
    }

    public bool IsGameOver
    {
      get
      {
        return this._isGameOver;
      }
      set
      {
        if (value == this._isGameOver)
          return;
        this._isGameOver = value;
      }
    }

    public bool IsHumanTurn
    {
      get
      {
        return this._isHumanTurn;
      }
      set
      {
        if (value == this._isHumanTurn)
          return;
        this._isHumanTurn = value;
      }
    }

    public bool TakeCardEnable
    {
      get
      {
        return this.IsHumanTurn && this._board.getCardsNum() != 0;
      }
    }

    public bool CallCheatEnable
    {
      get
      {
        return this.IsHumanTurn && this._oppLastMoveType == MoveType.PlayMove;
      }
    }

    public int BoardCardsNum
    {
      get
      {
        return this._board.getCardsNum();
      }
    }

    public int AgentCardsNum
    {
      get
      {
        return this.Player1.Cards.CardsNum;
      }
    }

    public int PlayedCardsNum
    {
      get
      {
        return this._board.PlayedCardsNum;
      }
    }

    public string LastClaimNum
    {
      get
      {
        if (this._board.LastClaimNum != -1)
          return this._board.LastClaimNum.ToString();
        return "";
      }
    }

    public string LastClaimType
    {
      get
      {
        return this._board.LastClaimType;
      }
    }

    public string LastClaimType2
    {
      get
      {
        return this._board.LastClaimType2;
      }
    }

    public CheatEngine(int numGames)
    {
      this._numGames = numGames;
      this._numStartPressed = 0;
      this.IsGameOver = true;
      this.PlayerMsg = "";
    }

    public void sendRecordingToOpponets(AudioMessage message, int playerId)
    {
          Program._tcpConnections[playerId].Send(message);
    }
    

    public void GameStep()
    {
      if (this.CurrentPlayer.CheatyOpponent) 
      {
        this.NonCurrentPlayer.addCards(this._board.emptyPlayedStack());
        this.BoardMsg = this.CurrentPlayer.PlayerName + " declared " + this.NonCurrentPlayer.PlayerName + " vocal statment was missing or not coresponding to your cardes claim." + 
        this.NonCurrentPlayer.PlayerName + " takes the game stack. This report will be checked manualy ";
        this.SendBoardToOpponents();
        this.swapPlayerTurn();
      }
      if (this.CurrentPlayer.CallCheat)
      {
        if (this.isCheat())
        {
          this.NonCurrentPlayer.addCards(this._board.emptyPlayedStack());
          this.BoardMsg = this.CurrentPlayer.PlayerName + " declared cheat and was correct. " + this.NonCurrentPlayer.PlayerName + " takes the game stack";
        }
        else
        {
          this.CurrentPlayer.addCards(this._board.emptyPlayedStack());
          this.BoardMsg = this.CurrentPlayer.PlayerName + " declared cheat and was incorrect. " + this.CurrentPlayer.PlayerName + " takes the game stack";
        }
        this.SendBoardToOpponents();
      }
      if (this.tryEndGame(this.CurrentPlayer.Forfeited))
        return;
      if (this.CurrentPlayer.TakeCard)
      {
        this.CurrentPlayer.TakeCard = false;
        CardsStruct cardsStruct = this._board.chooseRandomCards(3);
        this._board.removeCards(cardsStruct);
        this.CurrentPlayer.addCards(cardsStruct);
        this.BoardMsg = this.CurrentPlayer.PlayerName + " has taken 3 cards from the unused stack. ";
        string commaDelimitedString = cardsStruct.ToCommaDelimitedString();
        this.PlayerMsg = commaDelimitedString.Substring(0, commaDelimitedString.Length - 1);
        this.SendBoardToOpponents();
        this._currTurn.DerivedItemsList.Add((CardsStruct) cardsStruct.Clone());
        this.swapPlayerTurn();
      }
      else if (this.CurrentPlayer.PlayMove)
      {
        this._board.addCards(this.CurrentPlayer.realMove);
        this._board.LastMove = (CardsStruct) this.CurrentPlayer.realMove.Clone();
        this._board.LastClaim = (CardsStruct) this.CurrentPlayer.claimMove.Clone();
        this.CurrentPlayer.removeCards(this.CurrentPlayer.realMove);
        this.LastClaimPlayerName = this.CurrentPlayer.PlayerName;
        this.BoardMsg = this.CurrentPlayer.PlayerName + " made a claim.";
        this.SendBoardToOpponents();
        this.CurrentPlayer.PlayMove = false;
        this.swapPlayerTurn();
      }
      this.realMove.reset();
      this.claimMove.reset();
    }

    private string GetCardsToReveal()
    {
      IEnumerable<Turn> source = this._currSession.DerivedItemsList.Reverse<Turn>().Where<Turn>((Func<Turn, bool>) (turn => turn.MoveType == MoveType.PlayMove || turn.MoveType == MoveType.StartPressed));
      if (source.Count<Turn>() == 0)
        return string.Empty;
      string str = "";
      foreach (Turn turn in source)
      {
        if (turn.DerivedItemsList.Count != 0)
        {
          CardsStruct cardsStruct = turn.DerivedItemsList.First<CardsStruct>();
          str += cardsStruct.ToCommaDelimitedString();
        }
      }
      return str.Substring(0, str.Length - 1);
    }

    public void OnEndReveal()
    {
      this._currSession.SignalEnd();
      this.CurrentPlayer.CallCheat = false;
      this._currSession = new Session(this._currGame);
      this.LastClaimPlayerName = "";
      this.BoardMsg = "";
      this.SendBoardToOpponents();
      this.swapPlayerTurn();
    }

    private bool tryEndGame(bool forfieted = false)
    {
      if (this.NonCurrentPlayer.Cards.CardsNum > 0 && !forfieted)
        return false;
      for (int index = 0; index < Program.NUM_PLAYERS; ++index)
        this.PlayerStartPressed[index] = false;
      this.BoardMsg = this.NonCurrentPlayer.PlayerName + " has won. Game Ended.";
      this.SendBoardToOpponents();
      this._numStartPressed = 0;
      this._lastWinnerIndex = this.NonCurrentPlayer == this._player1 ? 0 : 1;
      if (!this.realMove.Equals((object) CardsStruct.EmptyStruct))
        this._currTurn.DerivedItemsList.Add((CardsStruct) this.realMove.Clone());
      if (!this.claimMove.Equals((object) CardsStruct.EmptyStruct))
        this._currTurn.DerivedItemsList.Add((CardsStruct) this.claimMove.Clone());
      this._currTurn.SignalEnd();
      this._currSession.SignalEnd();
      this._currGame.PlayerWonId = this._lastWinnerIndex;
      this._currGame.SignalEnd();
      this.SaveTurn();
      return true;
    }

    private bool isCheat()
    {
      return this._board.LastClaim != null && !this._board.LastMove.Equals((object) this._board.LastClaim);
    }

    public CheatEngine.TryStartGameReturnCodes TryStartGame()
    {
      if (++this._numStartPressed < Program.NUM_PLAYERS)
        return CheatEngine.TryStartGameReturnCodes.AWAITING_PLAYERS;
      this._numStartPressed = 0;
      this._board = new Board();
      if (this.GamesArchive.DerivedItemsList.Count<Game>() == this._numGames)
        return CheatEngine.TryStartGameReturnCodes.MAX_GAMES_REACHED;
      if (this.firstGame)
      {
        this.firstGame = false;
        this.CurrentPlayer = this.IsHumanTurn ? this._player2 : this._player1;
      }
      else
      {
        for (int index = 0; index < Program.NUM_PLAYERS; ++index)
          this.Players[index].Reset();
        this.CurrentPlayer = this.Players[this._lastWinnerIndex];
        this.IsHumanTurn = this.CurrentPlayer == this._player2;
        this._currGame = new Game(this.GamesArchive);
        this._currSession = new Session(this._currGame);
        this._currTurn = new Turn(this._currSession);
      }
      this.NonCurrentPlayer = this.CurrentPlayer == this._player1 ? this._player2 : this._player1;
      this.LastClaimPlayerName = "";
      this._currTurn.MoveType = MoveType.StartPressed;
      this._currTurn.MoveTime = TimeStamper.Time;
      this._currTurn.PlayerName = "Board";
      this._currTurn.PlayerIndex = -1;
      this.DealCards();
      this._currTurn.SignalEnd();
      this.BoardMsg = "";
      this.SendBoardToOpponents();
      this.swapPlayerTurn();
      return CheatEngine.TryStartGameReturnCodes.GAME_STARTED;
    }

    public void SendBoardToOpponents()
    {
      for (int index = 0; index < Program.NUM_PLAYERS; ++index)
      {
        BoardState state = new BoardState();
        Player player = index == 0 ? this.Players[1] : this.Players[0];
        bool flag = this.CurrentPlayer == this.Players[index];
        state.AgentCardsNum = player.Cards.CardsNum;
        state.BoardCardsNum = this._board.getCardsNum();
        state.BoardMsg = this._boardMsg;
        state.ComputerMsg = flag ? "Turn Ended" : "Playing Move";
        state.SetCards(this.Players[index].Cards.Model);
        state.IsServerTurn = flag || this.CurrentPlayer.CallCheat;
        state.TakeCardEnable = !flag && this._board.getCardsNum() != 0 && !this.CurrentPlayer.CallCheat;
        if (this.CurrentPlayer.CheatyOpponent)
        {
           state.IsServerTurn = flag;
           state.TakeCardEnable = !flag;
        }
        state.CanDispute = this.CurrentPlayer.CheatyOpponent && !flag;
        state.CallCheatEnable = !flag && this.CurrentPlayer.PlayMove;
        state.LastClaimNum = this._board.LastClaimNum != -1 ? this._board.LastClaimNum.ToString() : "";
        state.LastClaimPlayerName = this._lastClaimPlayerName;
        state.LastClaimType = this._board.LastClaimType;
        state.LastClaimType2 = this._board.LastClaimType2;
        state.PlayedCardsNum = this._board.PlayedCardsNum;
        state.PlayerMsg = this.PlayerMsg;
        state.AgentStartPressed = this.PlayerStartPressed[index];
        state.IsRevealing = this.CurrentPlayer.CallCheat;
        state.UsedCardsNumbers = this.CurrentPlayer.CallCheat ? this.GetCardsToReveal() : "";
        Program._tcpConnections[index].Send((Message) new BoardMessage(state));
      }
      this.CurrentPlayer.CheatyOpponent = false;
    }

    public void SendEmptyBoardToOpponent(int playerId, bool IsStartPressed)
    {
      Program._tcpConnections[playerId].Send((Message) new BoardMessage(new BoardState()
      {
        AgentCardsNum = 0,
        BoardCardsNum = 52,
        BoardMsg = "",
        ComputerMsg = "",
        IsServerTurn = true,
        TakeCardEnable = false,
        CallCheatEnable = false,
        LastClaimNum = "",
        LastClaimPlayerName = "",
        LastClaimType = "",
        LastClaimType2 = "",
        PlayedCardsNum = 0,
        PlayerMsg = "",
        AgentStartPressed = IsStartPressed,
        IsRevealing = false,
        CanDispute =false,
        UsedCardsNumbers = ""
      }));
    }

    public void GameStepOnReceivedOpponentMove(Move oppMove, int playerId)
    {
      this.realMove.Cards = oppMove.GetRealMoveCards()[0];
      this.claimMove.Cards = oppMove.GetClaimMoveCards()[0];
      this._oppLastMoveType = oppMove.MoveType;
      this._currTurn.MoveType = oppMove.MoveType;
      this._currTurn.MoveTime = TimeStamper.Time;
      this._currTurn.PlayerName = this.Players[playerId].PlayerName;
      this._currTurn.Player = this.Players[playerId];
      this._currTurn.PlayerIndex = playerId;
      switch (oppMove.MoveType)
      {
        case MoveType.PlayMove:
          ((HumanPlayer) this.CurrentPlayer).decideMove(false, this.realMove, this.claimMove);
          break;
        case MoveType.TakeCard:
          ((HumanPlayer) this.CurrentPlayer).decideMove(true, (CardsStruct) null, (CardsStruct) null);
          break;
        case MoveType.CallCheat:
          this.CurrentPlayer.decideCallCheat();
          break;
        case MoveType.ForfeitGame:
          this.CurrentPlayer.Forfeited = true;
          break;
        case MoveType.CallCheatyOpponent:
          this.CurrentPlayer.CheatyOpponent = true;
          break;
        }
      this.GameStep();
    }

    private void DealCards()
    {
      CardsStruct cardsStruct1 = this._board.chooseRandomCards(8);
      this._board.removeCards(cardsStruct1);
      this.Players[1].addCards(cardsStruct1);
      CardsStruct cardsStruct2 = this._board.chooseRandomCards(8);
      this._board.removeCards(cardsStruct2);
      this.Players[0].addCards(cardsStruct2);
      this._currTurn.DerivedItemsList.Add(this._board.setBeginCard());
    }

    public void InitPlayers(string[] Names)
    {
      HumanPlayer humanPlayer1 = new HumanPlayer();
      humanPlayer1.PlayerName = Names[0];
      this._player1 = (Player) humanPlayer1;
      HumanPlayer humanPlayer2 = new HumanPlayer();
      humanPlayer2.PlayerName = Names[1];
      this._player2 = (Player) humanPlayer2;
      this.Players[0] = this._player1;
      this.Players[1] = this._player2;
      this.IsHumanTurn = new Random().Next(1, Program.NUM_PLAYERS + 1) == 2;
      string[] strArray1 = new string[9]
      {
        DateTime.Now.Year.ToString(),
        "-",
        DateTime.Now.Month.ToString(),
        "-",
        DateTime.Now.Day.ToString(),
        "-",
        null,
        null,
        null
      };
      string[] strArray2 = strArray1;
      int index1 = 6;
      DateTime now = DateTime.Now;
      string str1 = now.Hour.ToString();
      strArray2[index1] = str1;
      strArray1[7] = "-";
      string[] strArray3 = strArray1;
      int index2 = 8;
      now = DateTime.Now;
      string str2 = now.Minute.ToString();
      strArray3[index2] = str2;
      string str3 = string.Concat(strArray1);
      for (int index3 = 0; index3 < Program.NUM_PLAYERS; ++index3)
      {
        this._opponentFolders[index3] = string.Format("\\{0}", (object) Names[index3]);
        this._fullPathPlayersFolders[index3] = Program.RootDir + this._opponentFolders[index3];
        int i = 0;
        while (Directory.Exists(Program.RootDir + this._opponentFolders[index3]))
         {
            if (Regex.IsMatch(_opponentFolders[index3].Substring(_opponentFolders[index3].Length - 1 ,1), @"\d")) { //check if last char is a digit
                        this._opponentFolders[index3] = _opponentFolders[index3].Substring(_opponentFolders[index3].Length - 1, 1);
                                    }
            this._opponentFolders[index3] = this._opponentFolders[index3] + i.ToString();
            i++;
            this._fullPathPlayersFolders[index3] = Program.RootDir + this._opponentFolders[index3];
         }
         Directory.CreateDirectory(this._fullPathPlayersFolders[index3]);
      }
      this._currGame = new Game(this.GamesArchive);
      this._currSession = new Session(this._currGame);
      this._currTurn = new Turn(this._currSession);
      this.SetFolders();
    }

    private void SetFolders()
    {
      int num = this._currGame.DerivedItemsList.Count<Session>();
      string str = num != 1 ? string.Format("\\Game{0:00}\\Session{1:00}\\Turn{2:000}", (object) this.GamesArchive.DerivedItemsList.Count<Game>(), (object) num, (object) this._currSession.DerivedItemsList.Count<Turn>()) : string.Format("\\Game{0:00}\\Session{1:00}\\Turn{2:000}", (object) this.GamesArchive.DerivedItemsList.Count<Game>(), (object) num, (object) (this._currSession.DerivedItemsList.Count<Turn>() - 1));
      for (int index = 0; index < Program.NUM_PLAYERS; ++index)
      {
        this._fullPathPlayersFolders[index] = Program.RootDir + this._opponentFolders[index] + str;
        Directory.CreateDirectory(this._fullPathPlayersFolders[index]);
      }
      Program.ResetImageIndexes();
    }

    private void SaveTurn()
    {
            for (int index = 0; index < Program.NUM_PLAYERS; ++index)
            {
                this.GamesArchive.Save(this._fullPathPlayersFolders[index] + "\\Summary.xml");
            }
    }

    public string GetCurrentFolder(int playerId)
    {
      if (this._fullPathPlayersFolders[playerId] != null && this._opponentFolders[playerId] != null)
        return this._fullPathPlayersFolders[playerId];
      return (string) null;
    }

    public void swapPlayerTurn()
    {
      if (this.CurrentPlayer == this._player1)
      {
        this.IsHumanTurn = true;
        this.CurrentPlayer = this._player2;
        this.NonCurrentPlayer = this._player1;
        this.PlayerMsg = "";
        this.ComputerMsg = "Turn Ended";
        this.BoardMsg = "";
      }
      else
      {
        this.IsHumanTurn = false;
        this.CurrentPlayer = this._player1;
        this.NonCurrentPlayer = this._player2;
        this.PlayerMsg = "";
        this.ComputerMsg = "Playing Move";
        this.BoardMsg = "";
      }
      if (!this.realMove.Equals((object) CardsStruct.EmptyStruct))
        this._currTurn.DerivedItemsList.Add((CardsStruct) this.realMove.Clone());
      if (!this.claimMove.Equals((object) CardsStruct.EmptyStruct))
        this._currTurn.DerivedItemsList.Add((CardsStruct) this.claimMove.Clone());
      this._currTurn.SignalEnd();
      this.SaveTurn();
      this._currTurn = new Turn(this._currSession);
      this.SetFolders();
    }

    public void OnCloseApp()
    {
      if (this._currTurn != null)
        this._currTurn.SignalEnd();
      if (this._currSession != null)
        this._currSession.SignalEnd();
      if (this._currGame != null)
        this._currGame.SignalEnd();
      if (this.GamesArchive == null)
        return;
      this.GamesArchive.SignalEnd();
      this.GamesArchive.Save(Program.RootDir + this._opponentFolders[0] + "\\Summary.Final.xml");
      this.GamesArchive.Save(Program.RootDir + this._opponentFolders[1] + "\\Summary.Final.xml");
    }

    public enum TryStartGameReturnCodes
    {
      AWAITING_PLAYERS,
      GAME_STARTED,
      MAX_GAMES_REACHED,
    }
  }
}
