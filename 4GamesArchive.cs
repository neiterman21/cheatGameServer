// Decompiled with JetBrains decompiler
// Type: CheatGame.Archive
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CentipedeModel.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CheatGame
{
   
  public class Archive : Container<Game>
  {
    public Demographics[] PlayersDemographics = new Demographics[2];
    private DateTime _baseDateTime;
    private readonly string _xmlFileNamePrefix;
    private int _endGameStringLen = 20;
    public String[] _endGameString = new String[2];
    Random rand = new Random();
    private bool prev_turn_liar = false;


    public Archive(string XmlFileNamePrefix)
    {
      TimeStamper.Start();
      this._baseDateTime = TimeStamper.BaseDateTime;
      this._xmlFileNamePrefix = XmlFileNamePrefix;
      _endGameString[0] = getRandomString(_endGameStringLen);
      _endGameString[1] = getRandomString(_endGameStringLen);
    }

    public void setPrevTurnLiar()
    {
      prev_turn_liar = true;
    }

    public string getRandomString(int len)
    {
      byte[] bytes = new byte[len];
      rand.NextBytes(bytes);
      StringBuilder str = new StringBuilder();
      foreach (byte byteValue in bytes)
        str.Append( byteValue.ToString("X"));
      return str.ToString();
    }

    private void SavePlayers(XmlElement Root)
    {
      for (int index = 0; index < 2; ++index)
      {
        XmlElement element = Root.OwnerDocument.CreateElement("Player");
        element.SetAttribute("Index", index.ToString());
        element.SetAttribute("Name", this.PlayersDemographics[index].FullName);
        element.SetAttribute("Gender", this.PlayersDemographics[index].Gender.ToString());
        element.SetAttribute("Age", this.PlayersDemographics[index].Age.ToString());
        element.SetAttribute("CountryOfBirth", this.PlayersDemographics[index].CountryOfBirth.ToString());
        element.SetAttribute("ParentsCountryOfBirth", this.PlayersDemographics[index].ParentsCountryOfBirth.ToString());
        element.SetAttribute("EducationField", this.PlayersDemographics[index].EducationField.ToString());
        element.SetAttribute("EducationType", this.PlayersDemographics[index].EducationType.ToString());
        element.SetAttribute("IsStudent", this.PlayersDemographics[index].IsStudent.ToString());
        element.SetAttribute("EndGameString", _endGameString[index]);
        Root.AppendChild((XmlNode) element);
      }
    }

    public void Save(string fileName)
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element1 = xmlDocument.CreateElement(this._xmlFileNamePrefix + nameof (Archive));
      if (prev_turn_liar)
      {
        element1.SetAttribute("prev_turn_liar", "True");
        prev_turn_liar = false;
      }
      xmlDocument.AppendChild((XmlNode) element1);
      XmlElement element2 = xmlDocument.CreateElement("Players");
      element1.AppendChild((XmlNode) element2);
      if (((IEnumerable<Demographics>) this.PlayersDemographics).All<Demographics>((Func<Demographics, bool>) (d => d != null)))
        this.SavePlayers(element2);
      this.SaveSummary(element1);
      element1.SetAttribute("BaseDate", this._baseDateTime.Date.ToShortDateString());
      element1.RemoveAttribute("LiarArchiveIndex");
      string directoryName = Path.GetDirectoryName(fileName);
      if (directoryName != "")
        Directory.CreateDirectory(directoryName);
      xmlDocument.Save(fileName);
    }
  }
}
