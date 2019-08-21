using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVirusGameManage : MonoBehaviour
{
    [System.Serializable]
    public class GameData
    {
        public GameObject FtGameSceneCtrl;
        public void Init()
        {
            if (VirusGloableData.Instance.m_VirusData.IsLoadingRootScene == true)
            {
                if (FtGameSceneCtrl != null)
                {
                    FtGameSceneCtrl.SetActive(true);
                }
            }
            else
            {
                CreateAudioListener();
            }
        }

        void CreateAudioListener()
        {
            GameObject obj = new GameObject("AudioListener");
            obj.AddComponent<AudioListener>();
        }
    }
    public GameData m_GameData;

    void Awake()
    {
        if (m_GameData != null)
        {
            m_GameData.Init();
        }
    }
}
