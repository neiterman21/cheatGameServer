// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.IPEndPointParser
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System.Net;

namespace CentipedeModel.Network
{
  public static class IPEndPointParser
  {
    public static IPEndPoint Parse(string s)
    {
      string[] strArray = s.Split(':');
      return new IPEndPoint(IPAddress.Parse(strArray[0]), int.Parse(strArray[1]));
    }
  }
}
