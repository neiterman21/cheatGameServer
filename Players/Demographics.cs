// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Players.Demographics
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Players.Enumarations;

namespace CentipedeModel.Players
{
  public sealed class Demographics
  {
    public string FullName { get; set; }

    public int Age { get; set; }

    public Genders Gender { get; set; }

    public Counties CountryOfBirth { get; set; }

    public Counties ParentsCountryOfBirth { get; set; }

    public EducationType EducationType { get; set; }

    public EducationFields EducationField { get; set; }

    public bool IsStudent { get; set; }
  }
}
