using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

	public SkillManager _skill;

	// Managers���� GameManager�� ȣ���ϱ� ������
	// GameManager�� ��ü�� Managers���� ������ Init()�� ȣ���ϰ� �ȴ�.

	public void Init()
	{
		_skill = GameObject.FindObjectOfType<SkillManager>();
		_skill.Init();

	}

	public void Update()
	{
		_skill.SetLinkTransform();		// TODO : Problem �׽�Ʈ�� ����Ű� ��������
	}



}
