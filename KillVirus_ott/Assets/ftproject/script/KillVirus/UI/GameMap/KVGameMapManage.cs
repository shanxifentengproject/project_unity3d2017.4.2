using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class KVGameMapManage : MonoBehaviourIgnoreGui
{
    [System.Serializable]
    public class MapData
    {
        public GameObject m_MapObj;
        public Transform m_Parent;
        public float m_DisX = 200f;
        public float m_MaxScale = 1.8f;
        public float m_MinScale = 1f;
        internal int m_MapCount = 4;
        internal float m_TimeChange = 0.3f;
        internal List<Transform> m_MapList = new List<Transform>();
        /// <summary>
        /// 最大关卡数
        /// </summary>
        internal int m_MaxLevel = 10;
        /// <summary>
        /// 当前关卡数
        /// </summary>
        public int m_CurLevel = 1;
        /// <summary>
        /// 最新解锁的关卡数
        /// </summary>
        public int m_UnlockMaxLevel = 5;
    }
    public MapData m_MapData;
    bool IsMoveMapToNextLevel = false;

    void Awake()
    {
        VirusGameMrg.Instance.m_UIMrg.m_MapManage = this;
    }

    /// <summary>
    /// 初始化4个单元地图
    /// </summary>
    public void Init()
    {
        m_MapData.m_MapList.Add(m_MapData.m_MapObj.transform);
        for (int i = 0; i < m_MapData.m_MapCount - 1; i++)
        {
            GameObject obj = QyCreateObject.Instantiate(m_MapData.m_MapObj, m_MapData.m_Parent);
            if (obj != null)
            {
                m_MapData.m_MapList.Add(obj.transform);
            }
        }

        for (int i = 0; i < m_MapData.m_MapCount; i++)
        {
            Vector3 pos = Vector3.zero;
            pos.x = (i - 1) * m_MapData.m_DisX;
            m_MapData.m_MapList[i].name = "map0" + (i + 1).ToString();
            m_MapData.m_MapList[i].localPosition = pos;
            if (i != 1)
            {
                m_MapData.m_MapList[i].localScale = new Vector3(m_MapData.m_MinScale, m_MapData.m_MinScale, 1f);
            }
            else
            {
                m_MapData.m_MapList[i].localScale = new Vector3(m_MapData.m_MaxScale, m_MapData.m_MaxScale, 1f);
            }
        }

        if (IGamerProfile.Instance != null)
        {
            m_MapData.m_MaxLevel = IGamerProfile.gameLevel.mapData.Length;
            m_MapData.m_UnlockMaxLevel = IGamerProfile.Instance.getLastLockedMap + 1;
        }
        m_MapData.m_CurLevel = VirusGameDataAdapter.GetLevel();
        //m_MapData.m_CurLevel = 2; m_MapData.m_MaxLevel = 2; m_MapData.m_UnlockMaxLevel = 1; //test
        InitCurrentLevel();
        UpdateLevalInfo();
        UpdateMapLevelLock();
    }

    internal void ChangeMapGroupParent(Transform tr)
    {
        transform.SetParent(tr);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    void UpdateLevalInfo()
    {
        for (int i = 0; i < m_MapData.m_MapList.Count; i++)
        {
            UiMapData dt = m_MapData.m_MapList[i].GetComponent<UiMapData>();
            UpdateLevelInfo(dt, m_MapData.m_CurLevel - 1 + i);
        }
    }

    void UpdateLevelInfo(UiMapData dt, int level)
    {
        if (dt != null)
        {
            dt.levelText.text = level.ToString();
        }
    }

    void InitCurrentLevel()
    {
        UpdateCurrentLevel(m_MapData.m_CurLevel);
    }

    /// <summary>
    /// 更新关卡UI显示状态
    /// </summary>
    void UpdateCurrentLevel(int curLevel)
    {
        for (int i = 0; i < m_MapData.m_MapList.Count; i++)
        {
            int val = i + curLevel - 1;
            QyFun.SetActive(m_MapData.m_MapList[i].gameObject, val <= 0 || val > m_MapData.m_MaxLevel ? false : true);
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        MoveMapToNextLevel();
    //    }
    //}

    internal void MoveMapToNextLevel()
    {
        if (m_MapData.m_CurLevel >= m_MapData.m_MaxLevel)
        {
            return;
        }

        if (IsMoveMapToNextLevel == true)
        {
            return;
        }
        IsMoveMapToNextLevel = true;

        //if (m_MapData.m_CurLevel == m_MapData.m_MaxLevel - 1)
        //{
        //    CheckCurrentLevel(m_MapData.m_MaxLevel);
        //}
        Transform trParent = m_MapData.m_MapObj.transform.parent;
        TweenPosition com = trParent.gameObject.AddComponent<TweenPosition>();
        com.from = trParent.localPosition;
        com.to = com.from - new Vector3(m_MapData.m_DisX, 0f, 0f);
        com.duration = m_MapData.m_TimeChange;
        com.PlayForward();
        com.AddOnFinished(onFinished);

        Transform tr = m_MapData.m_MapList[1];
        TweenScale scale = tr.gameObject.AddComponent<TweenScale>();
        scale.from = new Vector3(m_MapData.m_MaxScale, m_MapData.m_MaxScale, 1f);
        scale.to = new Vector3(m_MapData.m_MinScale, m_MapData.m_MinScale, 1f);
        scale.duration = m_MapData.m_TimeChange;
        scale.PlayForward();

        tr = m_MapData.m_MapList[2];
        scale = tr.gameObject.AddComponent<TweenScale>();
        scale.from = new Vector3(m_MapData.m_MinScale, m_MapData.m_MinScale, 1f);
        scale.to = new Vector3(m_MapData.m_MaxScale, m_MapData.m_MaxScale, 1f);
        scale.duration = m_MapData.m_TimeChange;
        scale.PlayForward();
    }

    void onFinished()
    {
        if (IsMoveMapToNextLevel == false)
        {
            return;
        }
        IsMoveMapToNextLevel = false;
        Transform trParent = m_MapData.m_MapObj.transform.parent;
        TweenPosition com = trParent.gameObject.GetComponent<TweenPosition>();
        if (com != null)
        {
            Destroy(com);
        }

        Transform tr = m_MapData.m_MapList[1];
        TweenScale scale = tr.gameObject.GetComponent<TweenScale>();
        if (scale != null)
        {
            Destroy(scale);
        }

        tr = m_MapData.m_MapList[2];
        scale = tr.gameObject.GetComponent<TweenScale>();
        if (scale != null)
        {
            Destroy(scale);
        }

        tr = m_MapData.m_MapList[0];
        m_MapData.m_MapList.RemoveAt(0);
        m_MapData.m_MapList.Add(tr);
        tr.localPosition += m_MapData.m_MapList.Count * new Vector3(m_MapData.m_DisX, 0f, 0f);

        UpdateLevelInfo(m_MapData.m_MapList[m_MapData.m_MapList.Count - 1].GetComponent<UiMapData>(),
            m_MapData.m_CurLevel + m_MapData.m_MapList.Count - 1);
        m_MapData.m_CurLevel++;
        UpdateCurrentLevel(m_MapData.m_CurLevel);
        UpdateMapLevelLock();
    }

    void UpdateMapLevelLock()
    {
        for (int i = 0; i < m_MapData.m_MapList.Count; i++)
        {
            UiMapData dt = m_MapData.m_MapList[i].GetComponent<UiMapData>();
            if (dt != null)
            {
                if (i < 2)
                {
                    dt.SetActiveLocker(false);
                }
                else
                {
                    int dVal = m_MapData.m_UnlockMaxLevel - m_MapData.m_CurLevel;
                    if (dVal > 0)
                    {
                        dt.SetActiveLocker(false);
                    }
                    else
                    {
                        m_MapData.m_UnlockMaxLevel = m_MapData.m_CurLevel;
                        dt.SetActiveLocker(true);
                    }
                }

                bool isBoss = GetIsBossLevel(m_MapData.m_CurLevel - 1 + i);
                dt.SetActiveBoss(isBoss);
            }
        }
    }

    bool GetIsBossLevel(int level)
    {
        bool isBoss = false;
        if (IGamerProfile.Instance != null)
        {
            int indexLeval = level - 1;
            if (IGamerProfile.gameLevel.mapData.Length > indexLeval && indexLeval >= 0)
            {
                isBoss = IGamerProfile.gameLevel.mapData[indexLeval].bossData.BossType == "" ? false : true;
            }
        }
        return isBoss;
    }
}
