using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// 현재 구상적으로 해둔것은 Save작업에서 스킬의 좌표를 저장하고
// 스킬을 셋팅할때 _type를 이용해서 자신이 무슨 스킬인지 확인하고
// _skillLink를 이용해서 선을 연결할 생각입니다.
// _skillPoint 현재 스킬이 얼마나 찍히는지

[System.Serializable]
public struct SkillInfo
{
	public Define.Skill type;					// 자신의 스킬타입
	public List<Define.Skill> skillLinks;		// 현재 스킬 기준으로 링크된 스킬들의 인덱스를 가지고 있음
	public Vector2 pos;							// 스킬의 위치	
}

public class Skill : MonoBehaviour 
{
	public Define.Skill			_type = Define.Skill.None;						// 자신의 타입을 정함
	public List<Define.Skill>	_skillLink = new List<Define.Skill>();       // 자신과 연결된 녀석 맵핑
	public int					_skillPoint = 0;
	public bool					_lock = false;

	public Text					_text;
	public Button				_button;

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


		_text = GetComponentInChildren<Text>();         // 귀찮으닌깐 맵핑하자
		_button = GetComponent<Button>();
		// TODO : 불안하면 예외처리하는게 정상이지만 귀찮다.
	}

	public void SetUpLock()
	{
		_lock = false;
		Managers.Log.Log("UnLock");
	}

	public void SetLock()
	{
		_lock = true;

		// TODO : 각 스프라이트에 대한 정보를 넣어주면 될듯 합니다.
		//		즉 : Lock을 했을때 스프라이트가 무엇으로 변경될지 알려주면 됨.
		Managers.Log.Log("Lock");
	}



	#region 저장, 로드


	public string Save()
	{
		SkillInfo info = new SkillInfo();
		info.type = _type;
		info.skillLinks = _skillLink;
		info.pos = GetComponent<RectTransform>().anchoredPosition;

		// Save
		return JsonUtility.ToJson(info);
	}

	public void Load(SkillInfo info)
	{
		string strSave = "";
		// Load
		info = JsonUtility.FromJson<SkillInfo>(strSave);

		_type = info.type;
		_skillLink = info.skillLinks;
		GetComponent<RectTransform>().anchoredPosition = info.pos;
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

	[EnumAction(typeof(Define.Skill))]
	public void _On_PointUp(int p_click)
	{
		Managers.Game._skill.PointUp((Define.Skill)p_click);
		Managers.Log.Log(string.Format("Skill : {0} / SkillPoint : {1}", _type, _skillPoint));
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
