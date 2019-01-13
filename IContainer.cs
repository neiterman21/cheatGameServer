// Decompiled with JetBrains decompiler
// Type: CheatGame.IContainer
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System.Xml;

namespace CheatGame
{
  public interface IContainer
  {
    void SignalEnd();

    void SaveSummary(XmlElement parent);
  }
}
