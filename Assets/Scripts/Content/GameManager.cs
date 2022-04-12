using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

	public SkillManager _skill;

	// Managers에서 GameManager를 호출하기 때문에
	// GameManager의 객체는 Managers에서 생성후 Init()를 호출하게 된다.

	public void Init()
	{
		_skill = GameObject.FindObjectOfType<SkillManager>();
		_skill.Init();

	}

	public void Update()
	{
		_skill.SetLinkTransform();		// TODO : Problem 테스트로 만든거고 지워야함
	}



}
