using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager 
{


	public void Init()
	{
	}

	public void Log(object message)
	{
#if DEBUG
		Debug.Log(message);
#endif
	}
	public void Log(object message, Object context)
	{
#if DEBUG
		Debug.Log(message, context);
#endif
	}
}
