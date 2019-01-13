// Decompiled with JetBrains decompiler
// Type: CentipedeModel.JpegEncoding
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace CentipedeModel
{
  public static class JpegEncoding
  {
    public static readonly ImageCodecInfo Codec = ((IEnumerable<ImageCodecInfo>) ImageCodecInfo.GetImageEncoders()).Single<ImageCodecInfo>((Func<ImageCodecInfo, bool>) (c => c.MimeType == "image/jpeg"));
    public static readonly EncoderParameters EncoderParams30 = new EncoderParameters();
    public static readonly EncoderParameters EncoderParams70;
    public static readonly EncoderParameters EncoderParams100;

    static JpegEncoding()
    {
      JpegEncoding.EncoderParams30.Param[0] = new EncoderParameter(Encoder.Quality, 30L);
      JpegEncoding.EncoderParams70 = new EncoderParameters();
      JpegEncoding.EncoderParams70.Param[0] = new EncoderParameter(Encoder.Quality, 70L);
      JpegEncoding.EncoderParams100 = new EncoderParameters();
      JpegEncoding.EncoderParams100.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
    }
  }
}
