using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

// 현재 구상적으로 해둔것은 Save작업에서 스킬의 좌표를 저장하고
// 스킬을 셋팅할때 _type를 이용해서 자신이 무슨 스킬인지 확인하고
// _skillLink를 이용해서 선을 연결할 생각입니다.
// _skillPoint 현재 스킬이 얼마나 찍히는지



public class Skill : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

	public Define.Skill			_type = Define.Skill.None;						// 자신의 타입을 정함
	public List<Define.Skill>	_skillLinks = new List<Define.Skill>();       // 자신과 연결된 녀석 맵핑
	public int					_skillPoint = 0;
	public int					_skillMaxPoint = 5;
	public bool					_lock = false;

	public Text					_text;
	public Button				_button;
	public RectTransform		_rectTransform;
	public Canvas				_canvas;

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
		_rectTransform = GetComponent<RectTransform>();
		_canvas = GameObject.Find("SkillHolder").GetComponent<Canvas>();

		_button.image.sprite = Managers.Game._skill._sprite[(int)_type];
		// TODO : 불안하면 예외처리하는게 정상이지만 귀찮다.
	}

	public void SetUnLock()
	{
		_lock = false;

		ColorBlock colorblock = _button.colors;
		Color color = colorblock.normalColor;
		color.r = 255.0f;
		color.g = 255.0f;
		color.b = 255.0f;
		color.a = 255.0f;
		colorblock.normalColor = color;
		_button.colors = colorblock;            // 색깔 지정
	}

	public void SetLock()
	{
		_lock = true;

		// TODO : 각 스프라이트에 대한 정보를 넣어주면 될듯 합니다.
		//		즉 : Lock을 했을때 스프라이트가 무엇으로 변경될지 알려주면 됨.

		ColorBlock colorblock = _button.colors;
		Color color = colorblock.normalColor;
		color.r = 0.0f;
		color.b = 0.0f;
		color.g = 0.0f;
		color.a = 125.0f;
		colorblock.normalColor = color;
		_button.colors = colorblock;			// 색깔 지정
	}


	#region 저장, 로드


	public string Save()
	{
		Data.SkillInfo info = new Data.SkillInfo();
		info.type = _type;
		info.skillLinks = _skillLinks;
		info.pos = GetComponent<RectTransform>().anchoredPosition;

		// Save
		return JsonUtility.ToJson(info);
	}

	public void Load(Data.SkillInfo info)
	{
		string strSave = "";
		// Load
		info = JsonUtility.FromJson<Data.SkillInfo>(strSave);

		_type = info.type;
		_skillLinks = info.skillLinks;
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
		foreach(Define.Skill type in _skillLinks) {
			if(type == p_type) {
				return;
			}
		}
		

		_skillLinks.Add(p_type);
	}

	public void SetTransformLink(int self)
	{
		List<Skill> skills = Managers.Game._skill._skills;
		List<Image> skillLink = Managers.Game._skill._skillLink;
		Transform trans = Managers.Game._skill._skillConnectionHolder.transform;

		for (int j = 0; j < skills[self]._skillLinks.Count; ++j) {
			int selfLink = self;
			int targetLink = (int)skills[self]._skillLinks[j];


			RectTransform tranSelf = skills[selfLink].GetComponent<RectTransform>();
			RectTransform tranTarget = skills[targetLink].GetComponent<RectTransform>();


			Image link = Managers.Resource.NewPrefab("Skill/SkillConnection", trans).GetComponent<Image>();
			RectTransform tranLink = link.GetComponent<RectTransform>();
			tranLink.anchoredPosition = tranSelf.anchoredPosition;        // 위치 변경
			link.color = Color.blue;


			Vector2 deltaPos = tranTarget.anchoredPosition - tranSelf.anchoredPosition;       // 거리를 얻어낸다
			tranLink.localScale = new Vector3(deltaPos.magnitude / 100.0f, 0.1f, 0.0f);   // TODO : 100.0f나눠주었는데 차이가 100정도 나는것 같다.
			tranLink.anchoredPosition += deltaPos / 2;        // 선을 중앙으로 변경해준다.


			float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;          // 각도를 정해준다.
			tranLink.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);

			skillLink.Add(link);       // 맵핑
		}
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
		_skillLinks.Remove(p_type);
	}

	#region UI Drag Action
	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(Managers.Game._skill._selectMode == 2) {		// 간선 연결

		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{

	}

	public void OnDrag(PointerEventData eventData)
	{
		if (Managers.Game._skill._selectMode == 1) {
			_rectTransform.anchoredPosition += eventData.delta;
			Managers.Game._skill.SetLinkTransform();
		}
	}
	#endregion
}
