using System;
using System.Collections.Generic;
using UnityEngine;

class UiSceneSelectGameMap : GuiUiSceneBase
{
    public override int uiSceneId { get { return (int)UiSceneUICamera.UISceneId.Id_UIGameMap; } }

    //public GameObject processObject;
    public GameObject lockObject;
    //public GuiPlaneAnimationText curLevelNumber;
    //public GuiPlaneAnimationText maxLevelNumber;
    //public GuiPlaneAnimationProgressBar levelProgressBar;
    //public GameObject[] selectPosition;

    public GuiPlaneAnimationPlayer moveAni;
    Transform[] maplist;
    public GameObject mapUnit;
    public float mapDisX = 5.95f;

    public GameObject arrowsright;
    public GameObject arrowsleft;

    private int currentSelectMapIndex = -1;

    private GuiExtendButtonGroup buttonGroup = null;
    protected override void OnInitializationUI()
    {
        buttonGroup = GetComponent<GuiExtendButtonGroup>();
        buttonGroup.selectFuntion += OnButtonSelectOk;
        //先停止工作
        buttonGroup.IsWorkDo = false;
        QyFun.SetActive(lockObject, false);
        CreateGameMap();
        //定位到最后解锁的地图上
        SetCurrentSelectMapIndex(IGamerProfile.Instance.getLastLockedMap);
    }

    void CreateGameMap()
    {
        maplist = new Transform[IGamerProfile.gameLevel.mapData.Length];
        maplist[0] = mapUnit.transform;
        UiMapData dt = maplist[0].GetComponent<UiMapData>();
        dt.levelNum.Text = "1";
        dt.SetActiveLocker(false);
        dt.SetActiveBoss(IGamerProfile.gameLevel.mapData[0].bossData.BossType == "" ? false : true);

        int lastLockMapIndex = IGamerProfile.Instance.getLastLockedMap;
        for (int i = 1; i < IGamerProfile.gameLevel.mapData.Length; i++)
        {
            GameObject obj = Instantiate(mapUnit);
            obj.name = "map" + (i + 1).ToString();
            obj.transform.SetParent(mapUnit.transform.parent);
            Vector3 pos = mapUnit.transform.localPosition;
            pos.x = i * mapDisX;
            obj.transform.localPosition = pos;
            maplist[i] = obj.transform;
            dt = obj.GetComponent<UiMapData>();
            dt.levelNum.Text = (i + 1).ToString();
            dt.SetActiveLocker(lastLockMapIndex < i ? true : false);
            dt.SetActiveBoss(IGamerProfile.gameLevel.mapData[i].bossData.BossType == "" ? false : true);
        }
    }
    
