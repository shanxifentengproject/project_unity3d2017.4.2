
public class VirusGameData
{
    public enum GameScene
    {
        Null = -1,
        rootpackage = 0,
        UI = 1,
        game1 = 2,
    }

    int _TotalCoin = 0;
    /// <summary>
    /// 玩家当前总金币数量
    /// </summary>
    public int TotalCoin
    {
        set
        {
            _TotalCoin = value;
            if (IGamerProfile.Instance != null)
            {
                IGamerProfile.Instance.playerdata.playerMoney = _TotalCoin;
                IGamerProfile.Instance.SaveGamerProfileToServer();
            }
        }
        get
        {
            return _TotalCoin;
        }
    }

    /// <summary>
    /// 玩家当前关卡战斗结束获得的金币数量
    /// </summary>
    public int CurLevelCoin;

    public int Level;

    public VirusGameData()
    {
        //这里对关卡信息进行初始化,从选择的关卡中读取信息
        if (IGamerProfile.Instance != null)
        {
            Level = IGamerProfile.Instance.gameEviroment.mapIndex + 1;
            TotalCoin = IGamerProfile.Instance.playerdata.playerMoney;
        }
        else
        {
            Level = 1;
            TotalCoin = 0;
        }
        //Level = 30; //test
    }
}

public class VirusGameDataAdapter
{
    private static VirusGameData _gameData;

    public static DifficultLevel CurDifficultLevel { set; get; }

    public static void Load()
    {
        _gameData = new VirusGameData();
        CurDifficultLevel = DifficultLevel.Difficult;
        InitCoinValue();

        if (VirusGameMrg.Instance.m_UIMrg.m_MapManage != null)
        {
            VirusGameMrg.Instance.m_UIMrg.m_MapManage.Init();
        }
    }
    
    public static void AddTotalCoin(int value)
    {
        _gameData.TotalCoin += value;
    }

    public static void UpdateTotalCoin(int value)
    {
        _gameData.TotalCoin = value;
    }

    /// <summary>
    /// 单个金币代表的价值
    /// </summary>
    static int _CoinValue = 1;
    static void InitCoinValue()
    {
        if (IGamerProfile.Instance != null)
        {
            UiSceneSelectGameCharacter.CharacterId id = UiSceneSelectGameCharacter.CharacterId.ShouYi;
            IGamerProfile.PlayerData.PlayerChacterData dt = IGamerProfile.Instance.playerdata.characterData[(int)id];
            GameCharacter.CharacterData characterDt = IGamerProfile.gameCharacter.characterDataList[(int)id];
            _CoinValue = characterDt.LevelAToVal.GetValue(dt.levelA); //单个金币代表的价值
        }
    }

    public static void AddLevelCoin(int value)
    {
        if (IGamerProfile.Instance != null)
        {
            _gameData.CurLevelCoin += _CoinValue;
        }
        else
        {
            _gameData.CurLevelCoin += value;
        }
    }

    public static void AddLevel()
    {
        //这里表示玩家已经通过当前关卡,需要对关卡信息进行存档
        if (IGamerProfile.Instance != null)
        {
            int index = _gameData.Level - 1;
            if (index < IGamerProfile.Instance.playerdata.levelProcess.Length)
            {
                //更新进度
                IGamerProfile.Instance.playerdata.levelProcess[index] = IGamerProfile.gameLevel.mapMaxLevel[index];
                IGamerProfile.Instance.SaveGamerProfileToServer();
            }

            if (_gameData.Level < IGamerProfile.Instance.playerdata.levelProcess.Length)
            {
                _gameData.Level++;
            }
            else
            {
                //已经到最大关卡了
            }
        }
        else
        {
            _gameData.Level++;
        }

        //if (IGamerProfile.Instance != null)
        //{
        //    int weaponLevel = VirusTool.UnlockViceWeapon(_gameData.Level);
        //    if (weaponLevel >= (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon01
        //        && weaponLevel <= (int)UiSceneSelectGameCharacter.CharacterId.ChildWeaponCount)
        //    {
        //        if (IGamerProfile.Instance.playerdata.characterData[weaponLevel].isactive == false)
        //        {
        //            //解锁副武器
        //            //标记当前角色解锁
        //            IGamerProfile.Instance.playerdata.characterData[weaponLevel].isactive = true;
        //            //存储档案
        //            IGamerProfile.Instance.SaveGamerProfileToServer();
        //        }
        //    }
        //}
    }

    public static void MinusTotalCoin(int value)
    {
        _gameData.TotalCoin -= value;
    }

    public static void ResetLevelCoin()
    {
        _gameData.CurLevelCoin = 0;
    }

    public static int GetTotalCoin()
    {
        return _gameData.TotalCoin;
    }

    public static int GetCurLevelCoin()
    {
        return _gameData.CurLevelCoin;
    }

    public static int GetLevel()
    {
        return _gameData.Level;
    }
}