// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.MessageEventArg
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;

namespace CentipedeModel.Network.Messages
{
  public sealed class MessageEventArg : EventArgs
  {
    public Message Message { get; private set; }

    public MessageEventArg(Message msg)
    {
      this.Message = msg;
    }
  }
}
