using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

	public enum Scene
	{
		Unknown,
		Start,

		// TO DO : 씬을 호출할때 맵핑된 씬을 선택해서 올릴예정이므로 ENUM으로 현재 상태를 확인및 바꿔준다.
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