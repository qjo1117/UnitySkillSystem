using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SkillInfo
{

}

public class Skill : MonoBehaviour 
{
	public Define.Skill _type = Define.Skill.None;						// �ڽ��� Ÿ���� ����
	public List<Define.Skill> _skillLink = new List<Define.Skill>();       // �ڽŰ� ����� �༮ ����
	public int _skillPoint = 0;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		// Skil�� �����Ǿ�����
		// Skil�� None�̸� �ʱ�ȭ �۾��� ���Ѵ�.
		if (_type == Define.Skill.None) {
			Managers.Log.Log("Define.Skil.None");
			return;
		}

	}

	#region ����, �ε�
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
		// ������ �ߺ�üũ
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
