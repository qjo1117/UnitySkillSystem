using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Extension
{
	public static T GetOrderComponent<T>(this GameObject go) where T : Component
	{
		T com = go.GetComponent<T>();
		if(com == null) {
			com = go.AddComponent<T>();
		}
		return com;
	}
}
