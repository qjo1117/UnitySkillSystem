using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
	// 사용방법
	//public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();
	//public void Init()
	//{
	//    StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
	//}

	#region Stat Example
	//[Serializable]
	//public class Stat
	//{
	//	public int level;
	//	public int maxHp;
	//	public int attack;
	//	public int totalExp;
	//}

	//[Serializable]
	//public class StatData : ILoader<int, Stat>
	//{
	//	public List<Stat> stats = new List<Stat>();

	//	public Dictionary<int, Stat> MakeDict()
	//	{
	//		Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
	//		foreach (Stat stat in stats)
	//			dict.Add(stat.level, stat);
	//		return dict;
	//	}
	//}
	#endregion

	public Dictionary<int, Data.SkillInfo> SkillDict { get; private set; } = new Dictionary<int, Data.SkillInfo>();
	public void Init()
	{
		//SkillDict = LoadJson<Data.SkillData, int, Data.SkillInfo>("StatData").MakeDict();
	}

	Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

	
}
