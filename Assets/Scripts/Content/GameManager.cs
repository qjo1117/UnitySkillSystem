using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

	public SkillManager _skill = new SkillManager();

	// Managers���� GameManager�� ȣ���ϱ� ������
	// GameManager�� ��ü�� Managers���� ������ Init()�� ȣ���ϰ� �ȴ�.

	public void Init()
	{
		_skill.Init(); 
	}

	public void Update()
	{

	}



}
