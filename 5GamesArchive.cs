// Decompiled with JetBrains decompiler
// Type: CheatGame.SpanMeasurement`1
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CheatGame
{
  public class SpanMeasurement<T> where T : IComparable, IComparable<T>, IEquatable<T>
  {
    private Func<T> _getState;

    public T OriginMeasurement { get; private set; }

    public T CurrentAccumlated
    {
      get
      {
        dynamic target = this._getState();
        target = target - this.OriginMeasurement;
        return target;  
      } 
    }

    public T Duration { get; private set; }

    public virtual T SignalEnd()
    {
      this.Duration = this.CurrentAccumlated;
      return this.Duration;
    }

    public SpanMeasurement(Func<T> GetState)
    {
      this._getState = GetState;
      this.OriginMeasurement = GetState();
      this.Duration = default (T);
    }
  }
}
