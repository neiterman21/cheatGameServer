// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.BoardMessage
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CheatGame;
using System.Xml;

namespace CentipedeModel.Network.Messages
{
  public class BoardMessage : Message
  {
    private BoardState _state;

    public BoardMessage(BoardState state)
    {
      this._state = state;
    }

    public BoardMessage(XmlDocument xml, byte[] bytes)
      : base(xml, bytes)
    {
      this._state = new BoardState();
      this.LoadProperties((object) this._state);
    }

    protected override void AppendProperties()
    {
      base.AppendProperties();
      this.AppendProperties((object) this._state);
    }

    public BoardState GetBoardState()
    {
      return this._state;
    }
  }
}
