using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	#region SkillInfo
	[System.Serializable]
	public struct SkillInfo
	{
		public Define.Skill type;                   // 자신의 스킬타입
		public List<Define.Skill> skillLinks;       // 현재 스킬 기준으로 링크된 스킬들의 인덱스를 가지고 있음
		public int skillPoint;                      // 현재 스킬포인트
		public int skillMaxPoint;
		public Vector2 pos;                         // 스킬의 위치
		public bool isLock;	
	}

	[Serializable]
	public class SkillData : ILoader<int, SkillInfo>
	{
		public List<SkillInfo> skillInfos = new List<SkillInfo>();

		public Dictionary<int, SkillInfo> MakeDict()
		{
			Dictionary<int, SkillInfo> dict = new Dictionary<int, SkillInfo>();
			foreach (SkillInfo data in skillInfos) {
				dict.Add((int)data.type, data);
			}
			return dict;
		}

		
	}
	#endregion
}