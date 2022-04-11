using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
	

	protected override void Init()
	{
		base.Init();
		SceneType = Define.Scene.Start;

		// Scene 관련된 초기화 작업 캐릭터 생성, 리소스맵핑, 메모리 할당, 아이템 맵핑 등등을 여기서 해준다. (각 씬마다 무엇을 할지 분류한 것)
		Managers.Game.Init();
	}

	void Update()
	{

	}

	public override void Clear()
	{
		// TODO : 초기화를 하자
	}
	
}
