using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QySetTransform : QyRoot
{
    [System.Serializable]
    public class TranData
    {
        public bool IsEnable = false;
        public bool IsAllChild = false;
        public float offDisX = 0f;
        public string nameHead = "";
        public Transform[] trArray;

        public void TestTr(Transform parentTr)
        {
            if (IsEnable == false)
            {
                return;
            }
            IsEnable = false;

            if (IsAllChild == true && parentTr != null)
            {
                //List<Transform> list = new List<Transform>(parentTr.GetComponentsInChildren<Transform>());
                //list.RemoveAt(0);
                //trArray = list.ToArray();
                trArray = new Transform[parentTr.childCount];
                for (int i = 0; i < trArray.Length; i++)
                {
                    trArray[i] = parentTr.GetChild(i);
                }
            }
            Debug.Log("count ======== " + parentTr.childCount);

            if (trArray.Length < 1)
            {
                return;
            }

            Vector3 startPos = Vector3.zero;
            float indexStartX = -0.5f * (trArray.Length - 1);
            startPos.x = offDisX * indexStartX;
            trArray[0].localPosition = startPos;

            float startX = trArray[0].localPosition.x;
            for (int i = 1; i < trArray.Length; i++)
            {
                Vector3 lp = Vector3.zero;
                lp.x = startX + i * offDisX;
                trArray[i].localPosition = lp;
            }

            if (nameHead != "")
            {
                for (int i = 0; i < trArray.Length; i++)
                {
                    trArray[i].name = nameHead + (i + 1).ToString();
                }
            }
        }
    }
    public TranData m_TranData;

    [System.Serializable]
    public class MatData
    {
        public bool IsEnable = false;
        public Material[] matArray;
        public void Init()
        {
            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i].mainTextureOffset = Vector2.zero;
                matArray[i].mainTextureScale = Vector2.one;
            }
        }

        public void TestMat()
        {
            if (IsEnable == false)
            {
                return;
            }
            IsEnable = false;

            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i].mainTextureOffset = new Vector2(0.8f, 0f);
                matArray[i].mainTextureScale = new Vector2(0.1f, 1f);
            }
        }
    }
    public MatData m_MatData;

    void Awake()
    {
        m_MatData.Init();
        Destroy(this);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (m_MatData != null)
        {
            m_MatData.TestMat();
        }

        if (m_TranData != null)
        {
            m_TranData.TestTr(transform);
        }
    }
#endif
}