    private void OnButtonSelectOk(int index)
    {
        SoundEffectPlayer.Play("buttonok.wav");
        //获取最后解锁的地图
        int lastLockMapIndex = IGamerProfile.Instance.getLastLockedMap;
        if (lastLockMapIndex < currentSelectMapIndex)
        {
            SetCurrentSelectMapIndex(lastLockMapIndex);
            return;
        }
        
        //设置当前地图
        IGamerProfile.Instance.gameEviroment.mapIndex = currentSelectMapIndex;
        IGamerProfile.Instance.gameEviroment.mapLevelIndex = IGamerProfile.Instance.playerdata.levelProcess[IGamerProfile.Instance.gameEviroment.mapIndex];
        if (IGamerProfile.Instance.gameEviroment.mapLevelIndex >= IGamerProfile.gameLevel.mapMaxLevel[IGamerProfile.Instance.gameEviroment.mapIndex])
        {
            IGamerProfile.Instance.gameEviroment.mapLevelIndex = IGamerProfile.gameLevel.mapMaxLevel[IGamerProfile.Instance.gameEviroment.mapIndex] - 1;
        }


        if ((GameCenterEviroment.platformChargeIntensity >= GameCenterEviroment.PlatformChargeIntensity.Intensity_VeryHigh) &&
                IGameCenterEviroment.effectCharacterLevelSale)
        {
            //优先升级副武器，其次升级主武器，最后升级收益属性
            int characterIndex = IGamerProfile.Instance.gameEviroment.characterIndex;
            UiSceneSelectGameCharacter.CharacterId id = (UiSceneSelectGameCharacter.CharacterId)characterIndex;
            UiSceneSelectGameCharacter.CharacterAttribute att = UiSceneSelectGameCharacter.GetUpgradeAttribute(id);
            //判断属性A是否需要升级
            bool isUpgrade = false;
            if (att != UiSceneSelectGameCharacter.CharacterAttribute.Null)
            {
                isUpgrade = UiSceneSelectGameCharacter.GetIsUpgradeCharacter(id, att);
            }

            if (isUpgrade == false)
            {
                //判断收益属性是否需要升级
                id = UiSceneSelectGameCharacter.CharacterId.ShouYi;
                att = UiSceneSelectGameCharacter.GetUpgradeAttribute(id);
                if (att != UiSceneSelectGameCharacter.CharacterAttribute.Null)
                {
                    isUpgrade = UiSceneSelectGameCharacter.GetIsUpgradeCharacter(id, att);
                }
            }

            if (isUpgrade == true)
            {
                //需要升级
                //让本身停止工作
                buttonGroup.IsWorkDo = false;
                GameObject obj = LoadResource_UIPrefabs("characterlevelsale.prefab");
                UiSceneCharacterLevelSale com = obj.GetComponent<UiSceneCharacterLevelSale>();
                if (com != null)
                {
                    UpgradeCharacterData dt = new UpgradeCharacterData(id, att);
                    //Debug.Log("dt ==> " + dt.ToString());
                    com.Init(dt);
                }
                return;
            }
        }
        IntoGame();
    }
    public class UpgradeCharacterData
    {
        public UiSceneSelectGameCharacter.CharacterId id;
        public UiSceneSelectGameCharacter.CharacterAttribute att;
        public UpgradeCharacterData(UiSceneSelectGameCharacter.CharacterId id, UiSceneSelectGameCharacter.CharacterAttribute att)
        {
            this.id = id;
            this.att = att;
        }
        public override string ToString()
        {
            return "id == " + id + ", att == " + att;
        }
    }
    private GuiPlaneAnimationPlayer tempMoveAni = null;
    private TweenScale tempScaleBigAni = null;
    private TweenScale tempScaleSmallAni = null;
    void MakeMapToBig(Transform mapTr)
    {
        TweenScale tween = mapTr.gameObject.AddComponent<TweenScale>();
        tween.from = mapTr.localScale;
        tween.to = new Vector3(0.8f, 0.8f, 1f);
        tween.duration = 0.3f;
        tween.AddOnFinished(OnScaleToBigAniPlayEnd);
        tween.PlayForward();
        tempScaleBigAni = tween;
    }
    void OnScaleToBigAniPlayEnd()
    {
        Destroy(tempScaleBigAni);
        tempScaleBigAni = null;
    }
    void MakeMapToSmall(Transform mapTr)
    {
        TweenScale tween = mapTr.gameObject.AddComponent<TweenScale>();
        tween.from = mapTr.localScale;
        tween.to = new Vector3(0.5f, 0.5f, 1f);
        tween.duration = 0.3f;
        tween.AddOnFinished(OnScaleToSmallAniPlayEnd);
        tween.PlayForward();
        tempScaleSmallAni = tween;
    }
    void OnScaleToSmallAniPlayEnd()
    {
        Destroy(tempScaleSmallAni);
        tempScaleSmallAni = null;
    }

    private void SetCurrentSelectMapIndex(int index)
    {
        if (currentSelectMapIndex == index)
            return;
        buttonGroup.IsWorkDo = false;

        MakeMapToBig(maplist[index]);
        if (currentSelectMapIndex > -1 && currentSelectMapIndex < maplist.Length)
        {
            MakeMapToSmall(maplist[currentSelectMapIndex]);
        }

        tempMoveAni = ((GameObject)UnityEngine.Object.Instantiate(moveAni.gameObject)).GetComponent<GuiPlaneAnimationPlayer>();
        tempMoveAni.transform.parent = this.transform;
        tempMoveAni.IsAutoDel = false;
        tempMoveAni.playMode = GuiPlaneAnimationPlayer.PlayMode.Mode_PlayOnec;
        tempMoveAni.DelegateOnPlayEndEvent += OnMoveAniPlayEnd;
        //增加关键帧
        GuiPlaneAnimationCurvePosition curvePosition = tempMoveAni.gameObject.GetComponentInChildren<GuiPlaneAnimationCurvePosition>();
        curvePosition.xCurve = UnityEngine.AnimationCurve.EaseInOut(0.0f, maplist[0].position.x,
                                1.0f, maplist[0].position.x - maplist[index].position.x);
        //将对象定位到开始坐标对象
        curvePosition.gameObject.transform.position = maplist[0].position;
        maplist[0].transform.parent.parent = curvePosition.gameObject.transform;
        //重新标记索引
        currentSelectMapIndex = index;
        //开始播放
        tempMoveAni.Play();
    }

