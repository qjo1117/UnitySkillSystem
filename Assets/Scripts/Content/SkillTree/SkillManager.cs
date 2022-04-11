using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager
{
    public Skill[] _skills;
    public GameObject _skillHoder = null;


    public int _skillPoint = 10;          // 레벨에 따른 스킬포인트 증가등을 구현하면 될듯

    public void Init()
	{
        if(_skillHoder == null) {
            _skillHoder = Managers.Resource.NewPrefab("Skill\\SkillHolder");
        }

        _skills = new Skill[(int)Define.Skill.End];          // End까지 생성후 셋팅을 해준다.
        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            //_skils.Add();     // 현재 enum에 대한 이름으로 변경후 string으로 집어넣는다.
            GameObject go = Managers.Resource.NewPrefab("Skill\\SkillNode", _skillHoder.transform);
            go.AddComponent<Skill>();

            Skill skill = go.GetComponent<Skill>();
            _skills[i] = skill;
            skill._type = (Define.Skill)i;              // 현재 타입을 설정해준다.
            skill.Init();                               // 타입에 따라서 설정해준다.

            go.GetComponent<RectTransform>().position = new Vector2(100.0f + (i * 200.0f), 600.0f);
            go.transform.GetComponentInChildren<Text>().text = string.Format("SkillNode_{0}", go.GetComponent<Skill>()._type);
            

        }

		#region Test Link
		{
            // TODO :   현재 그래프 형식으로 만들어져있지만 스킬트리는 분명히하자고 하면 트리형식으로 되어있음
            //          그러므로 단방향으로 다시 수정을 해야함
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

    public GameObject GetSkill(int index)                   // gameObject 가져오기 귀찮은 자의 이상한 함수선언
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

        _skills[index]._skillPoint += 1;        // 해당하는 스킬의 포인트를 올려주고 Manager에 있는 포인트를 낮춘다.
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

        _skills[index]._skillPoint -= 1;        // 해당하는 스킬의 포인트를 올려주고 Manager에 있는 포인트를 낮춘다.
        _skillPoint += 1;

    }
    public void PointDown(Define.Skill index)
	{
        PointDown((int)index);
	}

	#endregion
}
