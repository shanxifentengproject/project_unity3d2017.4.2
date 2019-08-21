using System.Collections.Generic;

public class VirusPlayerData
{

    public int WeaponLevel { set; get; }

    public int ShootNum { set; get; }
    public int MaxShootNum { get { return 8; } }
    public int MinShootNum { get { return 1; } }

    public int ShootPower { set; get; }

    public int ShootSpeed { set; get; }
   


    public bool IsShootCoin { set; get; }

    public bool IsRepulse { set; get; }

    public bool IsPower { set; get; }


    public VirusPlayerData()
    {
        InitShootNun();
        InitShootPower();

        IsShootCoin = false;
        IsRepulse = false;

        IsPower = false;

        WeaponLevel = 1;
        ShootSpeed = 1;
    }

    void InitShootNun()
    {
        //玩家主武器发射子弹数量
        if (IGamerProfile.Instance == null)
        {
            ShootNum = 1;
        }
        else
        {
            //max min maxLv minLv
            //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
            //cur = k * (curLv - minLv) + min;
            float max = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].LevelCRange.m_h0;
            float min = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].LevelCRange.m_h1;
            if (min < MinShootNum)
            {
                min = MinShootNum;
            }
            float curLv = IGamerProfile.Instance.playerdata.characterData[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].levelB;
            float maxLv = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].maxlevelB;
            float minLv = 0f;
            float k = (max - min) / (maxLv - minLv);
            //curLv = maxLv; //test
            ShootNum = (int)(k * (curLv - minLv) + min);
            ShootNum = (int)UnityEngine.Mathf.Clamp(ShootNum, min, max);
        }
    }

    void InitShootPower()
    {
        //这里对玩家主武器子弹伤害进行初始化,需要根据当前火力等级进行计算
        if (IGamerProfile.Instance == null)
        {
            //ShootPower = 2;
            ShootPower = 200; //test
        }
        else
        {
            //max min maxLv minLv
            //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
            //cur = k * (curLv - minLv) + min;
            float max = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].LevelBRange.m_h0;
            float min = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].LevelBRange.m_h1;
            float curLv = IGamerProfile.Instance.playerdata.characterData[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].levelB;
            float maxLv = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.MainWeapon].maxlevelB;
            float minLv = 0f;
            float k = (max - min) / (maxLv - minLv);
            //curLv = maxLv; //test
            ShootPower = (int)(k * (curLv - minLv) + min);
            ShootPower = (int)UnityEngine.Mathf.Clamp(ShootPower, min, max);
        }
    }
}

public class VirusPlayerDataAdapter
{

    private static VirusPlayerData _virusPlayerData;
    private static int _curNum;
    private static List<int> _upgradeNum; 
    public static void Load()
    {
        _virusPlayerData = new VirusPlayerData();
        _curNum = 0;
        _upgradeNum = new List<int> { 5, 8, 6, 7, 10, 15, 20 };
    }


    public static bool UpgradeShoot()
    {
        _curNum++;
        if (_curNum >= _upgradeNum[0])
        {
            if (_upgradeNum.Count > 1)
            {
                _curNum -= _upgradeNum[0];
                _upgradeNum.RemoveAt(0);
            }
            return true;
        }
        return false;
    }


    public static void AddWeaponLevel()
    {
        _virusPlayerData.WeaponLevel++;
    }

    public static void AddShootPower(int value)
    {
        //这里对玩家子弹伤害进行升级,需要进行存档
        _virusPlayerData.ShootPower += value;
    }

    public static void AddShootSpeed()
    {
        _virusPlayerData.ShootSpeed++;
    }

    public static void AddShootNum(int value)
    {
        _virusPlayerData.ShootNum += value;
    }



    public static void MulShootNum(int mul)
    {
        _virusPlayerData.ShootNum *= mul;
    }

    public static void MulHalfShootNum()
    {
        _virusPlayerData.ShootNum /= 2;
    }

    public static void MulShootPower(int mul)
    {
        _virusPlayerData.ShootPower *= mul;
        _virusPlayerData.IsPower = true;
    }

    public static void MulHalfShootPower()
    {
        _virusPlayerData.ShootPower /= 2;
        _virusPlayerData.IsPower = false;
    }

    public static void SetIsShootCoin(bool b)
    {
        _virusPlayerData.IsShootCoin = b;
    }

    public static void SetIsRepulse(bool b)
    {
        _virusPlayerData.IsRepulse = b;
    }


    public static bool GetPower()
    {
        return _virusPlayerData.IsPower;
    }

    public static int GetShootNum()
    {
        return _virusPlayerData.ShootNum;
    }

    public static int GetMaxShootNum()
    {
        return _virusPlayerData.MaxShootNum;
    }

    public static int GetShootPower()
    {
        return _virusPlayerData.ShootPower;
    }

    public static bool GetShootCoin()
    {
        return _virusPlayerData.IsShootCoin;
    }

    public static bool GetShootRepulse()
    {
        return _virusPlayerData.IsRepulse;
    }

    public static int GetWeaponLevel()
    {
        return _virusPlayerData.WeaponLevel;
    }

    public static int GetUpgradeValue()
    {
        switch (VirusGameDataAdapter.CurDifficultLevel)
        {
            case DifficultLevel.Simple:
                return 8;
            case DifficultLevel.General:
                return 4;
            case DifficultLevel.Difficult:
                return 1;
        }
        return 0;
    }
}