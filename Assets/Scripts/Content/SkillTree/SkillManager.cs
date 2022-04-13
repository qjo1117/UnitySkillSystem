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

    public int _skillPoint = 10;          // 레벨에 따른 스킬포인트 증가등을 구현하면 될듯
    public int _selectMode = 0;             // Enum 까지는 귀찮아서 일단 0은 스킬 포인트 업, 1은 드래그앤 드롭

    public Button _btnSave;
    public Button _btnLoad;


    public Sprite[] _sprite = new Sprite[64];

    public void Init()
	{
        if (_skillHoder == null) {                              // 없네?
            _skillHoder = GameObject.Find("SkillHolder");       // 진짜?
            if (_skillHoder == null) {                          // 그렇네?
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

        _btnSave = GameObject.Find("Save").GetComponent<Button>();          // 상당한
        _btnLoad = GameObject.Find("Load").GetComponent<Button>();          // 귀찮니즘

        _btnSave.onClick.AddListener(_On_Save);
        _btnLoad.onClick.AddListener(_On_Load);

        // Sprite
        _sprite = Managers.Resource.LoadAll<Sprite>("Images/SkillIcons");

        float yDelta = 0.0f;

        // 링크 생성 Pool
        {
            GameObject obj = Managers.Resource.NewPrefab("Skill/SkillConnection");
            Managers.Pool.CreatePool(obj, (int)Define.Skill.End * 2);       // 대략 두배정도 있으면 좋을듯해서 만듬
            Managers.Resource.DelPrefab(obj);
        }

        for (int i = 0; i < (int)Define.Skill.End; ++i) {
            // 현재 enum에 대한 이름으로 변경후 string으로 집어넣는다.
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
            // TODO :   현재 그래프 형식으로 만들어져있지만 스킬트리는 분명히하자고 하면 트리형식으로 되어있음
            //          그러므로 단방향으로 다시 수정을 해야함

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

        SetLinkTransform();             // 라인에 대한 링크커넥터를 해준다.
        SetAllLock();
    }

    private GameObject CreateSkillNode(Define.Skill type)
	{
        GameObject go = Managers.Resource.NewPrefab("Skill/SkillNode", _skillNodeHolder.transform);

        Skill skill = go.GetOrderComponent<Skill>();
        skill._type = type;              // 현재 타입을 설정해준다.
        skill.Init();                    // 타입에 따라서 설정해준다.
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
        // TODO : 포인터를 확인해서 맥스를 찍는 순간 다른 Lock된 녀석들도 풀어준다.
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
        SetLinkTransform();             // 라인에 대한 링크커넥터를 해준다.
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

        // 초기화
        for (int i = 0; i < _skillLink.Count; ++i) {
            Managers.Resource.DelPrefab(_skillLink[i].gameObject);
        }
        _skillLink.Clear();


		// 생성 및 배치
		for (int i = 0; i < (int)_skills.Count; ++i) {
			for (int j = 0; j < _skills[i]._skillLinks.Count; ++j) {
				int selfLink = i;
				int targetLink = (int)_skills[i]._skillLinks[j];


				RectTransform tranSelf = _skills[selfLink].GetComponent<RectTransform>();
				RectTransform tranTarget = _skills[targetLink].GetComponent<RectTransform>();


				Image link = Managers.Resource.NewPrefab("Skill/SkillConnection", _skillConnectionHolder.transform).GetComponent<Image>();
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

    public void PointCheckLock(int index)
	{
        if ((int)Define.Skill.End < index || 0 > index) {
            Managers.Log.Log("SkillManager / PointCheckLock Error");
            return;
        }

        if(_skills[index]._skillPoint == _skills[index]._skillMaxPoint) {       // 스킬이 맥스였을경우
            for (int i = 0; i < _skills[index]._skillLinks.Count; ++i) {
                int 인덱스 = (int)_skills[index]._skillLinks[i];            // TODO : 인덱스
                _skills[인덱스].SetUnLock();
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
            _skills[index]._skillPoint += 1;        // 해당하는 스킬의 포인트를 올려주고 Manager에 있는 포인트를 낮춘다.
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

        _skills[index]._skillPoint -= 1;        // 해당하는 스킬의 포인트를 올려주고 Manager에 있는 포인트를 낮춘다.
        _skillPoint += 1;

    }
    public void PointDown(Define.Skill index)
	{
        PointDown((int)index);
	}

	#endregion
}
