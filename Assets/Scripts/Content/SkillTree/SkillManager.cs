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

    public int _skillPoint = 10;          // ������ ���� ��ų����Ʈ �������� �����ϸ� �ɵ�

    public Button _btnSave;
    public Button _btnLoad;


    public void Init()
	{
        if (_skillHoder == null) {                           // ����?
            _skillHoder = GameObject.Find("SkillHolder");   // ��¥?
            if (_skillHoder == null) {                      // �׷���?
                Managers.Resource.NewPrefab("Skill/SkillHolder");
            }
        }

        _btnSave = GameObject.Find("Save").GetComponent<Button>();          // �����
        _btnLoad = GameObject.Find("Load").GetComponent<Button>();          // �����

        _btnSave.onClick.AddListener(_On_Save);
        _btnLoad.onClick.AddListener(_On_Load);

        float yDelta = 0.0f;

        // ��ũ ���� Pool
        {
            GameObject obj = Managers.Resource.NewPrefab("Skill/SkillConnection", _skillHoder.transform);
            Managers.Pool.CreatePool(obj, (int)Define.Skill.End * 2);       // �뷫 �ι����� ������ �������ؼ� ����
            Managers.Resource.DelPrefab(obj);
        }

        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            // ���� enum�� ���� �̸����� ������ string���� ����ִ´�.
            GameObject go = Managers.Resource.NewPrefab("Skill/SkillNode", _skillHoder.transform);

            Skill skill = go.GetOrderComponent<Skill>();
            skill._type = (Define.Skill)i;              // ���� Ÿ���� �������ش�.
            skill.Init();                               // Ÿ�Կ� ���� �������ش�.
            _skills.Add(skill);

            yDelta = -yDelta;
            yDelta += 1;

            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400.0f + (i * 200.0f), yDelta * 100.0f);
            _skills[i]._text.text = string.Format("SkillNode_{0}", _skills[i]._type);
        }

        #region Test Link
        {
            // TODO :   ���� �׷��� �������� ������������� ��ųƮ���� �и������ڰ� �ϸ� Ʈ���������� �Ǿ�����
            //          �׷��Ƿ� �ܹ������� �ٽ� ������ �ؾ���
            string strLink = "";


            AddLink(Define.Skill.Fire_1, Define.Skill.Fire_2);
            AddLink(Define.Skill.Fire_1, Define.Skill.Fire_5);
            foreach(Define.Skill type in _skills[(int)Define.Skill.Fire_1]._skillLink) {
                strLink += string.Format("{0}{1}", type, " / ");
            }
            GetSkill(Define.Skill.Fire_1).GetComponentInChildren<Text>().text = strLink;



            AddLink(Define.Skill.Fire_2, Define.Skill.Fire_1);
            AddLink(Define.Skill.Fire_2, Define.Skill.Fire_4);
            strLink = "";
            foreach (Define.Skill type in _skills[(int)Define.Skill.Fire_2]._skillLink) {
                strLink += string.Format("{0}{1}", type, " / ");
            }
            GetSkill(Define.Skill.Fire_2).GetComponentInChildren<Text>().text = strLink;



            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_2);
            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_4);
            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_5);
            AddLink(Define.Skill.Fire_3, Define.Skill.Fire_6);
            strLink = "";
            foreach (Define.Skill type in _skills[(int)Define.Skill.Fire_3]._skillLink) {
                strLink += string.Format("{0}{1}", type, " / ");
            }
            GetSkill(Define.Skill.Fire_3).GetComponentInChildren<Text>().text = strLink;



            AddLink(Define.Skill.Fire_4, Define.Skill.Fire_2);
            AddLink(Define.Skill.Fire_4, Define.Skill.Fire_3);
            strLink = "";
            foreach (Define.Skill type in _skills[(int)Define.Skill.Fire_4]._skillLink) {
                strLink += string.Format("{0}{1}", type, " / ");
            }
            GetSkill(Define.Skill.Fire_4).GetComponentInChildren<Text>().text = strLink;



            AddLink(Define.Skill.Fire_5, Define.Skill.Fire_1);
            AddLink(Define.Skill.Fire_5, Define.Skill.Fire_3);
            strLink = "";
            foreach (Define.Skill type in _skills[(int)Define.Skill.Fire_5]._skillLink) {
                strLink += string.Format("{0}{1}", type, " / ");
            }
            GetSkill(Define.Skill.Fire_5).GetComponentInChildren<Text>().text = strLink;



            AddLink(Define.Skill.Fire_6, Define.Skill.Fire_3);
            strLink = "";
            foreach (Define.Skill type in _skills[(int)Define.Skill.Fire_6]._skillLink) {
                strLink += string.Format("{0}{1}", type, " / ");
            }
            GetSkill(Define.Skill.Fire_6).GetComponentInChildren<Text>().text = strLink;
        }
        #endregion

        SetLinkTransform();             // ���ο� ���� ��ũĿ���͸� ���ش�.
        SetAllLock();


    }

    public void _On_Save()
	{
        string strSave = "";
        int size = _skills.Count;
        // TODO : ������ �˷�����ϴµ� �ȵʤ�
        strSave = JsonUtility.ToJson(size.ToString());

        for (int i = 0; i < _skills.Count; ++i) {
            strSave += _skills[i].Save();
        }
        File.WriteAllText(Application.dataPath + "/TestJson.json", strSave);
    }

    public void _On_Load()
	{
        string strSave = File.ReadAllText(Application.dataPath + "/TestJson.json");
        int size = JsonUtility.FromJson<int>(strSave);
        Managers.Log.Log(size);
        for (int i = 0; i < size; ++i) {
            SkillInfo info = JsonUtility.FromJson<SkillInfo>(strSave);
            Managers.Log.Log(info.type);
            _skills[i].Load(info);
        }
    }

    public void SetAllLock()
    {
        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            _skills[i].SetLock();
        }
        _skills[0].SetUpLock();
    }



    public void SetLinkTransform()
	{

        // �ʱ�ȭ
        for (int i = 0; i < _skillLink.Count; ++i) {
            Managers.Resource.DelPrefab(_skillLink[i].gameObject);
        }
        _skillLink.Clear();


		// ���� �� ��ġ
		for (int i = 0; i < (int)Define.Skill.End - 1; ++i) {
			for (int j = 0; j < _skills[i]._skillLink.Count; ++j) {
				int selfLink = i;
				int targetLink = (int)_skills[i]._skillLink[j];


				RectTransform tranSelf = _skills[selfLink].GetComponent<RectTransform>();
				RectTransform tranTarget = _skills[targetLink].GetComponent<RectTransform>();


				Image link = Managers.Resource.NewPrefab("Skill/SkillConnection", _skillHoder.transform).GetComponent<Image>();
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
    public void PointUp(int index)
    {
        if((int)Define.Skill.End < index || 0 > index) {
            Managers.Log.Log("SkillManager / PointUp Error");
            return;
		}

        _skills[index]._skillPoint += 1;        // �ش��ϴ� ��ų�� ����Ʈ�� �÷��ְ� Manager�� �ִ� ����Ʈ�� �����.
        _skillPoint -= 1;
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
