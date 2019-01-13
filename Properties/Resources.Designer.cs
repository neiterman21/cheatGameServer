// Decompiled with JetBrains decompiler
// Type: CheatGame.Properties.Resources
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace CheatGame.Properties
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) CheatGame.Properties.Resources.resourceMan, (object) null))
          CheatGame.Properties.Resources.resourceMan = new ResourceManager("CheatGame.Properties.Resources", typeof (CheatGame.Properties.Resources).Assembly);
        return CheatGame.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return CheatGame.Properties.Resources.resourceCulture;
      }
      set
      {
        CheatGame.Properties.Resources.resourceCulture = value;
      }
    }

    internal static Bitmap blankTV
    {
      get
      {
        return (Bitmap) CheatGame.Properties.Resources.ResourceManager.GetObject(nameof (blankTV), CheatGame.Properties.Resources.resourceCulture);
      }
    }
  }
}
