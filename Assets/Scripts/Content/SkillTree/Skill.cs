using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SkillInfo
{

}

public class Skill : MonoBehaviour 
{
	public Define.Skill _type = Define.Skill.None;						// 자신의 타입을 정함
	public List<Define.Skill> _skillLink = new List<Define.Skill>();       // 자신과 연결된 녀석 맵핑
	public int _skillPoint = 0;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		// Skil이 가동되었을떼
		// Skil이 None이면 초기화 작업을 안한다.
		if (_type == Define.Skill.None) {
			Managers.Log.Log("Define.Skil.None");
			return;
		}

	}

	#region 저장, 로드
	public void Save()
	{

	}

	public void Load()
	{

	}
	#endregion

	public void AddLink(int p_type)
	{
		AddLink((Define.Skill)p_type);
	}
	public void AddLink(Define.Skill p_type)
	{
		// 간단한 중복체크
		foreach(Define.Skill type in _skillLink) {
			if(type == p_type) {
				return;
			}
		}
		

		_skillLink.Add(p_type);
	}

	public void DelLink(int p_type)
	{
		DelLink((Define.Skill)p_type);
	}

	public void DelLink(Define.Skill p_type)
	{
		_skillLink.Remove(p_type);
	}


}
