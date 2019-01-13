// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.ControlMessage
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System.Xml;

namespace CentipedeModel.Network.Messages
{
  public sealed class ControlMessage : Message
  {
    public ControlCommandType Commmand { get; set; }

    public ControlMessage(ControlCommandType Commmand)
    {
      this.Commmand = Commmand;
    }

    public ControlMessage(XmlDocument xml, byte[] bytes)
      : base(xml, bytes)
    {
    }
  }
}
