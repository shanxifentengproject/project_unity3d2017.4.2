﻿using System.Collections.Generic;
using Assets.Scripts.Tool;
using UnityEngine;

public static class VirusTool
{
    //这里的属性是配置病毒npc左右移动范围
    public static float LeftX = -8.4f;
    public static float RightX = 8.4f;

    public static float TopY = 10.6f;
    public static float BottomY = -8.6f;

    private static List<string> _propNames;

    public static float GetScaleByLevel(SplitLevel splitLevel)
    {
        switch (splitLevel)
        {
            case SplitLevel.Level1:
                return 0.5f;
            case SplitLevel.Level2:
                return 0.7f;
            case SplitLevel.Level3:
                return 0.85f;
            case SplitLevel.Level4:
                return 1.1f;
            case SplitLevel.Level5:
                return 2f;
        }
        return 1;
    }


    public static ColorLevel GetColorLevel(ColorLevel lastLevel)
    {
        List<ColorLevel> levels = new List<ColorLevel>();
        switch (lastLevel)
        {
            case ColorLevel.Level0:
                levels.Add(ColorLevel.Level1);
                levels.Add(ColorLevel.Level2);
                return levels[Random.Range(0, 2)];
            case ColorLevel.Level1:
                levels.Add(ColorLevel.Level1);
                levels.Add(ColorLevel.Level2);
                levels.Add(ColorLevel.Level3);
                return levels[Random.Range(0, 3)];
            case ColorLevel.Level2:
                levels.Add(ColorLevel.Level2);
                levels.Add(ColorLevel.Level3);
                levels.Add(ColorLevel.Level4);
                return levels[Random.Range(0, 3)];
            case ColorLevel.Level3:
                levels.Add(ColorLevel.Level3);
                levels.Add(ColorLevel.Level4);
                levels.Add(ColorLevel.Level5);
                return levels[Random.Range(0, 3)];
            case ColorLevel.Level4:
                levels.Add(ColorLevel.Level4);
                levels.Add(ColorLevel.Level5);
                levels.Add(ColorLevel.Level6);
                return levels[Random.Range(0, 3)];
            case ColorLevel.Level5:
                levels.Add(ColorLevel.Level5);
                levels.Add(ColorLevel.Level6);
                levels.Add(ColorLevel.Level7);
                return levels[Random.Range(0, 3)];
            case ColorLevel.Level6:
                levels.Add(ColorLevel.Level6);
                levels.Add(ColorLevel.Level7);
                return levels[Random.Range(0, 2)];
            case ColorLevel.Level7:
                levels.Add(ColorLevel.Level7);
                levels.Add(ColorLevel.Level8);
                return levels[Random.Range(0, 2)];
            case ColorLevel.Level8:
                return ColorLevel.Level8;
        }
        return ColorLevel.Level8;
    }

