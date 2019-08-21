using System;
using System.Collections.Generic;
using UnityEngine;
class UiSceneSelectGameCharacter : GuiUiSceneBase
{
    public override int uiSceneId { get { return (int)UiSceneUICamera.UISceneId.Id_UIGameCharacter; } }

    public enum SelectCharacterMode
    {
        Mode_IntoGame,
        Mode_RebackGame,
        Mode_NextGame,
    }
    public static SelectCharacterMode selectCharacterMode = SelectCharacterMode.Mode_IntoGame;

    //射速和火力的当前等级与属性、升级需要金币数量
    //一共需要设置7个武器配置数据,1号武器当做主武器,2-6号武器当做副武器,7号武器当做收益数据信息
    public GuiPlaneAnimationTextRoll playerMoney = null;
    
    public GameObject btn_intogame;
    /// <summary>
    /// 主武器按键
    /// </summary>
    public GameObject btn_mainWeapon;
    public GameObject btn_mainWeaponActive;
    /// <summary>
    /// 主武器付费按键
    /// </summary>
    public GameObject btn_mainWeaponMoneyA;
    public GameObject btn_mainWeaponMoneyB;
    public GameObject mainWeaponChild;
    /// <summary>
    /// 副武器按键
    /// </summary>
    public GameObject btn_secondaryWeapon;
    public GameObject btn_secondaryWeaponActive;
    public GameObject btn_secondaryWeaponChild01;
    public GameObject btn_secondaryWeaponChild02;
    public GameObject btn_secondaryWeaponChild03;
    public GameObject btn_secondaryWeaponChild04;
    public GameObject btn_secondaryWeaponChild05;
    public GameObject btn_secondaryWeaponChild06;
    public GameObject btn_secondaryWeaponChild07;
    public GameObject btn_secondaryWeaponChild08;
    GameObject[] btn_secondaryWeaponSelectArray;
    /// <summary>
    /// 副武器付费按键
    /// </summary>
    public GameObject btn_secondaryWeaponMoneyA;
    public GameObject btn_secondaryWeaponMoneyB;
    public GameObject secondaryWeaponChild;
    /// <summary>
    /// 收益按键
    /// </summary>
    public GameObject btn_income;
    public GameObject btn_incomeActive;
    /// <summary>
    /// 收益付费按键
    /// </summary>
    public GameObject btn_incomeMoneyA;
    public GameObject btn_incomeMoneyB;
    public GameObject incomeChild;
    public GameObject[] m_PlayerArray;

    /// <summary>
    /// 用来标记最后一次选择的副武器信息.
    /// 0 - 主武器, [1 - 8] - [1 - 8号副武器]
    /// </summary>
    private int currentSelectGunIndex = 0;
    
    private enum ButtonId_NotActiveCharacter
    {
        Id_ActiveCharacter = 0,
        Id_IntoGame = 1,
        ButtonCount = 2,
    }
    private enum ButtonId_ActiveCharacter
    {
        Id_LevelCharacter = 1,
        Id_IntoGame = 0,
        ButtonCount = 2,
    }
    private enum ButtonId_MaxLevel
    {
        Id_IntoGame = 0,
        ButtonCount = 1,
    }

    private enum ButtonId_MainUI
    {
        Null = -1,
        StartGame = 0,
        MainWeapon = 1,
        SecondaryWeapon = 2,
        Income = 3,
        BtCount = 4,
    }
    ButtonId_MainUI m_ButtonId_MainUI = ButtonId_MainUI.Null;

    private enum ButtonId_ChildUI
    {
        Null = -1,
        MainBt = 0,
        FuFeiBt = 1,
        BtCount = 2,
    }
    ButtonId_ChildUI m_ButtonId_ChildUI = ButtonId_ChildUI.Null;

    private enum ButtonId_SecondaryWeaponUD
    {
        Null = -1,
        MainBt = 0,
        Weapon = 1,
        FuFeiBt = 2,
        BtCount = 2,
    }
    ButtonId_SecondaryWeaponUD m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.Null;

    public enum ButtonId_SecondaryWeaponLR
    {
        Null = -1,
        Weapon01 = 0,
        Weapon02 = 1,
        Weapon03 = 2,
        Weapon04 = 3,
        Weapon05 = 4,
        Weapon06 = 5,
        Weapon07 = 6,
        Weapon08 = 7,
        BtCount = 8,
    }
    ButtonId_SecondaryWeaponLR m_ButtonId_SecondaryWeaponLR = ButtonId_SecondaryWeaponLR.Null;

    CharacterId SecondaryWeaponLRToCharacterId(ButtonId_SecondaryWeaponLR lr)
    {
        if (lr == ButtonId_SecondaryWeaponLR.Null || lr == ButtonId_SecondaryWeaponLR.BtCount)
        {
            return CharacterId.Null;
        }
        return (CharacterId)((int)lr + 1);
    }

    ButtonId_SecondaryWeaponLR CharacterIdToSecondaryWeaponLR(CharacterId id)
    {
        if (id == CharacterId.Null
            || id == CharacterId.MainWeapon
            || id == CharacterId.ShouYi)
        {
            return ButtonId_SecondaryWeaponLR.Null;
        }
        return (ButtonId_SecondaryWeaponLR)((int)id - 1);
    }

    void InitPanel()
    {
        //TestActiveAllChildWeapon(); //test

        btn_secondaryWeaponSelectArray = new GameObject[8];
        btn_secondaryWeaponSelectArray[0] = btn_secondaryWeaponChild01.transform.GetChild(0).gameObject;
        btn_secondaryWeaponSelectArray[1] = btn_secondaryWeaponChild02.transform.GetChild(0).gameObject;
        btn_secondaryWeaponSelectArray[2] = btn_secondaryWeaponChild03.transform.GetChild(0).gameObject;
        btn_secondaryWeaponSelectArray[3] = btn_secondaryWeaponChild04.transform.GetChild(0).gameObject;
        btn_secondaryWeaponSelectArray[4] = btn_secondaryWeaponChild05.transform.GetChild(0).gameObject;
        btn_secondaryWeaponSelectArray[5] = btn_secondaryWeaponChild06.transform.GetChild(0).gameObject;
        btn_secondaryWeaponSelectArray[6] = btn_secondaryWeaponChild07.transform.GetChild(0).gameObject;
        btn_secondaryWeaponSelectArray[7] = btn_secondaryWeaponChild08.transform.GetChild(0).gameObject;

        m_SelectChildWeaponId = (CharacterId)IGamerProfile.Instance.getLastActiveChildCharacter;
        ShowCharacter(m_SelectChildWeaponId);

        m_ButtonId_MainUI = ButtonId_MainUI.StartGame;
        QyFun.SetActive(btn_mainWeaponActive, false);
        btn_secondaryWeaponActive.SetActive(false);
        btn_incomeActive.SetActive(false);
        mainWeaponChild.SetActive(false);
        secondaryWeaponChild.SetActive(false);
        incomeChild.SetActive(false);
    }

    private GuiExtendButtonGroup buttonGroup = null;

    //是否没有操作
    //private bool isNotHandle = true;

