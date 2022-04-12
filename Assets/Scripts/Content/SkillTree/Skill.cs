using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// ���� ���������� �صа��� Save�۾����� ��ų�� ��ǥ�� �����ϰ�
// ��ų�� �����Ҷ� _type�� �̿��ؼ� �ڽ��� ���� ��ų���� Ȯ���ϰ�
// _skillLink�� �̿��ؼ� ���� ������ �����Դϴ�.
// _skillPoint ���� ��ų�� �󸶳� ��������

[System.Serializable]
public struct SkillInfo
{
	public Define.Skill type;					// �ڽ��� ��ųŸ��
	public List<Define.Skill> skillLinks;		// ���� ��ų �������� ��ũ�� ��ų���� �ε����� ������ ����
	public Vector2 pos;							// ��ų�� ��ġ	
}

public class Skill : MonoBehaviour 
{
	public Define.Skill			_type = Define.Skill.None;						// �ڽ��� Ÿ���� ����
	public List<Define.Skill>	_skillLink = new List<Define.Skill>();       // �ڽŰ� ����� �༮ ����
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
		// Skil�� �����Ǿ�����
		// Skil�� None�̸� �ʱ�ȭ �۾��� ���Ѵ�.
		if (_type == Define.Skill.None) {
			Managers.Log.Log("Define.Skil.None");
			return;
		}


		_text = GetComponentInChildren<Text>();         // �������ѱ� ��������
		_button = GetComponent<Button>();
		// TODO : �Ҿ��ϸ� ����ó���ϴ°� ���������� ������.
	}

	public void SetUpLock()
	{
		_lock = false;
		Managers.Log.Log("UnLock");
	}

	public void SetLock()
	{
		_lock = true;

		// TODO : �� ��������Ʈ�� ���� ������ �־��ָ� �ɵ� �մϴ�.
		//		�� : Lock�� ������ ��������Ʈ�� �������� ������� �˷��ָ� ��.
		Managers.Log.Log("Lock");
	}



	#region ����, �ε�


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
		// ������ �ߺ�üũ
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
