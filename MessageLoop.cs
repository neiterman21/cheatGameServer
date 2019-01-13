// Decompiled with JetBrains decompiler
// Type: CheatGame.MessageLoop
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using System;
using System.Collections.Generic;
using System.Threading;

namespace CheatGame
{
  public sealed class MessageLoop
  {
    private Queue<MessageLoop.Task> m_tasks = new Queue<MessageLoop.Task>();
    private AutoResetEvent m_autoResetEvent = new AutoResetEvent(false);
    private bool m_running = true;

    public event EventHandler TaskEmpty;

    public Thread CurrentThread { get; private set; }

    public int Count
    {
      get
      {
        return this.m_tasks.Count;
      }
    }

    public void Run()
    {
      this.CurrentThread = Thread.CurrentThread;
      this.m_running = true;
      while (this.m_running)
      {
        if (this.m_tasks.Count == 0)
          this.m_autoResetEvent.WaitOne();
        if (!this.m_running)
          break;
        MessageLoop.Task task = (MessageLoop.Task) null;
        int count;
        lock (this.m_tasks)
        {
          if (this.m_tasks.Count > 0)
            task = this.m_tasks.Dequeue();
          count = this.m_tasks.Count;
        }
        if (count == 0 && this.TaskEmpty != null)
          this.TaskEmpty((object) this, EventArgs.Empty);
        try
        {
          task?.Invoke();
        }
        catch (Exception ex)
        {
          Console.WriteLine("Critical Error in message loop. Error: " + ex.Message);
        }
      }
    }

    public bool InvokeRequired()
    {
      return Thread.CurrentThread != this.CurrentThread;
    }

    public void BeginInvoke(Delegate method, params object[] args)
    {
      MessageLoop.Task task = new MessageLoop.Task(method, args);
      lock (this.m_tasks)
      {
        this.m_tasks.Enqueue(task);
        this.m_autoResetEvent.Set();
      }
    }

    public void Cancel()
    {
      lock (this.m_tasks)
      {
        this.m_running = false;
        this.m_autoResetEvent.Set();
      }
    }

    public sealed class Task
    {
      public Delegate Method;
      public object[] Args;

      public bool IsCompleted { get; private set; }

      public Task(Delegate method, object[] args)
      {
        this.Method = method;
        this.Args = args;
      }

      public void Invoke()
      {
        this.Method.DynamicInvoke(this.Args);
        this.IsCompleted = true;
      }
    }
  }
}
