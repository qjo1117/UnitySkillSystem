using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
	

	protected override void Init()
	{
		base.Init();
		SceneType = Define.Scene.Start;

		// Scene ���õ� �ʱ�ȭ �۾� ĳ���� ����, ���ҽ�����, �޸� �Ҵ�, ������ ���� ����� ���⼭ ���ش�. (�� ������ ������ ���� �з��� ��)
		Managers.Game.Init();
	}

	void Update()
	{

	}

	public override void Clear()
	{
		// TODO : �ʱ�ȭ�� ����
	}
	
}
