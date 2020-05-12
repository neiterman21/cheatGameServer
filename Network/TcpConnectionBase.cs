// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.TcpConnectionBase
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Network.Messages;
using CheatGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace CentipedeModel.Network
{
  public abstract class TcpConnectionBase : IDisposable
  {
    protected byte[] m_buffer = new byte[4096];
    protected List<byte> m_messageBuffer = new List<byte>();
    protected Socket m_socket;
    public Stopwatch stopwatch = new Stopwatch();
    public abstract void SetIPEndPoints(string serverIPEndPoint, string ClientIPEndPoint);

    public abstract IPEndPoint[] GetIPEndPoints();

    public event EventHandler Started;

    protected void RaiseStarted()
    {
      this.IsStarted = true;
      if (this.Started == null)
        return;
      this.Started((object) this, new EventArgs());
    }

    public event EventHandler<MessageEventArg> MessageReceived;

    protected void RaiseMessageReceived(Message message)
    {
      if (this.MessageReceived == null)
        return;
      this.MessageReceived((object) this, new MessageEventArg(message));
    }

    public bool IsStarted { get; protected set; }

    protected void BeginReceive()
    {
      this.m_socket.BeginReceive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None, new AsyncCallback(this.AcceptBytes), (object) null);
    }

    private int IndexOf(byte[] array, byte[] value)
    {
      if (array.Length < value.Length)
        return -1;
      int length = value.Length;
      int num = array.Length - length + 1;
      for (int index1 = 0; index1 < num; ++index1)
      {
        for (int index2 = 0; index2 < length && (int) array[index1 + index2] == (int) value[index2]; ++index2)
        {
          if (index2 == length - 1)
            return index1;
        }
      }
      return -1;
    }

    private void AcceptBytes(IAsyncResult result)
    {
      try
      {
        int count1 = this.m_socket.EndReceive(result);

        byte[] array1 = new byte[count1];
        Buffer.BlockCopy((Array) this.m_buffer, 0, (Array) array1, 0, count1);
        if (this.m_messageBuffer.Count > 0)
        {
          int num = Math.Min(Message.EOM.Length - 1, this.m_messageBuffer.Count);
          byte[] numArray = new byte[array1.Length + num];
          for (int index = num; index > 0; --index)
            numArray[num - index] = this.m_messageBuffer[this.m_messageBuffer.Count - index];
          Buffer.BlockCopy((Array) array1, 0, (Array) numArray, num, array1.Length);
          this.m_messageBuffer.RemoveRange(this.m_messageBuffer.Count - num, num);
          array1 = numArray;
        }
        for (int count2 = this.IndexOf(array1, Message.EOM); count2 >= 0; count2 = this.IndexOf(array1, Message.EOM))
        {
          byte[] numArray1 = new byte[count2];
          Buffer.BlockCopy((Array) array1, 0, (Array) numArray1, 0, count2);
          byte[] numArray2 = new byte[array1.Length - count2 - Message.EOM.Length];
          if (numArray2.Length > 0)
            Buffer.BlockCopy((Array) array1, count2 + Message.EOM.Length, (Array) numArray2, 0, numArray2.Length);
          this.m_messageBuffer.AddRange((IEnumerable<byte>) numArray1);
          try
          {
            byte[] array2 = this.m_messageBuffer.ToArray();
            string xml = Encoding.UTF8.GetString(array2);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            this.RaiseMessageReceived(Activator.CreateInstance(Type.GetType(typeof (Message).AssemblyQualifiedName.Replace(".Message,", "." + xmlDocument.DocumentElement.GetAttribute("Type") + ",")), new object[2]
            {
              (object) xmlDocument,
              (object) array2
            }) as Message);
          }
          catch (Exception ex)
          {
            Console.WriteLine("Error in parsing message. Error: " + ex.Message);
          }
          this.m_messageBuffer.Clear();
          array1 = numArray2;
        }
        if (array1.Length > 0)
          this.m_messageBuffer.AddRange((IEnumerable<byte>) array1);
        this.BeginReceive();
      }
      catch (SocketException)
      {
        Console.WriteLine("oponent disconected");
        Program.PlayerDisconectionHandler(this);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error in AcceptBytes(). Error: " + ex.Message);
      }
    }

    public abstract IAsyncResult BeginStart();

    public void Send(Message message)
    {
      byte[] bytes = message.GetBytes();
      byte[] buffer = new byte[bytes.Length + Message.EOM.Length];
      Buffer.BlockCopy((Array) bytes, 0, (Array) buffer, 0, bytes.Length);
      Buffer.BlockCopy((Array) Message.EOM, 0, (Array) buffer, bytes.Length, Message.EOM.Length);
      try
      {
        if (this.m_socket == null || !this.m_socket.Connected)
          return;
        this.m_socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, (AsyncCallback)null, (object)null);
        Console.WriteLine("sent message " + message.Type );
      }
      catch (Exception e)
      {
         Console.WriteLine("Failed sent message " + message.Type);
      }
 
    }

        public abstract void Dispose();
    public bool IsConnected()
    {
      bool blockingState = m_socket.Blocking;

      try
      {
        byte[] tmp = new byte[1];

        m_socket.Blocking = false;
        m_socket.Send(tmp, 0, 0);
        return true;
      }
      catch (SocketException e)
      {
        // 10035 == WSAEWOULDBLOCK
        if (e.NativeErrorCode.Equals(10035))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      finally
      {
        m_socket.Blocking = blockingState;
      }
    }


  }
}
