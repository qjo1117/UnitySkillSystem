using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

	public enum Scene
	{
		Unknown,
		Start,

		// TO DO : ���� ȣ���Ҷ� ���ε� ���� �����ؼ� �ø������̹Ƿ� ENUM���� ���� ���¸� Ȯ�ι� �ٲ��ش�.
		Intro,
		Login,
		SelectRoom,
		InGame,

		Game,
	}

	public enum Skill
	{
		None = -1,

		Fire_1,
		Fire_2,
		Fire_3,
		Fire_4,
		Fire_5,
		Fire_6,

		End
	};

}