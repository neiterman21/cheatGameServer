// Decompiled with JetBrains decompiler
// Type: CheatGame.Program
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel;
using CentipedeModel.Network;
using CentipedeModel.Network.Messages;
using CentipedeModel.Players;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Windows.Forms;
using CentipedeModel.Network.Messages;

namespace CheatGame
{
  public class Program 
    {
    public static int NUM_PLAYERS = 2;
    public static TcpConnectionBase[] _tcpConnections = new TcpConnectionBase[Program.NUM_PLAYERS];
    public static bool IsServer = true;
    public static int[] m_imagesIndex = new int[Program.NUM_PLAYERS];
    private static int _numConnectionStarted = 0;
    private static int numPlayersEndedRevealing = 0;
    private static int _numDemographicsReceived = 0;
    public static Demographics[] _demographics = new Demographics[2];
    public const string _paramsFileName = "Params.xml";
    private static CheatEngine viewModel;
    public static MessageLoop m_mainMessageLoop;
    public static MessageLoop[] m_saveMessageLoop;
    public static string RootDir;
    public static int _camIndex;
    public static int _camFrameRate;
    public static int _numGames;
    private static string SERVER_ENDPOINT;

   
        private static void Main(string[] args)
    {
      Console.WriteLine("Welcome to Liar! game server." + Environment.NewLine);
      Console.WriteLine(Environment.NewLine + "Loading parameters file from: {0}" + Environment.NewLine, (object) "Params.xml");
      Program.LoadParams();
      Console.WriteLine("Start Listening on: " + Program.SERVER_ENDPOINT + Environment.NewLine);
      Program.BeginConnectionsStarted();
      Console.WriteLine("Starting Engine..." + Environment.NewLine);
      Program.viewModel = new CheatEngine(Program._numGames);
      Console.WriteLine("Starting Message Loop and waiting for client messages..." + Environment.NewLine);
      Console.WriteLine(Environment.NewLine + "Note: Please re-start any open clients now..." + Environment.NewLine);
      Console.WriteLine(Environment.NewLine + "PRESS CTRL+C ANYTIME TO END SERVER" + Environment.NewLine);
      Console.CancelKeyPress += new ConsoleCancelEventHandler(Program.Console_CancelKeyPress);
      Program.m_saveMessageLoop = new MessageLoop[2];
      for (int index = 0; index < Program.m_saveMessageLoop.Length; ++index)
        Program.m_saveMessageLoop[index] = Program.CreateSaveImageLoop(index);
      Program.m_mainMessageLoop = new MessageLoop();
      Program.m_mainMessageLoop.Run();
      Program.viewModel.OnCloseApp();
      while (Program.m_saveMessageLoop[0].Count > 0)
      {
        Console.WriteLine("Waiting for Save Images 0 task to complete..." + (object) Program.m_saveMessageLoop[0].Count + " images left." + Environment.NewLine);
        Thread.Sleep(1000);
      }
      while (Program.m_saveMessageLoop[1].Count > 0)
      {
        Console.WriteLine("Waiting for Save Images 1 task to complete..." + (object) Program.m_saveMessageLoop[1].Count + " images left." + Environment.NewLine);
        Thread.Sleep(1000);
      }
      Program.m_saveMessageLoop[0].Cancel();
      Program.m_saveMessageLoop[1].Cancel();
    }

    private static MessageLoop CreateSaveImageLoop(int index)
    {
      MessageLoop messageLoop = new MessageLoop();
      new Thread(new ThreadStart(messageLoop.Run))
      {
        Name = ("SaveThread" + (object) index),
        IsBackground = true
      }.Start();
      return messageLoop;
    }

    private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
      e.Cancel = true;
      Program.m_mainMessageLoop.Cancel();
    }

    private static void LoadParams()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load("Params.xml");
      Program._numGames = doc.GetParamInt32("NUM_GAMES");
      Program._camIndex = doc.GetParamInt32("CAM_INDEX");
      Program._camFrameRate = doc.GetParamInt32("CAM_FRAME_RATE");
      Program.RootDir = doc.GetParamString("ROOT_DIR");
      if (Program.RootDir != "" && !Directory.Exists(Program.RootDir))
        Directory.CreateDirectory(Program.RootDir);
      if (string.IsNullOrEmpty(doc.GetParamString("NETWORK")))
      {
        SendEndGameMessagesToPlayers();
        return;
      }
        