    protected override void OnInitializationUI()
    {
        InitPanel();
        playerMoney.SetIntegerRollValue(0, true);
        playerMoney.SetIntegerRollValue(IGamerProfile.Instance.playerdata.playerMoney);
        
        buttonGroup = GetComponent<GuiExtendButtonGroup>();
        buttonGroup.selectFuntion += OnButtonSelectOk;
        buttonGroup.onButtonUp += OnButtonUp;
        buttonGroup.onButtonDown += OnButtonDown;
        buttonGroup.onButtonLeft += OnButtonLeft;
        buttonGroup.onButtonRight += OnButtonRight;

        buttonGroup.buttonList = new GameObject[(int)ButtonId_MainUI.BtCount];
        buttonGroup.selectAnchorList = new GuiAnchorObject[(int)ButtonId_MainUI.BtCount];
        buttonGroup.buttonList[(int)ButtonId_MainUI.StartGame] = btn_intogame;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.StartGame] = btn_intogame.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_MainUI.MainWeapon] = btn_mainWeapon;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.MainWeapon] = btn_mainWeapon.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_MainUI.SecondaryWeapon] = btn_secondaryWeapon;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.SecondaryWeapon] = btn_secondaryWeapon.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_MainUI.Income] = btn_income;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.Income] = btn_income.GetComponent<GuiAnchorObject>();
        buttonGroup.selectMode = GuiExtendButtonGroup.ButtonSelectMode.Mode_LeftRight;
        buttonGroup.CurrentSelectButtonIndex = (int)ButtonId_MainUI.StartGame;
        buttonGroup.IsWorkDo = true;
    }

    //需要重载Input刷新函数
    //如果返回true,表示可以继续刷新后面的对象，否则刷新处理会被截断
    public override bool OnInputUpdate()
    {
        if (InputDevice.ButtonBack)
        {
            if (selectCharacterMode == SelectCharacterMode.Mode_IntoGame ||
                    selectCharacterMode == SelectCharacterMode.Mode_RebackGame)
            {
                SoundEffectPlayer.Play("buttonok.wav");
                //闪白
                ((UiSceneUICamera)UIManager).FadeScreen();
                //进入角色选择界面
                ((UiSceneUICamera)UIManager).CreateAloneScene(UiSceneUICamera.UISceneId.Id_UIGameStart);
                return false;
            }

        }
        return true;
    }

    private enum BtState
    {
        Null = -1,
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }

    void OnButtonUpDown(BtState btState, int index)
    {
        //Debug.Log("Unity: OnButtonUpDown -> btState == " + btState + ", index == " + index);
        SoundEffectPlayer.Play("scroll.wav");
        if (m_ButtonId_MainUI == ButtonId_MainUI.SecondaryWeapon)
        {
            if (m_ButtonId_SecondaryWeaponUD == ButtonId_SecondaryWeaponUD.Null)
            {
                if (m_ButtonId_SecondaryWeaponLR == ButtonId_SecondaryWeaponLR.Null)
                {
                    m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.MainBt;
                }
                else
                {
                    m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.Weapon;
                }
                m_ButtonId_SecondaryWeaponLR = ButtonId_SecondaryWeaponLR.Null; //重置副武器左右按键状态
                FillUDButtons();
            }
            UpdataButtonIdSecondaryWeaponUD(btState);
        }

        //if (m_ButtonId_MainUI == ButtonId_MainUI.MainWeapon || m_ButtonId_MainUI == ButtonId_MainUI.Income)
        //{
        //    if (m_ButtonId_ChildUI == ButtonId_ChildUI.Null && btState == BtState.Down)
        //    {
        //        m_ButtonId_ChildUI = ButtonId_ChildUI.MainBt;
        //        FillUDButtons();
        //    }

        //    if (m_ButtonId_ChildUI != ButtonId_ChildUI.Null)
        //    {
        //        switch (btState)
        //        {
        //            case BtState.Down:
        //                {
        //                    UpdateBtChildUIState(ButtonId_ChildUI.FuFeiBt);
        //                    break;
        //                }
        //            case BtState.Up:
        //                {
        //                    UpdateBtChildUIState(ButtonId_ChildUI.MainBt);
        //                    break;
        //                }
        //        }
        //    }
        //}
    }

    /// <summary>
    /// 刷新副武器上下按键状态信息
    /// </summary>
    void UpdataButtonIdSecondaryWeaponUD(BtState btState)
    {
        switch (m_ButtonId_SecondaryWeaponUD)
        {
            case ButtonId_SecondaryWeaponUD.MainBt:
                {
                    if (btState == BtState.Down)
                    {
                        m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.Weapon;
                    }
                    break;
                }
            case ButtonId_SecondaryWeaponUD.Weapon:
                {
                    if (btState == BtState.Up)
                    {
                        m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.MainBt;
                    }
                    //if (btState == BtState.Down)
                    //{
                    //    m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.FuFeiBt;
                    //}
                    //else if (btState == BtState.Up)
                    //{
                    //    m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.MainBt;
                    //}
                    break;
                }
            //case ButtonId_SecondaryWeaponUD.FuFeiBt:
            //    {
            //        if (btState == BtState.Up)
            //        {
            //            m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.Weapon;
            //        }
            //        break;
            //    }
        }
    }

    void UpdateBtChildUIState(ButtonId_ChildUI btState)
    {
        switch (m_ButtonId_MainUI)
        {
            case ButtonId_MainUI.MainWeapon:
            case ButtonId_MainUI.Income:
                {
                    UpdateMainWeaponIncomeBtChildState(btState);
                    break;
                }
        }
    }

    void UpdateMainWeaponIncomeBtChildState(ButtonId_ChildUI btState)
    {
        m_ButtonId_ChildUI = btState;
    }

    /// <summary>
    /// 填充按键
    /// </summary>
    void FillUDButtons()
    {
        switch (m_ButtonId_MainUI)
        {
            //case ButtonId_MainUI.MainWeapon:
            //    {
            //        FillUDMainWeaponBts();
            //        break;
            //    }
            case ButtonId_MainUI.SecondaryWeapon:
                {
                    FillUDSecondaryWeaponBts();
                    break;
                }
            //case ButtonId_MainUI.Income:
            //    {
            //        FillUDIncomeWeaponBts();
            //        break;
            //    }
        }
    }

    /// <summary>
    /// 填充主武器界面按键
    /// </summary>
    void FillUDMainWeaponBts()
    {
        buttonGroup.buttonList = new GameObject[(int)ButtonId_ChildUI.BtCount];
        buttonGroup.selectAnchorList = new GuiAnchorObject[(int)ButtonId_ChildUI.BtCount];
        buttonGroup.buttonList[(int)ButtonId_ChildUI.MainBt] = btn_mainWeapon;
        buttonGroup.selectAnchorList[(int)ButtonId_ChildUI.MainBt] = btn_mainWeapon.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_ChildUI.FuFeiBt] = btn_mainWeaponMoneyA;
        buttonGroup.selectAnchorList[(int)ButtonId_ChildUI.FuFeiBt] = btn_mainWeaponMoneyA.GetComponent<GuiAnchorObject>();
        buttonGroup.selectMode = GuiExtendButtonGroup.ButtonSelectMode.Mode_UpDown;
        buttonGroup.CurrentSelectButtonIndex = (int)ButtonId_ChildUI.MainBt;
    }

    /// <summary>
    /// 填充副武器界面上下按键
    /// </summary>
    void FillUDSecondaryWeaponBts()
    {
        buttonGroup.buttonList = new GameObject[(int)ButtonId_SecondaryWeaponUD.BtCount];
        buttonGroup.selectAnchorList = new GuiAnchorObject[(int)ButtonId_SecondaryWeaponUD.BtCount];
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponUD.MainBt] = btn_secondaryWeapon;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponUD.MainBt] = btn_secondaryWeapon.GetComponent<GuiAnchorObject>();
        GameObject obj = GetSecondaryWeaponChild(m_SelectChildWeaponId);
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponUD.Weapon] = obj;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponUD.Weapon] = obj.GetComponent<GuiAnchorObject>();
        //buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponUD.FuFeiBt] = btn_secondaryWeaponMoney;
        //buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponUD.FuFeiBt] = btn_secondaryWeaponMoney.GetComponent<GuiAnchorObject>();
        buttonGroup.selectMode = GuiExtendButtonGroup.ButtonSelectMode.Mode_UpDown;
        buttonGroup.CurrentSelectButtonIndex = (int)m_ButtonId_SecondaryWeaponUD;
    }

    GameObject GetSecondaryWeaponChild(CharacterId id)
    {
        GameObject obj = null;
        switch (id)
        {
            case CharacterId.ChildWeapon01:
                {
                    obj = btn_secondaryWeaponChild01;
                    break;
                }
            case CharacterId.ChildWeapon02:
                {
                    obj = btn_secondaryWeaponChild02;
                    break;
                }
            case CharacterId.ChildWeapon03:
                {
                    obj = btn_secondaryWeaponChild03;
                    break;
                }
            case CharacterId.ChildWeapon04:
                {
                    obj = btn_secondaryWeaponChild04;
                    break;
                }
            case CharacterId.ChildWeapon05:
                {
                    obj = btn_secondaryWeaponChild05;
                    break;
                }
            case CharacterId.ChildWeapon06:
                {
                    obj = btn_secondaryWeaponChild06;
                    break;
                }
            case CharacterId.ChildWeapon07:
                {
                    obj = btn_secondaryWeaponChild07;
                    break;
                }
            case CharacterId.ChildWeapon08:
                {
                    obj = btn_secondaryWeaponChild08;
                    break;
                }
            default:
                {
                    obj = btn_secondaryWeaponChild01;
                    break;
                }
        }
        return obj;
    }

    /// <summary>
    /// 填充收益界面按键
    /// </summary>
    void FillUDIncomeWeaponBts()
    {
        buttonGroup.buttonList = new GameObject[(int)ButtonId_ChildUI.BtCount];
        buttonGroup.selectAnchorList = new GuiAnchorObject[(int)ButtonId_ChildUI.BtCount];
        buttonGroup.buttonList[(int)ButtonId_ChildUI.MainBt] = btn_income;
        buttonGroup.selectAnchorList[(int)ButtonId_ChildUI.MainBt] = btn_income.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_ChildUI.FuFeiBt] = btn_incomeMoneyA;
        buttonGroup.selectAnchorList[(int)ButtonId_ChildUI.FuFeiBt] = btn_incomeMoneyA.GetComponent<GuiAnchorObject>();
        buttonGroup.selectMode = GuiExtendButtonGroup.ButtonSelectMode.Mode_UpDown;
        buttonGroup.CurrentSelectButtonIndex = (int)ButtonId_ChildUI.MainBt;
    }

    /// <summary>
    /// 填充主界面按键
    /// </summary>
    void FillLRMainPanelBts()
    {
        buttonGroup.buttonList = new GameObject[(int)ButtonId_MainUI.BtCount];
        buttonGroup.selectAnchorList = new GuiAnchorObject[(int)ButtonId_MainUI.BtCount];
        buttonGroup.buttonList[(int)ButtonId_MainUI.StartGame] = btn_intogame;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.StartGame] = btn_intogame.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_MainUI.MainWeapon] = btn_mainWeapon;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.MainWeapon] = btn_mainWeapon.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_MainUI.SecondaryWeapon] = btn_secondaryWeapon;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.SecondaryWeapon] = btn_secondaryWeapon.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_MainUI.Income] = btn_income;
        buttonGroup.selectAnchorList[(int)ButtonId_MainUI.Income] = btn_income.GetComponent<GuiAnchorObject>();
        buttonGroup.selectMode = GuiExtendButtonGroup.ButtonSelectMode.Mode_LeftRight;
        buttonGroup.CurrentSelectButtonIndex = (int)m_ButtonId_MainUI;
    }
    
    /// <summary>
    /// 填充副武器界面左右按键
    /// </summary>
    void FillLRSecondaryWeaponBts()
    {
        buttonGroup.buttonList = new GameObject[(int)ButtonId_SecondaryWeaponLR.BtCount];
        buttonGroup.selectAnchorList = new GuiAnchorObject[(int)ButtonId_SecondaryWeaponLR.BtCount];
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon01] = btn_secondaryWeaponChild01;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon01] = btn_secondaryWeaponChild01.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon02] = btn_secondaryWeaponChild02;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon02] = btn_secondaryWeaponChild02.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon03] = btn_secondaryWeaponChild03;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon03] = btn_secondaryWeaponChild03.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon04] = btn_secondaryWeaponChild04;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon04] = btn_secondaryWeaponChild04.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon05] = btn_secondaryWeaponChild05;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon05] = btn_secondaryWeaponChild05.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon06] = btn_secondaryWeaponChild06;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon06] = btn_secondaryWeaponChild06.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon07] = btn_secondaryWeaponChild07;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon07] = btn_secondaryWeaponChild07.GetComponent<GuiAnchorObject>();
        buttonGroup.buttonList[(int)ButtonId_SecondaryWeaponLR.Weapon08] = btn_secondaryWeaponChild08;
        buttonGroup.selectAnchorList[(int)ButtonId_SecondaryWeaponLR.Weapon08] = btn_secondaryWeaponChild08.GetComponent<GuiAnchorObject>();
        buttonGroup.selectMode = GuiExtendButtonGroup.ButtonSelectMode.Mode_LeftRight;
        if (m_SelectChildWeaponId == CharacterId.Null)
        {
            buttonGroup.CurrentSelectButtonIndex = (int)ButtonId_SecondaryWeaponLR.Weapon01;
        }
        else
        {
            buttonGroup.CurrentSelectButtonIndex = (int)CharacterIdToSecondaryWeaponLR(m_SelectChildWeaponId);
        }
    }

    /// <summary>
    /// 更新副武器左右状态标记
    /// </summary>
    void UpdateButtonIdSecondaryWeaponLR(BtState btState)
    {
        if (m_ButtonId_SecondaryWeaponLR != ButtonId_SecondaryWeaponLR.Null)
        {
            bool isUpdateChildWeaponId = false;
            switch (m_ButtonId_SecondaryWeaponLR)
            {
                case ButtonId_SecondaryWeaponLR.Weapon01:
                    {
                        if (btState == BtState.Right)
                        {
                            m_ButtonId_SecondaryWeaponLR = ButtonId_SecondaryWeaponLR.Weapon02;
                            isUpdateChildWeaponId = true;
                        }
                        break;
                    }
                case ButtonId_SecondaryWeaponLR.Weapon02:
                case ButtonId_SecondaryWeaponLR.Weapon03:
                case ButtonId_SecondaryWeaponLR.Weapon04:
                case ButtonId_SecondaryWeaponLR.Weapon05:
                case ButtonId_SecondaryWeaponLR.Weapon06:
                case ButtonId_SecondaryWeaponLR.Weapon07:
                    {
                        if (btState == BtState.Right)
                        {
                            m_ButtonId_SecondaryWeaponLR = (ButtonId_SecondaryWeaponLR)((int)m_ButtonId_SecondaryWeaponLR + 1);
                            isUpdateChildWeaponId = true;
                        }
                        else if (btState == BtState.Left)
                        {
                            m_ButtonId_SecondaryWeaponLR = (ButtonId_SecondaryWeaponLR)((int)m_ButtonId_SecondaryWeaponLR - 1);
                            isUpdateChildWeaponId = true;
                        }
                        break;
                    }
                case ButtonId_SecondaryWeaponLR.Weapon08:
                    {
                        if (btState == BtState.Left)
                        {
                            m_ButtonId_SecondaryWeaponLR = ButtonId_SecondaryWeaponLR.Weapon07;
                            isUpdateChildWeaponId = true;
                        }
                        break;
                    }
            }

            CharacterId id = SecondaryWeaponLRToCharacterId(m_ButtonId_SecondaryWeaponLR);
            if (isUpdateChildWeaponId == true && true == GetIsActiveCharacter(id))
            {
                //只有激活的副武器才可以进入
                m_SelectChildWeaponId = id;
                ShowCharacter(id);
            }
            UpdateFuWeaponData(id);
        }
    }

    void OnButtonLeftRight(BtState btState, int index)
    {
        SoundEffectPlayer.Play("scroll.wav");
        if (m_ButtonId_MainUI == ButtonId_MainUI.MainWeapon || m_ButtonId_MainUI == ButtonId_MainUI.Income)
        {
            //主武器和收益界面
            if (m_ButtonId_ChildUI == ButtonId_ChildUI.MainBt)
            {
                //子菜单停留在主界面按键,填充左右按键.
                m_ButtonId_ChildUI = ButtonId_ChildUI.Null;
                index = (int)m_ButtonId_MainUI; //强制修改索引
                FillLRMainPanelBts();
            }
            else if (m_ButtonId_ChildUI == ButtonId_ChildUI.FuFeiBt)
            {
                return;
            }
        }

        if (m_ButtonId_MainUI == ButtonId_MainUI.SecondaryWeapon)
        {
            //副武器界面
            if (m_ButtonId_SecondaryWeaponUD == ButtonId_SecondaryWeaponUD.MainBt)
            {
                //子菜单停留在主界面,填充左右按键.
                m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.Null;
                index = (int)m_ButtonId_MainUI; //强制修改索引
                FillLRMainPanelBts();
            }
            else if (m_ButtonId_SecondaryWeaponUD == ButtonId_SecondaryWeaponUD.Weapon)
            {
                //子菜单停留在副武器,填充副武器的左右按键
                m_ButtonId_SecondaryWeaponUD = ButtonId_SecondaryWeaponUD.Null;
                if (m_SelectChildWeaponId == CharacterId.Null)
                {
                    m_ButtonId_SecondaryWeaponLR = ButtonId_SecondaryWeaponLR.Weapon01;
                }
                else
                {
                    m_ButtonId_SecondaryWeaponLR = CharacterIdToSecondaryWeaponLR(m_SelectChildWeaponId);
                }
                FillLRSecondaryWeaponBts();
            }
            else if (m_ButtonId_SecondaryWeaponUD == ButtonId_SecondaryWeaponUD.FuFeiBt)
            {
                return;
            }

            if (m_ButtonId_SecondaryWeaponLR != ButtonId_SecondaryWeaponLR.Null)
            {
                UpdateButtonIdSecondaryWeaponLR(btState);
                return;
            }
        }

        ButtonId_MainUI mainUI = (ButtonId_MainUI)index;
        //Debug.Log("Unity: OnButtonLeftRight -> btState == " + btState + ", index == " + index);
        switch (mainUI)
        {
            case ButtonId_MainUI.StartGame:
                {
                    if (btState == BtState.Right)
                    {
                        ShowMainUIPanel(ButtonId_MainUI.MainWeapon);
                    }
                    break;
                }
            case ButtonId_MainUI.MainWeapon:
                {
                    if (btState == BtState.Right)
                    {
                        ShowMainUIPanel(ButtonId_MainUI.SecondaryWeapon);
                    }
                    else if (btState == BtState.Left)
                    {
                        ShowMainUIPanel(ButtonId_MainUI.StartGame);
                    }
                    break;
                }
            case ButtonId_MainUI.SecondaryWeapon:
                {
                    if (btState == BtState.Right)
                    {
                        ShowMainUIPanel(ButtonId_MainUI.Income);
                    }
                    else if (btState == BtState.Left)
                    {
                        ShowMainUIPanel(ButtonId_MainUI.MainWeapon);
                    }
                    break;
                }
            case ButtonId_MainUI.Income:
                {
                    if (btState == BtState.Left)
                    {
                        ShowMainUIPanel(ButtonId_MainUI.SecondaryWeapon);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 显示主界面
    /// </summary>
    void ShowMainUIPanel(ButtonId_MainUI mainUI)
    {
        m_ButtonId_MainUI = mainUI;
        SetActiveMainWeaponPanel(mainUI == ButtonId_MainUI.MainWeapon);
        SetActiveSecondaryWeaponPanel(mainUI == ButtonId_MainUI.SecondaryWeapon);
        SetActiveIncomePanel(mainUI == ButtonId_MainUI.Income);
    }

    /// <summary>
    /// 主武器界面
    /// </summary>
    void SetActiveMainWeaponPanel(bool isActive)
    {
        if (mainWeaponChild != null)
        {
            mainWeaponChild.SetActive(isActive);
        }

        if (isActive == true)
        {
            InitMainWeaponData();
        }
        else
        {
            QyFun.SetActive(btn_mainWeaponActive, false);
            ResetMainWeaponData();
        }
    }

    /// <summary>
    /// 副武器界面
    /// </summary>
    void SetActiveSecondaryWeaponPanel(bool isActive)
    {
        if (secondaryWeaponChild != null)
        {
            secondaryWeaponChild.SetActive(isActive);
        }

        if (isActive == true)
        {
            InitFuWeaponData();
        }
        else
        {
            QyFun.SetActive(btn_secondaryWeaponActive, false);
            ResetFuWeaponData();
        }
    }

    /// <summary>
    /// 收益界面
    /// </summary>
    void SetActiveIncomePanel(bool isActive)
    {
        if (incomeChild != null)
        {
            incomeChild.SetActive(isActive);
        }

        if (isActive == true)
        {
            InitShouYiData();
        }
        else
        {
            QyFun.SetActive(btn_incomeActive, false);
            ResetShouYiData();
        }
    }

    private void OnButtonUp(int index)
    {
        OnButtonUpDown(BtState.Up, index);
    }

    private void OnButtonDown(int index)
    {
        OnButtonUpDown(BtState.Down, index);
    }

    private void OnButtonLeft(int index)
    {
        OnButtonLeftRight(BtState.Left, index);
    }

    private void OnButtonRight(int index)
    {
        OnButtonLeftRight(BtState.Right, index);
    }
    
    public enum CharacterId
    {
        Null = -1,
        MainWeapon = 0,
        ChildWeapon01 = 1,
        ChildWeapon02 = 2,
        ChildWeapon03 = 3,
        ChildWeapon04 = 4,
        ChildWeapon05 = 5,
        ChildWeapon06 = 6,
        ChildWeapon07 = 7,
        ChildWeapon08 = 8,
        ChildWeaponCount = 8,
        ShouYi = 9,
    }
    /// <summary>
    /// 升级对象标记
    /// </summary>
    CharacterId m_UpgradeCharacterId = CharacterId.Null;
    CharacterId _SelectChildWeaponId = CharacterId.Null;
    /// <summary>
    /// 选择的副武器标记
    /// </summary>
    CharacterId m_SelectChildWeaponId
    {
        set
        {
            _SelectChildWeaponId = value;
            ShowFuWeaponSelectImg();
        }
        get
        {
            return _SelectChildWeaponId;
        }
    }

    public enum CharacterAttribute
    {
        Null = -1,
        AttributeA = 0,
        AttributeB = 1,
    }
    /// <summary>
    /// 升级对象属性标记
    /// </summary>
    CharacterAttribute m_UpgradeAttribute = CharacterAttribute.Null;
    /// <summary>
    /// 获取需要升级的属性
    /// </summary>
    public static CharacterAttribute GetUpgradeAttribute(CharacterId id)
    {
        if (id == CharacterId.Null)
        {
            return CharacterAttribute.Null;
        }

        int indexVal = (int)id;
        IGamerProfile.PlayerData.PlayerChacterData dt = IGamerProfile.Instance.playerdata.characterData[indexVal];
        int lvA = dt.levelA;
        int lvAMax = IGamerProfile.gameCharacter.characterDataList[indexVal].maxlevelA;
        int lvB = dt.levelB;
        int lvBMax = IGamerProfile.gameCharacter.characterDataList[indexVal].maxlevelB;
        if (lvA >= lvAMax && lvB >= lvBMax)
        {
            return CharacterAttribute.Null;
        }

        CharacterAttribute att;
        if (lvB < lvA)
        {
            if (lvB < lvBMax)
            {
                att = CharacterAttribute.AttributeB;
            }
            else
            {
                att = CharacterAttribute.AttributeA;
            }
        }
        else
        {
            if (lvA < lvAMax)
            {
                att = CharacterAttribute.AttributeA;
            }
            else
            {
                att = CharacterAttribute.AttributeB;
            }
        }
        return att;
    }

    void UpdateUpgradeAttribute(CharacterId id)
    {
        if (id == CharacterId.Null)
        {
            return;
        }

        int indexVal = (int)id;
        IGamerProfile.PlayerData.PlayerChacterData dt = IGamerProfile.Instance.playerdata.characterData[indexVal];
        int lvA = dt.levelA;
        int lvAMax = IGamerProfile.gameCharacter.characterDataList[indexVal].maxlevelA;
        int lvB = dt.levelB;
        int lvBMax = IGamerProfile.gameCharacter.characterDataList[indexVal].maxlevelB;
        if (lvB < lvA)
        {
            if (lvB < lvBMax)
            {
                m_UpgradeAttribute = CharacterAttribute.AttributeB;
            }
            else
            {
                m_UpgradeAttribute = CharacterAttribute.AttributeA;
            }
        }
        else
        {
            if (lvA < lvAMax)
            {
                m_UpgradeAttribute = CharacterAttribute.AttributeA;
            }
            else
            {
                m_UpgradeAttribute = CharacterAttribute.AttributeB;
            }
        }

        switch (id)
        {
            case CharacterId.MainWeapon:
                {
                    m_MainWeaponData.SetSelectAttribute(m_UpgradeAttribute);
                    break;
                }
            case CharacterId.ShouYi:
                {
                    m_ShouYiData.SetSelectAttribute(m_UpgradeAttribute);
                    break;
                }
            case CharacterId.ChildWeapon01:
            case CharacterId.ChildWeapon02:
            case CharacterId.ChildWeapon03:
            case CharacterId.ChildWeapon04:
            case CharacterId.ChildWeapon05:
            case CharacterId.ChildWeapon06:
            case CharacterId.ChildWeapon07:
            case CharacterId.ChildWeapon08:
                {
                    m_FuWeaponData.SetSelectAttribute(m_UpgradeAttribute);
                    break;
                }
        }
    }

    /// <summary>
    /// 获取是否需要升级
    /// </summary>
    public static bool GetIsUpgradeCharacter(CharacterId id, CharacterAttribute att)
    {
        if (id == CharacterId.Null)
        {
            return false;
        }

        int index = (int)id;
        int levelCur = 0;
        int maxLevel = 1;
        switch (att)
        {
            case CharacterAttribute.AttributeA:
                {
                    levelCur = IGamerProfile.Instance.playerdata.characterData[index].levelA;
                    maxLevel = IGamerProfile.gameCharacter.characterDataList[index].maxlevelA;
                    if (levelCur >= maxLevel)
                    {
                        //已经到达最高级别
                        if (levelCur > maxLevel)
                        {
                            IGamerProfile.Instance.playerdata.characterData[index].levelA = maxLevel;
                        }
                        return false;
                    }
                    break;
                }
            case CharacterAttribute.AttributeB:
                {
                    levelCur = IGamerProfile.Instance.playerdata.characterData[index].levelB;
                    maxLevel = IGamerProfile.gameCharacter.characterDataList[index].maxlevelB;
                    if (levelCur >= maxLevel)
                    {
                        //已经到达最高级别
                        if (levelCur > maxLevel)
                        {
                            IGamerProfile.Instance.playerdata.characterData[index].levelB = maxLevel;
                        }
                        return false;
                    }
                    break;
                }
        }
        return true;
    }

    /// <summary>
    /// 获取是否激活相应武器
    /// </summary>
    bool GetIsActiveCharacter(CharacterId id)
    {
        if (id == CharacterId.Null)
        {
            return false;
        }

        if (id == CharacterId.MainWeapon || id == CharacterId.ShouYi)
        {
            //主武器和收益默认激活
            return true;
        }
        int index = (int)id;
        return IGamerProfile.Instance.playerdata.characterData[index].isactive;
    }

    void TestActiveAllChildWeapon()
    {
        for (int i = (int)CharacterId.ChildWeapon01; i <= (int)CharacterId.ChildWeaponCount; i++)
        {
            //IGamerProfile.Instance.playerdata.characterData[i].isactive = false;
            IGamerProfile.Instance.playerdata.characterData[i].isactive = i < (int)CharacterId.ShouYi ? true : false;
        }
        IGamerProfile.Instance.SaveGamerProfileToServer();
    }

    [Serializable]
    public class PayMoneyData
    {
        internal static int defaultValue = -1;
        internal int defaultRollValue = 0;
        /// <summary>
        /// 属性1付费信息
        /// </summary>
        int _payMoneyA = defaultValue;
        internal int payMoneyA
        {
            set
            {
                if (_payMoneyA != value)
                {
                    _payMoneyA = value;
                    if (payMoneyTextA != null)
                    {
                        payMoneyTextA.Text = _payMoneyA.ToString();
                    }
                    UpdateTextColorMoneyA();
                }
                else
                {
                    if (_payMoneyA > 0)
                    {
                        UpdateTextColorMoneyA();
                    }
                }
            }
            get
            {
                return _payMoneyA;
            }
        }

        void UpdateTextColorMoneyA()
        {
            if (qyChangeMatrialManageA != null)
            {
                if (_payMoneyA > IGamerProfile.Instance.playerdata.playerMoney)
                {
                    //玩家金币不足升级金币数展示位红色
                    qyChangeMatrialManageA.ChangeMatrial(QyChangeMatrial.ChangeState.New);
                }
                else
                {
                    //玩家金币充足升级金币数展示位白色
                    qyChangeMatrialManageA.ChangeMatrial(QyChangeMatrial.ChangeState.Old);
                }
            }
        }
        public GuiPlaneAnimationText payMoneyTextA;
        public QyChangeMatrialManage qyChangeMatrialManageA;
        public GameObject selectTxObjA;
        /// <summary>
        /// 属性2付费信息
        /// </summary>
        int _payMoneyB = defaultValue;
        internal int payMoneyB
        {
            set
            {
                if (_payMoneyB != value)
                {
                    _payMoneyB = value;
                    if (payMoneyTextB != null)
                    {
                        payMoneyTextB.Text = _payMoneyB.ToString();
                    }
                    UpdateTextColorMoneyB();
                }
                else
                {
                    if (_payMoneyB > 0)
                    {
                        UpdateTextColorMoneyB();
                    }
                }
            }
            get
            {
                return _payMoneyB;
            }
        }

        void UpdateTextColorMoneyB()
        {
            if (qyChangeMatrialManageB != null)
            {
                if (_payMoneyB > IGamerProfile.Instance.playerdata.playerMoney)
                {
                    //玩家金币不足升级金币数展示位红色
                    qyChangeMatrialManageB.ChangeMatrial(QyChangeMatrial.ChangeState.New);
                }
                else
                {
                    //玩家金币充足升级金币数展示位白色
                    qyChangeMatrialManageB.ChangeMatrial(QyChangeMatrial.ChangeState.Old);
                }
            }
        }
        public GuiPlaneAnimationText payMoneyTextB;
        public QyChangeMatrialManage qyChangeMatrialManageB;
        public GameObject selectTxObjB;
        
        /// <summary>
        /// 主界面按键的付费信息
        /// </summary>
        int _payMoneyCur = defaultValue;
        internal int payMoneyCur
        {
            set
            {
                if (_payMoneyCur != value)
                {
                    _payMoneyCur = value;
                    if (payMoneyTextCur != null)
                    {
                        payMoneyTextCur.Text = _payMoneyCur.ToString();
                    }
                    UpdateTextColorMoneyCur();
                }
                else
                {
                    if (_payMoneyCur > 0)
                    {
                        UpdateTextColorMoneyCur();
                    }
                }
            }
            get
            {
                return _payMoneyCur;
            }
        }

        void UpdateTextColorMoneyCur()
        {
            if (qyChangeMatrialManageCur != null)
            {
                if (_payMoneyCur > IGamerProfile.Instance.playerdata.playerMoney)
                {
                    //玩家金币不足升级金币数展示位红色
                    qyChangeMatrialManageCur.ChangeMatrial(QyChangeMatrial.ChangeState.New);
                }
                else
                {
                    //玩家金币充足升级金币数展示位白色
                    qyChangeMatrialManageCur.ChangeMatrial(QyChangeMatrial.ChangeState.Old);
                }
            }
        }
        public GuiPlaneAnimationText payMoneyTextCur;
        public QyChangeMatrialManage qyChangeMatrialManageCur;

        public void SetSelectAttribute(CharacterAttribute att)
        {
            QyFun.SetActive(selectTxObjA, att == CharacterAttribute.AttributeA ? true : false);
            QyFun.SetActive(selectTxObjB, att == CharacterAttribute.AttributeB ? true : false);
            switch (att)
            {
                case CharacterAttribute.AttributeA:
                    {
                        payMoneyCur = payMoneyA;
                        break;
                    }
                case CharacterAttribute.AttributeB:
                    {
                        payMoneyCur = payMoneyB;
                        break;
                    }
            }
            //Debug.Log("att ================ " + att + ", payMoneyCur == " + payMoneyCur);
        }

        public void ResetData()
        {
            payMoneyA = defaultValue;
            payMoneyB = defaultValue;
            payMoneyCur = defaultValue;
        }
    }

    [Serializable]
    public class MainWeaponData : PayMoneyData
    {
        int _levelA = defaultValue;
        internal int levelA
        {
            set
            {
                if (_levelA != value)
                {
                    if (_levelA == defaultValue)
                    {
                        if (speedLevelText != null)
                        {
                            speedLevelText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _levelA = value;
                    if (speedLevelText != null)
                    {
                        speedLevelText.SetIntegerRollValue(_levelA);
                    }
                }
            }
            get
            {
                return _levelA;
            }
        }
        public GuiPlaneAnimationTextRoll speedLevelText;
        int _levelB = defaultValue;
        internal int levelB
        {
            set
            {
                if (_levelB != value)
                {
                    if (_levelB == defaultValue)
                    {
                        if (huoLiLevelText != null)
                        {
                            huoLiLevelText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _levelB = value;
                    if (huoLiLevelText != null)
                    {
                        huoLiLevelText.SetIntegerRollValue(_levelB);
                    }
                }
            }
            get
            {
                return _levelB;
            }
        }
        public GuiPlaneAnimationTextRoll huoLiLevelText;

        int _speedAttribute = defaultValue;
        internal int speedAttribute
        {
            set
            {
                if (_speedAttribute != value)
                {
                    if (_speedAttribute == defaultValue)
                    {
                        if (speedAttributeText != null)
                        {
                            speedAttributeText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _speedAttribute = value;
                    if (speedAttributeText != null)
                    {
                        speedAttributeText.SetIntegerRollValue(_speedAttribute);
                    }
                }
            }
            get
            {
                return _speedAttribute;
            }
        }
        public GuiPlaneAnimationTextRoll speedAttributeText;

        int _huoLiAttribute = defaultValue;
        internal int huoLiAttribute
        {
            set
            {
                if (_huoLiAttribute != value)
                {
                    if (_huoLiAttribute == defaultValue)
                    {
                        if (huoLiAttributeText != null)
                        {
                            huoLiAttributeText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _huoLiAttribute = value;
                    if (huoLiAttributeText != null)
                    {
                        huoLiAttributeText.SetIntegerRollValue(_huoLiAttribute);
                    }
                }
            }
            get
            {
                return _huoLiAttribute;
            }
        }
        public GuiPlaneAnimationTextRoll huoLiAttributeText;

        public void Reset()
        {
            levelA = defaultValue;
            levelB = defaultValue;
            speedAttribute = defaultValue;
            huoLiAttribute = defaultValue;
            ResetData();
        }
    }
    public MainWeaponData m_MainWeaponData;

    /// <summary>
    /// 初始化主武器UI数据
    /// </summary>
    void InitMainWeaponData()
    {
        UpdateMainWeaponData();
    }

    /// <summary>
    /// 更新主武器UI数据
    /// </summary>
    void UpdateMainWeaponData()
    {
        if (m_MainWeaponData == null)
        {
            return;
        }
        CharacterId id = CharacterId.MainWeapon;
        bool isUpgradeA = GetIsUpgradeCharacter(id, CharacterAttribute.AttributeA);
        bool isUpgradeB = GetIsUpgradeCharacter(id, CharacterAttribute.AttributeB);
        QyFun.SetActive(btn_mainWeaponMoneyA, isUpgradeA);
        QyFun.SetActive(btn_mainWeaponMoneyB, isUpgradeB);

        IGamerProfile.PlayerData.PlayerChacterData dt = IGamerProfile.Instance.playerdata.characterData[(int)id];
        m_MainWeaponData.levelA = dt.levelA;
        m_MainWeaponData.levelB = dt.levelB;
        GameCharacter.CharacterData characterDt = IGamerProfile.gameCharacter.characterDataList[(int)id];
        m_MainWeaponData.speedAttribute = characterDt.LevelAToVal.GetValue(dt.levelA);
        m_MainWeaponData.huoLiAttribute = characterDt.LevelBToVal.GetValue(dt.levelB);
        if (isUpgradeA == true)
        {
            m_MainWeaponData.payMoneyA = characterDt.LevelAToMoney.GetValue(dt.levelA);
        }
        if (isUpgradeB == true)
        {
            m_MainWeaponData.payMoneyB = characterDt.LevelAToMoney.GetValue(dt.levelB);
        }

        if (GetIsUpgradeCharacter(CharacterId.MainWeapon, CharacterAttribute.AttributeA) == false
            && GetIsUpgradeCharacter(CharacterId.MainWeapon, CharacterAttribute.AttributeB) == false)
        {
            QyFun.SetActive(btn_mainWeaponActive, false);
        }
        else
        {
            QyFun.SetActive(btn_mainWeaponActive, true);
            UpdateUpgradeAttribute(CharacterId.MainWeapon);
        }
    }

    /// <summary>
    /// 升级主武器
    /// </summary>
    void UpgradeMainWeapon()
    {
        if (GetIsUpgradeCharacter(CharacterId.MainWeapon, CharacterAttribute.AttributeA) == false
            && GetIsUpgradeCharacter(CharacterId.MainWeapon, CharacterAttribute.AttributeB) == false)
        {
            return;
        }
        m_UpgradeCharacterId = CharacterId.MainWeapon;
        switch (m_UpgradeAttribute)
        {
            case CharacterAttribute.AttributeA:
                {
                    //请求升级
                    IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                                IGamerProfile.gameCharacter.characterDataList[(int)m_UpgradeCharacterId].LevelAToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[(int)m_UpgradeCharacterId].levelA),
                                                0,
                                                PayMoneyCallback), this);
                    break;
                }
            case CharacterAttribute.AttributeB:
                {
                    //请求升级
                    IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                                IGamerProfile.gameCharacter.characterDataList[(int)m_UpgradeCharacterId].LevelAToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[(int)m_UpgradeCharacterId].levelB),
                                                0,
                                                PayMoneyCallback), this);
                    break;
                }
        }
    }

    /// <summary>
    /// 重置主武器UI数据
    /// </summary>
    void ResetMainWeaponData()
    {
        if (m_MainWeaponData == null)
        {
            return;
        }
        m_MainWeaponData.Reset();
    }

    [Serializable]
    public class ShouYiData : PayMoneyData
    {
        int _levelA = defaultValue;
        internal int levelA
        {
            set
            {
                if (_levelA != value)
                {
                    if (_levelA == defaultValue)
                    {
                        if (coinJiaZhiLevelText != null)
                        {
                            coinJiaZhiLevelText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _levelA = value;
                    if (coinJiaZhiLevelText != null)
                    {
                        coinJiaZhiLevelText.SetIntegerRollValue(_levelA);
                    }
                }
            }
            get
            {
                return _levelA;
            }
        }
        public GuiPlaneAnimationTextRoll coinJiaZhiLevelText;
        int _levelB = defaultValue;
        internal int levelB
        {
            set
            {
                if (_levelB != value)
                {
                    if (_levelB == defaultValue)
                    {
                        if (guanQiaJCLevelText != null)
                        {
                            guanQiaJCLevelText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _levelB = value;
                    if (guanQiaJCLevelText != null)
                    {
                        guanQiaJCLevelText.SetIntegerRollValue(_levelB);
                    }
                }
            }
            get
            {
                return _levelB;
            }
        }
        public GuiPlaneAnimationTextRoll guanQiaJCLevelText;

        int _coinJiaZhiAttribute = defaultValue;
        internal int coinJiaZhiAttribute
        {
            set
            {
                if (_coinJiaZhiAttribute != value)
                {
                    if (_coinJiaZhiAttribute == defaultValue)
                    {
                        if (coinJiaZhiAttributeText != null)
                        {
                            coinJiaZhiAttributeText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _coinJiaZhiAttribute = value;
                    if (coinJiaZhiAttributeText != null)
                    {
                        coinJiaZhiAttributeText.SetIntegerRollValue(_coinJiaZhiAttribute);
                    }
                }
            }
            get
            {
                return _coinJiaZhiAttribute;
            }
        }
        public GuiPlaneAnimationTextRoll coinJiaZhiAttributeText;

        int _guanQiaJCAttribute = defaultValue;
        internal int guanQiaJCAttribute
        {
            set
            {
                if (_guanQiaJCAttribute != value)
                {
                    if (_guanQiaJCAttribute == defaultValue)
                    {
                        if (guanQiaJCAttributeText != null)
                        {
                            guanQiaJCAttributeText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _guanQiaJCAttribute = value;
                    if (guanQiaJCAttributeText != null)
                    {
                        guanQiaJCAttributeText.SetIntegerRollValue(_guanQiaJCAttribute);
                    }
                }
            }
            get
            {
                return _guanQiaJCAttribute;
            }
        }
        public GuiPlaneAnimationTextRoll guanQiaJCAttributeText;

        public void Reset()
        {
            levelA = defaultValue;
            levelB = defaultValue;
            coinJiaZhiAttribute = defaultValue;
            guanQiaJCAttribute = defaultValue;
            ResetData();
        }
    }
    public ShouYiData m_ShouYiData;

    /// <summary>
    /// 初始化主武器UI数据
    /// </summary>
    void InitShouYiData()
    {
        UpdateShouYiData();
    }

    /// <summary>
    /// 更新主武器UI数据
    /// </summary>
    void UpdateShouYiData()
    {
        if (m_ShouYiData == null)
        {
            return;
        }
        CharacterId id = CharacterId.ShouYi;
        bool isUpgradeA = GetIsUpgradeCharacter(id, CharacterAttribute.AttributeA);
        bool isUpgradeB = GetIsUpgradeCharacter(id, CharacterAttribute.AttributeB);
        QyFun.SetActive(btn_incomeMoneyA, isUpgradeA);
        QyFun.SetActive(btn_incomeMoneyB, isUpgradeB);

        IGamerProfile.PlayerData.PlayerChacterData dt = IGamerProfile.Instance.playerdata.characterData[(int)id];
        m_ShouYiData.levelA = dt.levelA;
        m_ShouYiData.levelB = dt.levelB;
        GameCharacter.CharacterData characterDt = IGamerProfile.gameCharacter.characterDataList[(int)id];
        m_ShouYiData.coinJiaZhiAttribute = characterDt.LevelAToVal.GetValue(dt.levelA);
        m_ShouYiData.guanQiaJCAttribute = characterDt.LevelBToVal.GetValue(dt.levelB);
        if (isUpgradeA == true)
        {
            m_ShouYiData.payMoneyA = characterDt.LevelAToMoney.GetValue(dt.levelA);
        }
        if (isUpgradeB == true)
        {
            m_ShouYiData.payMoneyB = characterDt.LevelAToMoney.GetValue(dt.levelB);
        }

        if (GetIsUpgradeCharacter(CharacterId.ShouYi, CharacterAttribute.AttributeA) == false
            && GetIsUpgradeCharacter(CharacterId.ShouYi, CharacterAttribute.AttributeB) == false)
        {
            QyFun.SetActive(btn_incomeActive, false);
        }
        else
        {
            QyFun.SetActive(btn_incomeActive, true);
            UpdateUpgradeAttribute(CharacterId.ShouYi);
        }
    }

    /// <summary>
    /// 升级主武器
    /// </summary>
    void UpgradeShouYi()
    {
        if (GetIsUpgradeCharacter(CharacterId.ShouYi, CharacterAttribute.AttributeA) == false
            && GetIsUpgradeCharacter(CharacterId.ShouYi, CharacterAttribute.AttributeB) == false)
        {
            return;
        }
        m_UpgradeCharacterId = CharacterId.ShouYi;
        switch (m_UpgradeAttribute)
        {
            case CharacterAttribute.AttributeA:
                {
                    //请求升级
                    IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                                IGamerProfile.gameCharacter.characterDataList[(int)m_UpgradeCharacterId].LevelAToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[(int)m_UpgradeCharacterId].levelA),
                                                0,
                                                PayMoneyCallback), this);
                    break;
                }
            case CharacterAttribute.AttributeB:
                {
                    //请求升级
                    IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                                IGamerProfile.gameCharacter.characterDataList[(int)m_UpgradeCharacterId].LevelAToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[(int)m_UpgradeCharacterId].levelB),
                                                0,
                                                PayMoneyCallback), this);
                    break;
                }
        }
    }

    /// <summary>
    /// 重置主武器UI数据
    /// </summary>
    void ResetShouYiData()
    {
        if (m_ShouYiData == null)
        {
            return;
        }
        m_ShouYiData.Reset();
    }
    
    [Serializable]
    public class FuWeaponData : PayMoneyData
    {
        int _levelA = defaultValue;
        internal int levelA
        {
            set
            {
                if (_levelA != value)
                {
                    if (_levelA == defaultValue)
                    {
                        if (qiangDuLevelText != null)
                        {
                            qiangDuLevelText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _levelA = value;
                    if (qiangDuLevelText != null)
                    {
                        qiangDuLevelText.SetIntegerRollValue(_levelA);
                    }
                }
            }
            get
            {
                return _levelA;
            }
        }
        public GuiPlaneAnimationTextRoll qiangDuLevelText;
        int _leveB = defaultValue;
        internal int levelB
        {
            set
            {
                if (_leveB != value)
                {
                    if (_leveB == defaultValue)
                    {
                        if (huoLiLevelText != null)
                        {
                            huoLiLevelText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _leveB = value;
                    if (huoLiLevelText != null)
                    {
                        huoLiLevelText.SetIntegerRollValue(_leveB);
                    }
                }
            }
            get
            {
                return _leveB;
            }
        }
        public GuiPlaneAnimationTextRoll huoLiLevelText;

        int _qingDuAttribute = defaultValue;
        internal int qiangDuAttribute
        {
            set
            {
                if (_qingDuAttribute != value)
                {
                    if (_qingDuAttribute == defaultValue)
                    {
                        if (qiangDuAttributeText != null)
                        {
                            qiangDuAttributeText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _qingDuAttribute = value;
                    if (qiangDuAttributeText != null)
                    {
                        qiangDuAttributeText.SetIntegerRollValue(_qingDuAttribute);
                    }
                }
            }
            get
            {
                return _qingDuAttribute;
            }
        }
        public GuiPlaneAnimationTextRoll qiangDuAttributeText;

        int _huoLiAttribute = defaultValue;
        internal int huoLiAttribute
        {
            set
            {
                if (_huoLiAttribute != value)
                {
                    if (_huoLiAttribute == defaultValue)
                    {
                        if (huoLiAttributeText != null)
                        {
                            huoLiAttributeText.SetIntegerRollValue(defaultRollValue, true);
                        }
                    }

                    _huoLiAttribute = value;
                    if (huoLiAttributeText != null)
                    {
                        huoLiAttributeText.SetIntegerRollValue(_huoLiAttribute);
                    }
                }
            }
            get
            {
                return _huoLiAttribute;
            }
        }
        public GuiPlaneAnimationTextRoll huoLiAttributeText;

        public void Reset()
        {
            levelA = defaultValue;
            levelB = defaultValue;
            qiangDuAttribute = defaultValue;
            huoLiAttribute = defaultValue;
            ResetData();
        }
    }
    public FuWeaponData m_FuWeaponData;

    /// <summary>
    /// 初始化主武器UI数据
    /// </summary>
    void InitFuWeaponData()
    {
        if (m_SelectChildWeaponId != CharacterId.Null)
        {
            UpdateFuWeaponData(m_SelectChildWeaponId);
        }
        else
        {
            UpdateFuWeaponData(CharacterId.ChildWeapon01);
        }
        UpdateFuWeaponButton();
    }

    void ShowFuWeaponSelectImg()
    {
        GameObject obj = GetChildWeaponSelect(CharacterId.ChildWeapon01);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon01);

        obj = GetChildWeaponSelect(CharacterId.ChildWeapon02);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon02);

        obj = GetChildWeaponSelect(CharacterId.ChildWeapon03);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon03);

        obj = GetChildWeaponSelect(CharacterId.ChildWeapon04);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon04);

        obj = GetChildWeaponSelect(CharacterId.ChildWeapon05);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon05);

        obj = GetChildWeaponSelect(CharacterId.ChildWeapon06);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon06);

        obj = GetChildWeaponSelect(CharacterId.ChildWeapon07);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon07);

        obj = GetChildWeaponSelect(CharacterId.ChildWeapon08);
        QyFun.SetActive(obj, m_SelectChildWeaponId == CharacterId.ChildWeapon08);
    }

    GameObject GetChildWeaponSelect(CharacterId id)
    {
        if (id == CharacterId.Null
            || id == CharacterId.MainWeapon
            || id == CharacterId.ShouYi)
        {
            return null;
        }
        return btn_secondaryWeaponSelectArray[(int)id - 1];
    }

    void UpdateFuWeaponButton()
    {
        UpdateFuWeaponButton(CharacterId.ChildWeapon01);
        UpdateFuWeaponButton(CharacterId.ChildWeapon02);
        UpdateFuWeaponButton(CharacterId.ChildWeapon03);
        UpdateFuWeaponButton(CharacterId.ChildWeapon04);
        UpdateFuWeaponButton(CharacterId.ChildWeapon05);
        UpdateFuWeaponButton(CharacterId.ChildWeapon06);
        UpdateFuWeaponButton(CharacterId.ChildWeapon07);
        UpdateFuWeaponButton(CharacterId.ChildWeapon08);
    }
    
    void UpdateFuWeaponButton(CharacterId id)
    {
        GameObject btHui = null;
        bool isActive = GetIsActiveCharacter(id);
        switch (id)
        {
            case CharacterId.ChildWeapon01:
                {
                    btHui = btn_secondaryWeaponChild01;
                    break;
                }
            case CharacterId.ChildWeapon02:
                {
                    btHui = btn_secondaryWeaponChild02;
                    break;
                }
            case CharacterId.ChildWeapon03:
                {
                    btHui = btn_secondaryWeaponChild03;
                    break;
                }
            case CharacterId.ChildWeapon04:
                {
                    btHui = btn_secondaryWeaponChild04;
                    break;
                }
            case CharacterId.ChildWeapon05:
                {
                    btHui = btn_secondaryWeaponChild05;
                    break;
                }
            case CharacterId.ChildWeapon06:
                {
                    btHui = btn_secondaryWeaponChild06;
                    break;
                }
            case CharacterId.ChildWeapon07:
                {
                    btHui = btn_secondaryWeaponChild07;
                    break;
                }
            case CharacterId.ChildWeapon08:
                {
                    btHui = btn_secondaryWeaponChild08;
                    break;
                }
        }

        if (btHui == null)
        {
            return;
        }

        QyChangeMatrial qyChangeMatrial = btHui.GetComponent<QyChangeMatrial>();
        if (qyChangeMatrial != null)
        {
            QyChangeMatrial.ChangeState st = isActive == true ? QyChangeMatrial.ChangeState.New : QyChangeMatrial.ChangeState.Old;
            qyChangeMatrial.ChangeMatrial(st);
        }
    }

    /// <summary>
    /// 判断是否为副武器
    /// </summary>
    bool GetIsFuWeapon(CharacterId id)
    {
        int idVal = (int)id;
        if (idVal < (int)CharacterId.ChildWeapon01 || idVal > (int)CharacterId.ChildWeaponCount)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 更新主武器UI数据
    /// </summary>
    void UpdateFuWeaponData(CharacterId id)
    {
        if (m_FuWeaponData == null)
        {
            return;
        }

        if (GetIsFuWeapon(id) == false)
        {
            return;
        }

        bool isUpgradeA = GetIsUpgradeCharacter(id, CharacterAttribute.AttributeA);
        bool isUpgradeB = GetIsUpgradeCharacter(id, CharacterAttribute.AttributeB);
        bool isActive = GetIsActiveCharacter(id);
        QyFun.SetActive(btn_secondaryWeaponMoneyA, isUpgradeA && isActive);
        QyFun.SetActive(btn_secondaryWeaponMoneyB, isUpgradeB && isActive);

        IGamerProfile.PlayerData.PlayerChacterData dt = IGamerProfile.Instance.playerdata.characterData[(int)id];
        m_FuWeaponData.levelA = dt.levelA;
        m_FuWeaponData.levelB = dt.levelB;
        GameCharacter.CharacterData characterDt = IGamerProfile.gameCharacter.characterDataList[(int)id];
        m_FuWeaponData.qiangDuAttribute = characterDt.LevelAToVal.GetValue(dt.levelA);
        m_FuWeaponData.huoLiAttribute = characterDt.LevelBToVal.GetValue(dt.levelB);
        if (isUpgradeA == true && isActive == true)
        {
            m_FuWeaponData.payMoneyA = characterDt.LevelAToMoney.GetValue(dt.levelA);
        }
        if (isUpgradeB == true && isActive == true)
        {
            m_FuWeaponData.payMoneyB = characterDt.LevelAToMoney.GetValue(dt.levelB);
        }

        if (GetIsUpgradeCharacter(id, CharacterAttribute.AttributeA) == false
            && GetIsUpgradeCharacter(id, CharacterAttribute.AttributeB) == false)
        {
            //已经是最高等级
            QyFun.SetActive(btn_secondaryWeaponActive, false);
        }
        else
        {
            if (m_SelectChildWeaponId != CharacterId.Null)
            {
                bool isUpgrade = true;
                if (m_ButtonId_SecondaryWeaponLR != ButtonId_SecondaryWeaponLR.Null)
                {
                    CharacterId idLR = SecondaryWeaponLRToCharacterId(m_ButtonId_SecondaryWeaponLR);
                    if (idLR != m_SelectChildWeaponId)
                    {
                        //当前光标和所选中的副武器不一致
                        isUpgrade = false;
                    }
                }
                QyFun.SetActive(btn_secondaryWeaponActive, isUpgrade);
                UpdateUpgradeAttribute(id);
            }
            else
            {
                QyFun.SetActive(btn_secondaryWeaponActive, false);
            }
        }
    }

    /// <summary>
    /// 升级主武器
    /// </summary>
    void UpgradeFuWeapon(CharacterId id)
    {
        if (GetIsFuWeapon(id) == false)
        {
            return;
        }

        if (GetIsActiveCharacter(id) == false)
        {
            return;
        }

        if (GetIsUpgradeCharacter(id, CharacterAttribute.AttributeA) == false
            && GetIsUpgradeCharacter(id, CharacterAttribute.AttributeB) == false)
        {
            return;
        }
        m_UpgradeCharacterId = id;
        switch (m_UpgradeAttribute)
        {
            case CharacterAttribute.AttributeA:
                {
                    //请求升级
                    IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                                IGamerProfile.gameCharacter.characterDataList[(int)m_UpgradeCharacterId].LevelAToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[(int)m_UpgradeCharacterId].levelA),
                                                0,
                                                PayMoneyCallback), this);
                    break;
                }
            case CharacterAttribute.AttributeB:
                {
                    //请求升级
                    IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                                IGamerProfile.gameCharacter.characterDataList[(int)m_UpgradeCharacterId].LevelAToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[(int)m_UpgradeCharacterId].levelB),
                                                0,
                                                PayMoneyCallback), this);
                    break;
                }
        }
    }

    /// <summary>
    /// 重置主武器UI数据
    /// </summary>
    void ResetFuWeaponData()
    {
        if (m_FuWeaponData == null)
        {
            return;
        }
        m_FuWeaponData.Reset();
    }

    private void OnButtonSelectOk(int index)
    {
        SoundEffectPlayer.Play("buttonok.wav");
        switch (m_ButtonId_MainUI)
        {
            case ButtonId_MainUI.StartGame:
                {
                    //开始游戏
                    IntoGame();
                    break;
                }
            case ButtonId_MainUI.MainWeapon:
                {
                    UpgradeMainWeapon();
                    break;
                }
            case ButtonId_MainUI.Income:
                {
                    UpgradeShouYi();
                    break;
                }
            case ButtonId_MainUI.SecondaryWeapon:
                {
                    if (m_SelectChildWeaponId != CharacterId.Null)
                    {
                        bool isUpgrade = true;
                        if (m_ButtonId_SecondaryWeaponLR != ButtonId_SecondaryWeaponLR.Null)
                        {
                            CharacterId id = SecondaryWeaponLRToCharacterId(m_ButtonId_SecondaryWeaponLR);
                            if (id != m_SelectChildWeaponId)
                            {
                                //当前光标和所选中的副武器不一致
                                isUpgrade = false;
                            }
                        }

                        if (isUpgrade == true)
                        {
                            UpgradeFuWeapon(m_SelectChildWeaponId);
                        }
                    }
                    break;
                }
        }
        return;
    }

    /// <summary>
    /// 增加角色级别
    /// </summary>
    void UpCharacterLevel(CharacterId id)
    {
        if (id == CharacterId.Null)
        {
            return;
        }

        int index = (int)id;
        switch (m_UpgradeAttribute)
        {
            case CharacterAttribute.AttributeA:
                {
                    int curLevel = IGamerProfile.Instance.playerdata.characterData[index].levelA;
                    int maxLevel = IGamerProfile.gameCharacter.characterDataList[index].maxlevelA;
                    if (curLevel < maxLevel)
                    {
                        //增加角色级别
                        IGamerProfile.Instance.playerdata.characterData[index].levelA += 1;
                    }
                    else if (curLevel > maxLevel)
                    {
                        IGamerProfile.Instance.playerdata.characterData[index].levelA = maxLevel;
                    }
                    break;
                }
            case CharacterAttribute.AttributeB:
                {
                    int curLevel = IGamerProfile.Instance.playerdata.characterData[index].levelB;
                    int maxLevel = IGamerProfile.gameCharacter.characterDataList[index].maxlevelB;
                    if (curLevel < maxLevel)
                    {
                        //增加角色级别
                        IGamerProfile.Instance.playerdata.characterData[index].levelB += 1;
                    }
                    else if (curLevel > maxLevel)
                    {
                        IGamerProfile.Instance.playerdata.characterData[index].levelB = maxLevel;
                    }
                    break;
                }
        }
        //存储档案
        IGamerProfile.Instance.SaveGamerProfileToServer();
    }

    private void PayMoneyCallback(IGamerProfile.PayMoneyData paydata, bool isSucceed)
    {
        if (m_UpgradeCharacterId == CharacterId.Null)
        {
            return;
        }

        if (isSucceed == true)
        {
            UpCharacterLevel(m_UpgradeCharacterId);
            //播放一个触发光效
            GameObject obj = LoadResource_UIPrefabs("SelectChracterEffect.prefab");
            obj.transform.SetParent(transform);
        }

        //刷新用户钱
        playerMoney.SetIntegerRollValue(IGamerProfile.Instance.playerdata.playerMoney);
        switch (m_UpgradeCharacterId)
        {
            case CharacterId.MainWeapon:
                {
                    if (isSucceed)
                    {
                        UpdateMainWeaponData();
                    }
                    break;
                }
            case CharacterId.ShouYi:
                {
                    if (isSucceed)
                    {
                        UpdateShouYiData();
                    }
                    break;
                }
            case CharacterId.ChildWeapon01:
            case CharacterId.ChildWeapon02:
            case CharacterId.ChildWeapon03:
            case CharacterId.ChildWeapon04:
            case CharacterId.ChildWeapon05:
            case CharacterId.ChildWeapon06:
            case CharacterId.ChildWeapon07:
            case CharacterId.ChildWeapon08:
                {
                    if (isSucceed)
                    {
                        UpdateFuWeaponData(m_UpgradeCharacterId);
                    }
                    break;
                }
        }
        return;
    }

    private void IntoGame()
    {
        if (m_SelectChildWeaponId != CharacterId.Null)
        {
            currentSelectGunIndex = (int)m_SelectChildWeaponId;
        }
        //保持当前选择的角色索引
        IGamerProfile.Instance.playerdata.playerLastChacterIndex = currentSelectGunIndex;
        IGamerProfile.Instance.SaveGamerProfileToServer();

        //设置当前地图
        IGamerProfile.Instance.gameEviroment.characterIndex = currentSelectGunIndex;
        if (selectCharacterMode == SelectCharacterMode.Mode_NextGame)
        {
            //进入加载界面
            ((UiSceneUICamera)UIManager).CreatePoolingScene(UiSceneUICamera.UISceneId.Id_UIGameLoading, UiSceneGameLoading.LoadingType.Type_LoadingGameNew);
            //删除主UI界面
            ((UiSceneUICamera)UIManager).ReleaseAloneScene();
        }
        else
        {
            //闪白
            ((UiSceneUICamera)UIManager).FadeScreen();
            //进入角色选择界面
            ((UiSceneUICamera)UIManager).CreateAloneScene(UiSceneUICamera.UISceneId.Id_UIGameMap);
        }
    }

    //一个子UI删除
    public override void OnChildUIRelease(GuiUiSceneBase ui)
    {
        if (ui is UiSceneRechargeAsk)
        {
            //重新设置会光标
            buttonGroup.IsWorkDo = true;
        }
    }

    void ShowCharacter(CharacterId id)
    {
        int index = (int)id;
        if (index < 0 || index >= m_PlayerArray.Length)
        {
            return;
        }

        for (int i = 0; i < m_PlayerArray.Length; i++)
        {
            QyFun.SetActive(m_PlayerArray[i], index == i ? true : false);
        }
    }
}
