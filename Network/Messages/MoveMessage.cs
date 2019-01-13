// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.MoveMessage
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System.Xml;

namespace CentipedeModel.Network.Messages
{
  public sealed class MoveMessage : Message
  {
    private Move _move;

    public MoveMessage(Move move)
    {
      this._move = move;
    }

    public MoveMessage(XmlDocument xml, byte[] bytes)
      : base(xml, bytes)
    {
      this._move = new Move();
      this.LoadProperties((object) this._move);
    }

    protected override void AppendProperties()
    {
      base.AppendProperties();
      this.AppendProperties((object) this._move);
    }

    public Move GetMove()
    {
      return this._move;
    }
  }
}
