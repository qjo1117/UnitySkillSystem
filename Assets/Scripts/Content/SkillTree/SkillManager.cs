using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager
{
    public Skill[] _skills;
    public GameObject _skillHoder = null;


    public int _skillPoint = 10;          // ������ ���� ��ų����Ʈ �������� �����ϸ� �ɵ�

    public void Init()
	{
        if(_skillHoder == null) {
            _skillHoder = Managers.Resource.NewPrefab("Skill\\SkillHolder");
        }

        _skills = new Skill[(int)Define.Skill.End];          // End���� ������ ������ ���ش�.
        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            //_skils.Add();     // ���� enum�� ���� �̸����� ������ string���� ����ִ´�.
            GameObject go = Managers.Resource.NewPrefab("Skill\\SkillNode", _skillHoder.transform);
            go.AddComponent<Skill>();

            Skill skill = go.GetComponent<Skill>();
            _skills[i] = skill;
            skill._type = (Define.Skill)i;              // ���� Ÿ���� �������ش�.
            skill.Init();                               // Ÿ�Կ� ���� �������ش�.

            go.GetComponent<RectTransform>().position = new Vector2(100.0f + (i * 200.0f), 600.0f);
            go.transform.GetComponentInChildren<Text>().text = string.Format("SkillNode_{0}", go.GetComponent<Skill>()._type);
            

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

  //      for(int i = 0; i < _skills.Length; i++) {
  //          GetSkill(i).SetActive(false);
		//}
  //      GetSkill(0).SetActive(true);

        #endregion
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
