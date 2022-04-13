using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

// ���� ���������� �صа��� Save�۾����� ��ų�� ��ǥ�� �����ϰ�
// ��ų�� �����Ҷ� _type�� �̿��ؼ� �ڽ��� ���� ��ų���� Ȯ���ϰ�
// _skillLink�� �̿��ؼ� ���� ������ �����Դϴ�.
// _skillPoint ���� ��ų�� �󸶳� ��������



public class Skill : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

	public Define.Skill			_type = Define.Skill.None;						// �ڽ��� Ÿ���� ����
	public List<Define.Skill>	_skillLinks = new List<Define.Skill>();       // �ڽŰ� ����� �༮ ����
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
		// Skil�� �����Ǿ�����
		// Skil�� None�̸� �ʱ�ȭ �۾��� ���Ѵ�.
		if (_type == Define.Skill.None) {
			Managers.Log.Log("Define.Skil.None");
			return;
		}


		_text = GetComponentInChildren<Text>();         // �������ѱ� ��������
		_button = GetComponent<Button>();
		_rectTransform = GetComponent<RectTransform>();
		_canvas = GameObject.Find("SkillHolder").GetComponent<Canvas>();

		_button.image.sprite = Managers.Game._skill._sprite[(int)_type];
		// TODO : �Ҿ��ϸ� ����ó���ϴ°� ���������� ������.
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
		_button.colors = colorblock;            // ���� ����
	}

	public void SetLock()
	{
		_lock = true;

		// TODO : �� ��������Ʈ�� ���� ������ �־��ָ� �ɵ� �մϴ�.
		//		�� : Lock�� ������ ��������Ʈ�� �������� ������� �˷��ָ� ��.

		ColorBlock colorblock = _button.colors;
		Color color = colorblock.normalColor;
		color.r = 0.0f;
		color.b = 0.0f;
		color.g = 0.0f;
		color.a = 125.0f;
		colorblock.normalColor = color;
		_button.colors = colorblock;			// ���� ����
	}


	#region ����, �ε�


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
		// ������ �ߺ�üũ
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
			tranLink.anchoredPosition = tranSelf.anchoredPosition;        // ��ġ ����
			link.color = Color.blue;


			Vector2 deltaPos = tranTarget.anchoredPosition - tranSelf.anchoredPosition;       // �Ÿ��� ����
			tranLink.localScale = new Vector3(deltaPos.magnitude / 100.0f, 0.1f, 0.0f);   // TODO : 100.0f�����־��µ� ���̰� 100���� ���°� ����.
			tranLink.anchoredPosition += deltaPos / 2;        // ���� �߾����� �������ش�.


			float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;          // ������ �����ش�.
			tranLink.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);

			skillLink.Add(link);       // ����
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
		if(Managers.Game._skill._selectMode == 2) {		// ���� ����

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