    public static int GetVirusTotalValue(string virusName, int level)
    {
        int totalValue = 100;
        switch (virusName)
        {
            case "NormalVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.NormalVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 10 + 100;
                }
                break;
            case "FastVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.FastVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 4 + 100;
                }
                break;
            case "CureVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.CureVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 4 + 100;
                }
                break;
            case "CollisionVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.CollisionVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 5 + 100;
                }
                break;
            case "SlowDownVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.SlowDownVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 3 + 100;
                }
                break;
            case "RegenerativeVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.RegenerativeVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 2 + 100;
                }
                break;
            case "SwallowVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.SwallowVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 6 + 100;
                }
                break;
            case "ExplosiveVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.ExplosiveVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 10 + 100;
                }
                break;
            case "AdsorptionVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.AdsorptionVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 10 + 100;
                }
                break;
            case "DefendingVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.DefendingVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 6 + 100;
                }
                break;
            case "TrackingVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.TrackingVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 10 + 100;
                }
                break;
            case "DartVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.DartVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 8 + 100;
                }
                break;
            case "LaunchVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.LaunchVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 10 + 100;
                }
                break;
            case "VampireVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.VampireVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 5 + 100;
                }
                break;
            case "ExpansionVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.ExpansionVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 10 + 100;
                }
                break;
            case "ThreePointsVirus":
                if (IGamerProfile.Instance != null)
                {
                    totalValue = level * level * IGamerProfile.gameLevel.GetNpcHealth((int)VirusName.ThreePointsVirus) + 100;
                }
                else
                {
                    totalValue = level * level * 8 + 100;
                }
                break;
        }
        return totalValue;
    }

    public static int GetVirusHealthByColorLevel(string virusName, int level, ColorLevel colorLevel)
    {
        int totalValue = GetVirusTotalValue(virusName, level);
        Vector2 section = GetSectionByVirusLevel(colorLevel);
        return (int)Random.Range(section.x * totalValue, section.y * totalValue);
    }


    public static ColorLevel GetVirusColorLevel(string virusName, int level, float health)
    {
        int totalValue = GetVirusTotalValue(virusName, level);
        return GetVirusColorLevel(totalValue, health);
    }


    private static Vector2 GetSectionByVirusLevel(ColorLevel virusLevel)
    {
        switch (virusLevel)
        {
            case ColorLevel.Level0: return new Vector2(0.5f, 1);
            case ColorLevel.Level1: return new Vector2(0.4f, 0.5f);
            case ColorLevel.Level2: return new Vector2(0.35f, 0.4f);
            case ColorLevel.Level3: return new Vector2(0.3f, 0.35f);
            case ColorLevel.Level4: return new Vector2(0.25f, 0.3f);
            case ColorLevel.Level5: return new Vector2(0.2f, 0.25f);
            case ColorLevel.Level6: return new Vector2(0.15f, 0.2f);
            case ColorLevel.Level7: return new Vector2(0.05f, 0.15f);
            case ColorLevel.Level8: return new Vector2(0.01f, 0.05f);
        }
        return Vector2.zero;
    }


    private static ColorLevel GetVirusColorLevel(int totalValue,float curValue)
    {
        int v0 = (int)(totalValue * 1);
        int v1 = (int)(totalValue * 0.5f);
        int v2 = (int)(totalValue * 0.4f);
        int v3 = (int)(totalValue * 0.35f);
        int v4 = (int)(totalValue * 0.3f);
        int v5 = (int)(totalValue * 0.25f);
        int v6 = (int)(totalValue * 0.2f);
        int v7 = (int)(totalValue * 0.15f);
        int v8 = (int)(totalValue * 0.05f);
        int v9 = (int)(totalValue * 0);
        if (curValue <= v0 && curValue >= v1)
        {
            return ColorLevel.Level0;
        }
        if (curValue < v1 && curValue >= v2)
        {
            return ColorLevel.Level1;
        }
        if (curValue < v2 && curValue >= v3)
        {
            return ColorLevel.Level2;
        }
        if (curValue < v3 && curValue >= v4)
        {
            return ColorLevel.Level3;
        }
        if (curValue < v4 && curValue >= v5)
        {
            return ColorLevel.Level4;
        }
        if (curValue < v5 && curValue >= v6)
        {
            return ColorLevel.Level5;
        }
        if (curValue < v6 && curValue >= v7)
        {
            return ColorLevel.Level6;
        }
        if (curValue < v7 && curValue >= v8)
        {
            return ColorLevel.Level7;
        }
        if (curValue < v8 && curValue >= v9)
        {
            return ColorLevel.Level8;
        }
        return ColorLevel.Level8;
    }


    private static ChanceRoll GetRoll(int level)
    {
        ChanceRoll roll = new ChanceRoll();
        if (level == 1)
        {
            roll.FillRoll(new List<float> { 100, 10, 10, 50, 10, 80, 80, 10, 50 });
        }
        if (level >= 2 && level <= 5)
        {
            roll.FillRoll(new List<float> { 80, 10, 10, 50, 10, 80, 80, 50, 50 });
        }
        if (level > 5 && level <= 10)
        {
            roll.FillRoll(new List<float> { 100, 10, 10, 50, 10, 50, 50, 100, 50 });
        }
        if (level > 10)
        {
            roll.FillRoll(new List<float> { 2500, 40, 40, 100, 10, 50, 50, 100, 50 });
        }
        return roll;
    }


    private static void FillRoll(this ChanceRoll roll, List<float> raritys)
    {
        for (int i = 0; i < raritys.Count; i++)
        {
            roll.Add(raritys[i]);
        }
    }





    public static List<float> GetRandomAngles(int count, float offsetValue)
    {
        if (count * offsetValue > 360)
            return null;
        float o = 360f / count;
        List<float> angles = new List<float>();
        angles.Add(Random.Range(0, o));
        for (int i = 0; i < count - 1; i++)
        {
            angles.Add(Random.Range(offsetValue, o) + angles[i]);
        }
        return angles;
    }


    public static Vector2 SceneToUguiPos(RectTransform rectTransform, Vector2 pos)
    {
        Vector2 outPos;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, null, out outPos);
        return outPos;
    }


    public static Vector2 GetBesselPoint(Vector2 start, Vector2 end, Vector2 mid,float t)
    {
        return (1 - t)*(1 - t)*start + 2*t*(1 - t)*mid + t*t*end;
    }


    public static int GetUpgradeCoin(int level)
    {
        return 235 * level;
    }


    public static int GetVirusDeathCoin(int level)
    {
        int count = Random.Range(0, 10);
        if (count > 6)
            return 0;
        float coin = Random.Range(2f, 5f) * level;
        return Mathf.RoundToInt(coin);
    }


    public static void Reshuffle<T>(this List<T> originList)
    {
        for (int i = 0; i < originList.Count; i++)
        {
            int index = Random.Range(i, originList.Count);
            var tempValue = originList[index];
            originList[index] = originList[i];
            originList[i] = tempValue;
        }
    }


    public static int GetChildSplit(SplitLevel splitLevel, VirusName virusName)
    {
        switch (splitLevel)
        {
            case SplitLevel.Level1:
                return 1;
            case SplitLevel.Level2:
                if (virusName == VirusName.ThreePointsVirus)
                    return 1 + 3;
                return 1 + 2;
            case SplitLevel.Level3:
                if (virusName == VirusName.ThreePointsVirus)
                    return 1 + 3 + 9;
                return 1 + 2 + 4;
            case SplitLevel.Level4:
                if (virusName == VirusName.ThreePointsVirus)
                    return 1 + 3 + 9 + 27;
                return 1 + 2 + 4 + 8;
            case SplitLevel.Level5:
                if (virusName == VirusName.ThreePointsVirus)
                    return 1 + 3 + 9 + 27 + 81;
                return 1 + 2 + 4 + 8 + 16;
        }
        return 0;
    }


    public static int GetSplit(SplitLevel splitLevel, VirusName virusName)
    {
        switch (splitLevel)
        {
            case SplitLevel.Level1:
                return 0;
            case SplitLevel.Level2:
                if (virusName == VirusName.ThreePointsVirus)
                    return 3;
                return 2;
            case SplitLevel.Level3:
                if (virusName == VirusName.ThreePointsVirus)
                    return 3 + 9;
                return 2 + 4;
            case SplitLevel.Level4:
                if (virusName == VirusName.ThreePointsVirus)
                    return 3 + 9 + 27;
                return 2 + 4 + 8;
            case SplitLevel.Level5:
                if (virusName == VirusName.ThreePointsVirus)
                    return 3 + 9 + 27 + 81;
                return 2 + 4 + 8 + 16;
        }
        return 0;
    }


    public static string GetPropName(int level)
    {
        if (_propNames == null)
        {
            _propNames = new List<string>();
            _propNames.Add("None");
            for (int i = 0; i <= (int)VirusPropEnum.Weaken; i++)
            {
                var propEnum = (VirusPropEnum)i;
                _propNames.Add(propEnum.ToString());
            }
        }
        var roll = GetRoll(level);
        int index = roll.Roll();
        return _propNames[index];
    }


    public static int UnlockViceWeapon(int level)
    {
        //这里是配置通过多少关卡之后解锁的副武器数据
        if (IGamerProfile.Instance == null)
        {
            switch (level - 1)
            {
                case 5: return 1;
                case 20: return 2;
                case 40: return 3;
                case 60: return 4;
                case 80: return 5;
                case 100: return 6;
                case 120: return 7;
                case 150: return 8;
            }
        }
        else
        {
            int realLevel = level - 1;
            int unlockFuWp01 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon01].unlock;
            int unlockFuWp02 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon02].unlock;
            int unlockFuWp03 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon03].unlock;
            int unlockFuWp04 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon04].unlock;
            int unlockFuWp05 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon05].unlock;
            int unlockFuWp06 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon06].unlock;
            int unlockFuWp07 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon07].unlock;
            int unlockFuWp08 = IGamerProfile.gameCharacter.characterDataList[(int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon08].unlock;
            if (realLevel == unlockFuWp01)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon01;
            }
            else if (realLevel == unlockFuWp02)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon02;
            }
            else if (realLevel == unlockFuWp03)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon03;
            }
            else if (realLevel == unlockFuWp04)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon04;
            }
            else if (realLevel == unlockFuWp05)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon05;
            }
            else if (realLevel == unlockFuWp06)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon06;
            }
            else if (realLevel == unlockFuWp07)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon07;
            }
            else if (realLevel == unlockFuWp08)
            {
                return (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon08;
            }
        }
        return -1;
    }


    public static string GetStrByIntger(int value)
    {
        if (value > 1000 && value < 1000 * 1000)//K
        {
            float v = value * 1.0f / 1000f;
            return string.Format("{0:F1}K", v);
        }
        if (value >= 1000 * 1000 && value < 1000 * 1000 * 1000)//M
        {
            float v = value * 1.0f / 1000f / 1000f;
            return string.Format("{0:F1}M", v);
        }
        if (value > 1000 * 1000 * 1000)//B
        {
            float v = value * 1.0f / 1000f / 1000f / 1000f;
            return string.Format("{0:F1}B", v);
        }
        return value.ToString();
    }
}