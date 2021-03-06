﻿// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Server
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Network.Messages;
using CheatGame;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace CentipedeModel.Network
{
  public sealed class Server : TcpConnectionBase
  {
    private TcpListener m_tcpListner;
    private TcpClient m_client;
    private IPEndPoint m_serverIPEndPoint;
    

    public override void SetIPEndPoints(string ServerIPEndPoint, string ClientIPEndPoint)
    {
      this.m_serverIPEndPoint = IPEndPointParser.Parse(ServerIPEndPoint);
    }

    public override IPEndPoint[] GetIPEndPoints()
    {
      return new IPEndPoint[1]{ this.m_serverIPEndPoint };
    }

    public override void Dispose()
    {
      if (this.m_tcpListner != null)
      {
        this.m_tcpListner.Stop();
        if (this.m_tcpListner.Server != null)
          this.m_tcpListner.Server.Dispose();
      }
      if (this.m_client == null)
        return;
      this.m_client.Close();
    }

    private void OnAcceptTcpClient(IAsyncResult result)
    {
      this.m_client = this.m_tcpListner.EndAcceptTcpClient(result);
      this.m_tcpListner.Stop();
      this.m_socket = this.m_client.Client;
      this.m_socket.NoDelay = true;
      this.BeginReceive();
      this.RaiseStarted();
      Thread ping = new Thread(sendPing);
      ping.IsBackground = true;
      ping.Name = "ping thread";
      ping.Start();
    }

    public void sendPing()
    {
      stopwatch.Start();
      Console.WriteLine("starting tick send");
      Send(new ControlMessage(ControlCommandType.Tick));
      Thread.Sleep(1000);
      while (true)
      {
        Thread.Sleep(50);
        if (TimeSpan.FromSeconds(3) <= stopwatch.Elapsed)
        {
          Send(new ControlMessage(ControlCommandType.Tick));
        }
        
        if (TimeSpan.FromSeconds(8) <= stopwatch.Elapsed)
        {
          Console.WriteLine("time passed " + stopwatch.Elapsed);
          Thread.Sleep(5000);
          Program.PlayerDisconectionHandler(this);
        }
      }
    }


    public override IAsyncResult BeginStart()
    {
      this.m_tcpListner = new TcpListener(this.m_serverIPEndPoint);
      this.m_tcpListner.Start();
      return this.m_tcpListner.BeginAcceptTcpClient(new AsyncCallback(this.OnAcceptTcpClient), (object) null);
    }

    
  }
}
