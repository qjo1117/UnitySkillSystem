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

    public int _skillPoint = 10;          // 레벨에 따른 스킬포인트 증가등을 구현하면 될듯

    public Button _btnSave;
    public Button _btnLoad;


    public void Init()
	{
        if (_skillHoder == null) {                           // 없네?
            _skillHoder = GameObject.Find("SkillHolder");   // 진짜?
            if (_skillHoder == null) {                      // 그렇네?
                Managers.Resource.NewPrefab("Skill/SkillHolder");
            }
        }

        _btnSave = GameObject.Find("Save").GetComponent<Button>();          // 상당한
        _btnLoad = GameObject.Find("Load").GetComponent<Button>();          // 상당한

        _btnSave.onClick.AddListener(_On_Save);
        _btnLoad.onClick.AddListener(_On_Load);

        float yDelta = 0.0f;

        // 링크 생성 Pool
        {
            GameObject obj = Managers.Resource.NewPrefab("Skill/SkillConnection", _skillHoder.transform);
            Managers.Pool.CreatePool(obj, (int)Define.Skill.End * 2);       // 대략 두배정도 있으면 좋을듯해서 만듬
            Managers.Resource.DelPrefab(obj);
        }

        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            // 현재 enum에 대한 이름으로 변경후 string으로 집어넣는다.
            GameObject go = Managers.Resource.NewPrefab("Skill/SkillNode", _skillHoder.transform);

            Skill skill = go.GetOrderComponent<Skill>();
            skill._type = (Define.Skill)i;              // 현재 타입을 설정해준다.
            skill.Init();                               // 타입에 따라서 설정해준다.
            _skills.Add(skill);

            yDelta = -yDelta;
            yDelta += 1;

            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400.0f + (i * 200.0f), yDelta * 100.0f);
            _skills[i]._text.text = string.Format("SkillNode_{0}", _skills[i]._type);
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

        SetLinkTransform();             // 라인에 대한 링크커넥터를 해준다.
        SetAllLock();


    }

    public void _On_Save()
	{
        string strSave = "";
        int size = _skills.Count;
        // TODO : 갯수를 알려줘야하는데 안됨ㄷ
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

        // 초기화
        for (int i = 0; i < _skillLink.Count; ++i) {
            Managers.Resource.DelPrefab(_skillLink[i].gameObject);
        }
        _skillLink.Clear();


		// 생성 및 배치
		for (int i = 0; i < (int)Define.Skill.End - 1; ++i) {
			for (int j = 0; j < _skills[i]._skillLink.Count; ++j) {
				int selfLink = i;
				int targetLink = (int)_skills[i]._skillLink[j];


				RectTransform tranSelf = _skills[selfLink].GetComponent<RectTransform>();
				RectTransform tranTarget = _skills[targetLink].GetComponent<RectTransform>();


				Image link = Managers.Resource.NewPrefab("Skill/SkillConnection", _skillHoder.transform).GetComponent<Image>();
				RectTransform tranLink = link.GetComponent<RectTransform>();
				tranLink.anchoredPosition = tranSelf.anchoredPosition;        // 위치 변경
				link.color = Color.blue;


				Vector2 deltaPos = tranTarget.anchoredPosition - tranSelf.anchoredPosition;       // 거리를 얻어낸다
				tranLink.localScale = new Vector3(deltaPos.magnitude / 100.0f, 0.1f, 0.0f);   // TODO : 100.0f나눠주었는데 차이가 100정도 나는것 같다.
				tranLink.anchoredPosition += deltaPos / 2;        // 선을 중앙으로 변경해준다.


                float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;          // 각도를 정해준다.
				tranLink.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);

				_skillLink.Add(link);       // 맵핑
			}
		}
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
            return;
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