      for (int index = 0; index < Program.NUM_PLAYERS; ++index)
      {
        Program.SERVER_ENDPOINT = doc.GetParamString("SERVER_ENDPOINT" + (index + 1).ToString());
        Program._tcpConnections[index] = (TcpConnectionBase) new Server();
        Program._tcpConnections[index].SetIPEndPoints(Program.SERVER_ENDPOINT + ":5432" + (index + 1).ToString(), (string) null);
      }
    }

    protected static void BeginConnectionsStarted()
    {
      for (int index = 0; index < Program.NUM_PLAYERS; ++index)
      {
        Program._tcpConnections[index].Started += new EventHandler(Program.OnTcpConnection_Started);
        Program._tcpConnections[index].MessageReceived += new EventHandler<MessageEventArg>(Program.OnServer_MessageReceived);
        Program._tcpConnections[index].BeginStart();
      }
    }

    protected static void OnTcpConnection_Started(object sender, EventArgs e)
    {
        if (Program.m_mainMessageLoop.InvokeRequired())
            Program.m_mainMessageLoop.BeginInvoke((Delegate)new EventHandler(Program.OnTcpConnection_Started), sender, (object)e);
        else
        {
            Console.WriteLine("Connection established: " + (object)(sender as Server).GetIPEndPoints()[0]);
            _numConnectionStarted = _numConnectionStarted + 1;
        }
        if (_numConnectionStarted >= 2)
            Process.Start(Application.ExecutablePath);
        }

    protected static bool TryInitPlayers()
    {
      if (Program._numDemographicsReceived < Program.NUM_PLAYERS)
        return false;
      string[] Names = new string[Program.NUM_PLAYERS];
      for (int index = 0; index < Program.NUM_PLAYERS; ++index)
        Names[index] = Program._demographics[index].FullName;
      Program.viewModel.InitPlayers(Names);
      return true;
    }

    protected static void OnServer_MessageReceived(object sender, MessageEventArg e)
    {
      if (Program.m_mainMessageLoop.InvokeRequired())
      {
        Program.m_mainMessageLoop.BeginInvoke((Delegate) new EventHandler<MessageEventArg>(Program.OnServer_MessageReceived), sender, (object) e);
      }
      else
      {
        int playerId = sender as Server == Program._tcpConnections[0] ? 0 : 1;
        if (e.Message is DemographicsMessage)
          Program.OnServer_ReceivedDemographics(e.Message as DemographicsMessage, playerId);
        else if (e.Message is MoveMessage)
          Program.OnServer_ReceivedMove(e.Message as MoveMessage, playerId);
        else if (e.Message is ControlMessage)
        {
          Program.OnServer_ReceivedControl(e.Message as ControlMessage, playerId);
        }
        else
        {
          if (!(e.Message is AudioMessage))
            return;
          Program.OnServer_ReceivedAudioMessage(e.Message as AudioMessage, playerId);
        }
      }
    }

    private static void OnServer_ReceivedControl(ControlMessage controlMessage, int playerId)
    {
      if (controlMessage.Commmand != ControlCommandType.Tick)
      Console.WriteLine("Player " + (object) playerId + " received control msg : " + (object) controlMessage.Commmand);
      if (controlMessage.Commmand == ControlCommandType.Report) ReportUnfairPlay();
      if (controlMessage.Commmand == ControlCommandType.Tick)
      {
        _tcpConnections[playerId].stopwatch.Reset();
        _tcpConnections[playerId].stopwatch.Start();
        return;
      }
      if (++Program.numPlayersEndedRevealing != Program.NUM_PLAYERS)
        return;
      Console.WriteLine("Reveal Ended");
      Program.numPlayersEndedRevealing = 0;
      Program.viewModel.OnEndReveal();
    }

    public static void ResetImageIndexes()
    {
      for (int index = 0; index < Program.NUM_PLAYERS; ++index)
        Program.m_imagesIndex[index] = 0;
    }
    private static void ReportUnfairPlay()
    {
      viewModel.GamesArchive.setPrevTurnLiar();
    }
    private static void SaveAudio(AudioMessage msg, TimeSpan time, string folder, int audioIndex, int playerId)
    {
      MessageLoop messageLoop = Program.m_saveMessageLoop[playerId];
      if (messageLoop.InvokeRequired())
      {
        messageLoop.BeginInvoke((Delegate) new Program.SaveAudioHandler(Program.SaveAudio), (object) msg, (object) time, (object) folder, (object)audioIndex, (object) playerId);
      }
      else
      {
        TimeSpan time1 = TimeStamper.Time;
        try
        {
          string str = string.Format("audio_{0:0000}.wav", (object)audioIndex);
          string filename = string.Format("{0}\\{1}", (object) folder, (object) str);
          Console.WriteLine("saving audio file to " + filename);
          WaveFileWriter.CreateWaveFile(filename, msg.GetRecording());
          if (messageLoop.Count % 100 == 0 && messageLoop.Count != 0)
            Console.WriteLine("m_saveMessageLoop" + (object) playerId + ".Count: " + (object) messageLoop.Count);
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error in save recording: " + ex.Message);
        }
        finally
        {
          msg.Dispose();
        }
        TimeSpan timeSpan = TimeStamper.Time - time1;
      }
    }

    private static void OnServer_ReceivedAudioMessage(AudioMessage message, int playerId)
    {
      TimeSpan time = TimeStamper.Time;
      int index = (playerId + 1) % 2;
      Console.WriteLine("Received Audio. Name: " + Program._demographics[playerId].FullName);
      Program._tcpConnections[index].Send(message);
      string currentFolder = Program.viewModel.GetCurrentFolder(playerId);
      if (currentFolder != null)
      {
        Program.SaveAudio(message, time, currentFolder, Program.m_imagesIndex[playerId], playerId);
        ++Program.m_imagesIndex[playerId];
      }
      TimeSpan timeSpan = TimeStamper.Time - time;
   
    }

    private static void OnServer_ReceivedDemographics(DemographicsMessage message, int playerId)
    {
      Program._demographics[playerId] = message.GetDemographics();
      Program.viewModel.GamesArchive.PlayersDemographics[playerId] = Program._demographics[playerId];
      ++Program._numDemographicsReceived;
      Console.WriteLine("Received Demographics. Name: " + Program._demographics[playerId].FullName);
      Program.TryInitPlayers();
      CheatEngine viewModel = Program.viewModel;
      bool flag = false;
      int playerId1 = playerId;
      int num = flag ? 1 : 0;
      viewModel.SendEmptyBoardToOpponent(playerId1, num != 0);
    }

    private static void OnServer_ReceivedMove(MoveMessage message, int playerId)
    {
      
      Move move = message.GetMove();
      Console.WriteLine("revieced move" + move.ToString());
      if (move.MoveType == MoveType.StartPressed)
      {
        Console.WriteLine("recieved start game move");
        Program.viewModel.PlayerStartPressed[playerId] = true;
        CheatEngine viewModel = Program.viewModel;
        bool flag = true;
        int playerId1 = playerId;
        int num = flag ? 1 : 0;
        viewModel.SendEmptyBoardToOpponent(playerId1, num != 0);
        if (Program.viewModel.TryStartGame() != CheatEngine.TryStartGameReturnCodes.MAX_GAMES_REACHED)
          return;
        Console.WriteLine("max games reached");
        SendEndGameMessagesToPlayers();
        Program.m_mainMessageLoop.Cancel();
      }
      else
        Program.viewModel.GameStepOnReceivedOpponentMove(move, playerId);
    }

    public static void SendEndGameMessagesToPlayers()
    {
       for (int index = 0; index < 2; ++index)
         {
           ControlMessage msg = new ControlMessage(ControlCommandType.EndMatch , viewModel.GamesArchive._endGameString[index]);
           Program._tcpConnections[index].Send(msg);
         }        
             
    }

    public static void PlayerDisconectionHandler(TcpConnectionBase client)
    {
      if (_numConnectionStarted >= 2 )
      {
        for (int index = 0; index < 2; ++index)
        {
          ControlMessage msg;
         // Program.m_mainMessageLoop.Cancel();
         if (_numDemographicsReceived >= 2)
          {
            msg = new ControlMessage(ControlCommandType.OpponentDisconected, viewModel.GamesArchive._endGameString[index]);
            Console.WriteLine("sending oponent disconected");
            Program._tcpConnections[index].Send(msg);
            Program.viewModel.OnCloseApp();
          }
          else
          {
            msg = new ControlMessage(ControlCommandType.OpponentDisconected, "");
            Program._tcpConnections[index].Send(msg);
          }
          Thread.Sleep(10000);
          Environment.Exit(0);
        }
      }
      else
      {
        Process.Start(Application.ExecutablePath);
        Environment.Exit(0);
      }
      
      
    }

    public delegate void SaveAudioHandler(AudioMessage msg, TimeSpan time, string folder, int imageIndex, int playerId);
  }
}
