using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class SkillManager : MonoBehaviour 
{
    public List<Skill> _skills = new List<Skill>();
    public List<Image> _skillLink = new List<Image>();

    public GameObject _skillHoder = null;
    public GameObject _skillConnectionHolder = null;
    public GameObject _skillNodeHolder = null;

    public int _skillPoint = 10;          // ������ ���� ��ų����Ʈ �������� �����ϸ� �ɵ�
    public int _selectMode = 0;             // Enum ������ �����Ƽ� �ϴ� 0�� ��ų ����Ʈ ��, 1�� �巡�׾� ���

    public Button _btnSave;
    public Button _btnLoad;


    public Sprite[] _sprite = new Sprite[64];

    public void Init()
	{
        if (_skillHoder == null) {                              // ����?
            _skillHoder = GameObject.Find("SkillHolder");       // ��¥?
            if (_skillHoder == null) {                          // �׷���?
                Managers.Resource.NewPrefab("Skill/SkillHolder");
            }
        }

        if(_skillConnectionHolder == null) {
            _skillConnectionHolder = GameObject.Find("SkillConnectionHolder");
            if (_skillConnectionHolder == null) {
                _skillConnectionHolder = new GameObject { name = "SkillConnectionHolder" };
                _skillConnectionHolder.GetOrderComponent<RectTransform>().localPosition = Vector2.zero;
            }
            _skillConnectionHolder.transform.SetParent(_skillHoder.transform);
        }

        if (_skillNodeHolder == null) {
            _skillNodeHolder = GameObject.Find("SkillNodeHolder");
            if (_skillNodeHolder == null) {
                _skillNodeHolder = new GameObject { name = "SkillNodeHolder" };
                _skillNodeHolder.GetOrderComponent<RectTransform>().localPosition = Vector2.zero;
            }
            _skillNodeHolder.transform.SetParent(_skillHoder.transform);
        }

        _btnSave = GameObject.Find("Save").GetComponent<Button>();          // �����
        _btnLoad = GameObject.Find("Load").GetComponent<Button>();          // ��������

        _btnSave.onClick.AddListener(_On_Save);
        _btnLoad.onClick.AddListener(_On_Load);

        // Sprite
        _sprite = Managers.Resource.LoadAll<Sprite>("Images/SkillIcons");

        float yDelta = 0.0f;

        // ��ũ ���� Pool
        {
            GameObject obj = Managers.Resource.NewPrefab("Skill/SkillConnection");
            Managers.Pool.CreatePool(obj, (int)Define.Skill.End * 2);       // �뷫 �ι����� ������ �������ؼ� ����
            Managers.Resource.DelPrefab(obj);
        }

        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            // ���� enum�� ���� �̸����� ������ string���� ����ִ´�.
            GameObject go = CreateSkillNode((Define.Skill)i);

            yDelta = -yDelta;
            yDelta += 1;

            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400.0f + (i * 200.0f), yDelta * 100.0f);
            _skills[i]._text.text = string.Format("SkillNode_{0}", _skills[i]._type);


            int index = i;
            _skills[i]._button.onClick.AddListener(() => { _On_PointUp(index); });
        }

        #region Test Link
        {
            // TODO :   ���� �׷��� �������� ������������� ��ųƮ���� �и������ڰ� �ϸ� Ʈ���������� �Ǿ�����
            //          �׷��Ƿ� �ܹ������� �ٽ� ������ �ؾ���

            AddLink(Define.Skill.Fire_1, Define.Skill.Fire_2);
            AddLink(Define.Skill.Fire_1, Define.Skill.Fire_5);



            AddLink(Define.Skill.Fire_2, Define.Skill.Fire_1);
            AddLink(Define.Skill.Fire_2, Define.Skill.Fire_3);
            AddLink(Define.Skill.Fire_2, Define.Skill.Fire_4);



            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_2);
            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_4);
            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_5);
            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_6);



            AddLink(Define.Skill.Fire_4, Define.Skill.Fire_2);
            AddLink(Define.Skill.Fire_4, Define.Skill.Fire_3);



            AddLink(Define.Skill.Fire_5, Define.Skill.Fire_1);
            AddLink(Define.Skill.Fire_5, Define.Skill.Fire_3);



            AddLink(Define.Skill.Fire_6, Define.Skill.Fire_3);
        }
        #endregion

        SetLinkTransform();             // ���ο� ���� ��ũĿ���͸� ���ش�.
        SetAllLock();
    }

    private GameObject CreateSkillNode(Define.Skill type)
	{
        GameObject go = Managers.Resource.NewPrefab("Skill/SkillNode", _skillNodeHolder.transform);

        Skill skill = go.GetOrderComponent<Skill>();
        skill._type = type;              // ���� Ÿ���� �������ش�.
        skill.Init();                    // Ÿ�Կ� ���� �������ش�.
        _skills.Add(skill);
        return go;
    }


    public void _On_PointUp(int index)
	{
        if(_skillPoint == 0) {
            Managers.Log.Log("_On Point SkillPoint X");
            return;
		}

        if(_skills[index]._lock == true) {
            Managers.Log.Log("_On_ PointUp Class Lock");
            return;
		}

        if(_selectMode == 1) {
            return;
		}

        PointUp(index);
        // TODO : �����͸� Ȯ���ؼ� �ƽ��� ��� ���� �ٸ� Lock�� �༮�鵵 Ǯ���ش�.
        PointCheckLock(index);
    }

    public class SkillInfoList
	{
        public List<Data.SkillInfo> skillList = new List<Data.SkillInfo>();
    }

    public void _On_Save()
	{
        string strSave = "";

        SkillInfoList list = new SkillInfoList();
        for (int i = 0; i < _skills.Count; ++i) {
            Data.SkillInfo info = new Data.SkillInfo();

            info.type = _skills[i]._type;
            info.skillPoint = _skills[i]._skillPoint;
            info.skillLinks = _skills[i]._skillLinks;
            info.pos = _skills[i]._rectTransform.anchoredPosition;
            info.isLock = _skills[i]._lock;

            list.skillList.Add(info);
        }
        strSave = JsonUtility.ToJson(list);
        Managers.Log.Log(strSave);

        File.WriteAllText(Application.dataPath + "/TestJson.json", strSave);
    }

    public void _On_Load()
	{
        string strSave = File.ReadAllText(Application.dataPath + "/TestJson.json");
        SkillInfoList list = JsonUtility.FromJson<SkillInfoList>(strSave);
        ClearSkill();

        for (int i = 0; i < list.skillList.Count; ++i) {
            GameObject go = CreateSkillNode(list.skillList[i].type);
            go.GetComponent<Skill>()._skillPoint = list.skillList[i].skillPoint;
            go.GetComponent<Skill>()._skillLinks =  list.skillList[i].skillLinks;
            go.GetComponent<Skill>()._rectTransform.anchoredPosition = list.skillList[i].pos;
            if(list.skillList[i].isLock) {
                go.GetComponent<Skill>().SetLock();
            }
            else {
                go.GetComponent<Skill>().SetUnLock();
            }

            _skills.Add(go.GetComponent<Skill>());
        }
        SetLinkTransform();             // ���ο� ���� ��ũĿ���͸� ���ش�.
    }

    private void ClearSkill()
	{
        foreach(Skill skill in _skills) {
            Managers.Resource.DelPrefab(skill.gameObject);
		}
        _skills.Clear();

    }

    public void SetAllLock()
    {
        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            _skills[i].SetLock();
        }
        _skills[0].SetUnLock();
    }



    public void SetLinkTransform()
	{

        // �ʱ�ȭ
        for (int i = 0; i < _skillLink.Count; ++i) {
            Managers.Resource.DelPrefab(_skillLink[i].gameObject);
        }
        _skillLink.Clear();


		// ���� �� ��ġ
		for (int i = 0; i < (int)_skills.Count; ++i) {
			for (int j = 0; j < _skills[i]._skillLinks.Count; ++j) {
				int selfLink = i;
				int targetLink = (int)_skills[i]._skillLinks[j];


				RectTransform tranSelf = _skills[selfLink].GetComponent<RectTransform>();
				RectTransform tranTarget = _skills[targetLink].GetComponent<RectTransform>();


				Image link = Managers.Resource.NewPrefab("Skill/SkillConnection", _skillConnectionHolder.transform).GetComponent<Image>();
				RectTransform tranLink = link.GetComponent<RectTransform>();
				tranLink.anchoredPosition = tranSelf.anchoredPosition;        // ��ġ ����
				link.color = Color.blue;


				Vector2 deltaPos = tranTarget.anchoredPosition - tranSelf.anchoredPosition;       // �Ÿ��� ����
				tranLink.localScale = new Vector3(deltaPos.magnitude / 100.0f, 0.1f, 0.0f);   // TODO : 100.0f�����־��µ� ���̰� 100���� ���°� ����.
				tranLink.anchoredPosition += deltaPos / 2;        // ���� �߾����� �������ش�.


                float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;          // ������ �����ش�.
				tranLink.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);

				_skillLink.Add(link);       // ����
			}
		}
	}



    public GameObject GetSkill(int index)                   // gameObject �������� ������ ���� �̻��� �Լ�����
	{
        return _skills[index].gameObject;
	}
    public GameObject GetSkill(Define.Skill index)
	{
        return GetSkill((int)index);
    }

    public void AddLink(Define.Skill self, Define.Skill target)
	{
        _skills[(int)self].AddLink(target);
    }


    #region Helper

    public void PointCheckLock(int index)
	{
        if ((int)Define.Skill.End < index || 0 > index) {
            Managers.Log.Log("SkillManager / PointCheckLock Error");
            return;
        }

        if(_skills[index]._skillPoint == _skills[index]._skillMaxPoint) {       // ��ų�� �ƽ��������
            for (int i = 0; i < _skills[index]._skillLinks.Count; ++i) {
                int �ε��� = (int)_skills[index]._skillLinks[i];            // TODO : �ε���
                _skills[�ε���].SetUnLock();
            }
        }

    }

    public void PointUp(int index)
    {
        if((int)Define.Skill.End < index || 0 > index) {
            Managers.Log.Log("SkillManager / PointUp Error");
            return;
		}

        if (_skills[index]._skillPoint < _skills[index]._skillMaxPoint) {
            _skills[index]._skillPoint += 1;        // �ش��ϴ� ��ų�� ����Ʈ�� �÷��ְ� Manager�� �ִ� ����Ʈ�� �����.
            _skillPoint -= 1;
        }
    }

    public void PointUp(Define.Skill index)
	{
        PointUp((int)index);
	}

    public void PointDown(int index)
    {
        if ((int)Define.Skill.End < index || 0 > index) {
            Managers.Log.Log("SkillManager / PointDown Error");
            return;
        }

        _skills[index]._skillPoint -= 1;        // �ش��ϴ� ��ų�� ����Ʈ�� �÷��ְ� Manager�� �ִ� ����Ʈ�� �����.
        _skillPoint += 1;

    }
    public void PointDown(Define.Skill index)
	{
        PointDown((int)index);
	}

	#endregion
}
