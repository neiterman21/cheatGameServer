// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.DemographicsMessage
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Players;
using System.Xml;

namespace CentipedeModel.Network.Messages
{
  public sealed class DemographicsMessage : Message
  {
    private Demographics m_demographics;

    public DemographicsMessage(XmlDocument xml, byte[] bytes)
      : base(xml, bytes)
    {
      this.m_demographics = new Demographics();
      this.LoadProperties((object) this.m_demographics);
    }

    public DemographicsMessage(Demographics demographics)
    {
      this.m_demographics = demographics;
    }

    protected override void AppendProperties()
    {
      base.AppendProperties();
      this.AppendProperties((object) this.m_demographics);
    }

    public Demographics GetDemographics()
    {
      return this.m_demographics;
    }
  }
}
