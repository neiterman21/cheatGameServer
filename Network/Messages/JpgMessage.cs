// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.JpgMessage
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Drawing;
using System.IO;
using System.Xml;

namespace CentipedeModel.Network.Messages
{
  public sealed class JpgMessage : Message
  {
    private Image m_image;
    private MemoryStream m_memoryStream;

    public string Value { get; set; }

    public JpgMessage(XmlDocument xml, byte[] bytes)
      : base(xml, bytes)
    {
    }

    public JpgMessage(Image image)
    {
      this.m_image = image;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        lock (this.m_image)
          this.m_image.Save((Stream) memoryStream, JpegEncoding.Codec, JpegEncoding.EncoderParams100);
        this.Value = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
      }
    }

    public Image GetImage()
    {
      if (this.m_image == null)
      {
        this.m_memoryStream = new MemoryStream(Convert.FromBase64String(this.Value));
        this.m_image = (Image) new Bitmap((Stream) this.m_memoryStream, false);
      }
      return this.m_image;
    }

    public void DisposeImage()
    {
      if (this.m_image != null)
        this.m_image.Dispose();
      if (this.m_memoryStream == null)
        return;
      this.m_memoryStream.Dispose();
    }
  }
}
