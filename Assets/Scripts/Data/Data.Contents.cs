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
		public Define.Skill type;                   // �ڽ��� ��ųŸ��
		public List<Define.Skill> skillLinks;       // ���� ��ų �������� ��ũ�� ��ų���� �ε����� ������ ����
		public int skillPoint;                      // ���� ��ų����Ʈ
		public int skillMaxPoint;
		public Vector2 pos;                         // ��ų�� ��ġ
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