    private void OnMoveAniPlayEnd()
    {
        //将地图对象移出来
        maplist[0].transform.parent.parent = tempMoveAni.transform.parent;
        UnityEngine.Object.DestroyObject(tempMoveAni.gameObject);
        tempMoveAni = null;
        //刷新当前选择属性
        UpdateCurrentSelectMapData(currentSelectMapIndex);
    }

    void StartShowLockObj()
    {
        if (lockObject != null && lockObject.activeInHierarchy == false)
        {
            StartCoroutine(ShowLockObj());
        }
    }

    System.Collections.IEnumerator ShowLockObj()
    {
        TweenScale scale = lockObject.AddComponent<TweenScale>();
        scale.from = Vector3.zero;
        scale.to = Vector3.one;
        scale.duration = 0.2f;
        scale.PlayForward();
        QyFun.SetActive(lockObject, true);
        yield return new WaitForSeconds(1f);
        QyFun.SetActive(lockObject, false);
        Destroy(scale);
    }

    private void UpdateCurrentSelectMapData(int index)
    {
        //获取最后解锁的地图
        int lastLockMapIndex = IGamerProfile.Instance.getLastLockedMap;
        if (index == 0)
        {
            arrowsright.SetActive(lastLockMapIndex > index ? true : false);
            arrowsleft.SetActive(false);
        }
        else if (index == IGamerProfile.gameLevel.mapData.Length - 1)
        {
            arrowsright.SetActive(false);
            arrowsleft.SetActive(true);
        }
        else
        {
            arrowsright.SetActive(lastLockMapIndex > index ? true : false);
            arrowsleft.SetActive(true);
        }

        buttonGroup.IsWorkDo = true;
    }

    //需要重载Input刷新函数
    //如果返回true,表示可以继续刷新后面的对象，否则刷新处理会被截断
    public override bool OnInputUpdate() 
    {
        if (InputDevice.ButtonLeft && buttonGroup.IsWorkDo)
        {
            SoundEffectPlayer.Play("scroll.wav");
            if (currentSelectMapIndex > 0)
            {
                SetCurrentSelectMapIndex(currentSelectMapIndex - 1);
            }
            else
            {
                StartShowLockObj();
            }
            return false;
        }
        else if (InputDevice.ButtonRight && buttonGroup.IsWorkDo)
        {
            SoundEffectPlayer.Play("scroll.wav");
            if (currentSelectMapIndex < maplist.Length - 1
                && currentSelectMapIndex < IGamerProfile.Instance.getLastLockedMap)
            {
                SetCurrentSelectMapIndex(currentSelectMapIndex + 1);
            }
            else
            {
                StartShowLockObj();
            }
            return false;
        }
        else if (InputDevice.ButtonBack)
        {
            SoundEffectPlayer.Play("buttonok.wav");
            //闪白
            ((UiSceneUICamera)UIManager).FadeScreen();
            //进入角色选择界面
            ((UiSceneUICamera)UIManager).CreateAloneScene(UiSceneUICamera.UISceneId.Id_UIGameCharacter);
            return false;
        }
        return true; 
    }

    //一个子UI删除
    public override void OnChildUIRelease(GuiUiSceneBase ui)
    {
        if (ui is UiSceneCharacterLevelSale)
        {
            IntoGame();
        }
    }


    public void IntoGame()
    {
        //闪白
        //((UiSceneUICamera)UIManager).FadeScreen();
        ////设置为进入游戏模式
        //UiSceneSelectGameCharacter.selectCharacterMode = UiSceneSelectGameCharacter.SelectCharacterMode.Mode_IntoGame;
        ////进入角色选择界面
        //((UiSceneUICamera)UIManager).CreateAloneScene(UiSceneUICamera.UISceneId.Id_UIGameCharacter);
        //进入加载界面
        ((UiSceneUICamera)UIManager).CreatePoolingScene(UiSceneUICamera.UISceneId.Id_UIGameLoading, UiSceneGameLoading.LoadingType.Type_LoadingGameNew);
        //删除主UI界面
        ((UiSceneUICamera)UIManager).ReleaseAloneScene();
    }
}
