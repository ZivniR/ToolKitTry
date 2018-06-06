using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#if !UNITY_EDITOR
using System.Threading.Tasks;  
#endif

/// <summary>
///  Used for handeling Thread that is compatible with UWP and Unity 
/// </summary>
public class ThreadCompat : MonoBehaviour
{

    //Delegate for runnable function
    public delegate void RunFunction();

    private RunFunction function;

#if !UNITY_EDITOR
    private Task exchangeTask;
#endif

#if UNITY_EDITOR
    private Thread exchangeThread;
#endif

    /// <summary>
    ///  Start the thread
    ///  Us like:
    ///  
    ///  threadComp.start(new ThreadCompat.RunFunction(threadFunc));
    ///  ...
    ///  public void threadFunc(){
    ///     ...
    ///  }
    ///  
    /// </summary>
    /// <param name="function">public delegate void RunFunction();</param>
    public void start(RunFunction function)
    {
        this.function = function;

#if UNITY_EDITOR
        exchangeThread = new System.Threading.Thread(runnable);
        exchangeThread.Start();
#else
        exchangeTask = Task.Run(() => runnable());
#endif

    }

    private void runnable()
    {
        this.function();
    }

}