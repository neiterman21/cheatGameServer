// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.Message
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace CentipedeModel.Network.Messages
{
  public abstract class Message
  {
    public static readonly byte[] EOM = new byte[6]
    {
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4
    };
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
    private byte[] m_bytes;
    protected XmlDocument m_xml;

    public string Type { get; set; }

    public Message(XmlDocument xml, byte[] bytes)
    {
      this.m_bytes = bytes;
      this.m_xml = xml;
      this.LoadProperties((object) this);
    }

    public Message()
    {
      this.Type = this.GetType().Name;
      this.m_xml = new XmlDocument();
      this.m_xml.AppendChild((XmlNode) this.m_xml.CreateElement(nameof (Message)));
    }

    protected string GetStringValue(string name)
    {
      return this.m_xml.DocumentElement.GetAttribute(name);
    }

    protected DateTime GetDateTimeValue(string name)
    {
      return DateTime.ParseExact(this.GetStringValue(name), "yyyy-MM-dd HH:mm:ss.fff", (IFormatProvider) null);
    }

    protected T GetValue<T>(string name)
    {
      return (T) this.GetValue(name, typeof (T));
    }

    protected object GetValue(string name, System.Type type)
    {
      if (type == typeof (DateTime))
        return (object) this.GetDateTimeValue(name);
      string stringValue = this.GetStringValue(name);
      return TypeDescriptor.GetConverter(type).ConvertFromString(stringValue);
    }

    protected void Append(string name, string value)
    {
      this.m_xml.DocumentElement.SetAttribute(name, value);
    }

    protected void Append(string name, object value)
    {
      if (value is DateTime)
      {
        this.Append(name, (DateTime) value);
      }
      else
      {
        string str = TypeDescriptor.GetConverter(value.GetType()).ConvertToString(value);
        this.Append(name, str);
      }
    }

    protected void Append(string name, DateTime value)
    {
      this.Append(name, value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
    }

    protected void AppendProperties(object item)
    {
      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item))
      {
        object obj = property.GetValue(item);
        this.Append(property.Name, obj);
      }
    }

    protected void LoadProperties(object item)
    {
      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item))
      {
        object obj = this.GetValue(property.Name, property.PropertyType);
        property.SetValue(item, obj);
      }
    }

    protected virtual void AppendProperties()
    {
      this.m_xml.DocumentElement.RemoveAllAttributes();
      this.AppendProperties((object) this);
    }

    public byte[] GetBytes()
    {
      if (this.m_bytes == null)
      {
        this.AppendProperties();
        this.m_bytes = Encoding.UTF8.GetBytes(this.m_xml.OuterXml);
      }
      return this.m_bytes;
    }
  }
}
