﻿using System.Collections.Generic;
using Assets.Scripts.Tool;
using UnityEngine;

public class WaveVirusItem
{
    public VirusName VirusName;
    public SplitLevel SplitLevel;
    public ColorLevel ColorLevel;
}

public class WaveVirusData
{
    public List<List<WaveVirusItem>> Data;

    public Dictionary<SplitLevel, int> Cache;

    public bool HasBoss;

    public WaveVirusData()
    {
        Data = new List<List<WaveVirusItem>>();
        Cache = new Dictionary<SplitLevel, int>();

        HasBoss = false;
    }
}

public class WaveVirusDataAdapter
{
    private static WaveVirusData _data;
    private static ChanceRoll _nameRoll;
    private static ChanceRoll _colorRoll;
    private static List<VirusName> _virusNames;

    private static VirusName _lastBossName;
    private static VirusName _curBossName;

    public static int MaxWave = 3;
    public static int WaveIndex { set; get; }

    public static bool IsNextWave { set; get; }

    public static int Load(int level)
    {
        _data = new WaveVirusData();
        _nameRoll = new ChanceRoll();
        _colorRoll = new ChanceRoll();
        WaveIndex = 0;
        IsNextWave = true;
        if (IGamerProfile.Instance != null)
        {
            string bossType = IGamerProfile.gameLevel.mapData[level - 1].bossData.BossType;
            _data.HasBoss = bossType == "" ? false : true;
            if (_data.HasBoss == true)
            {
                _curBossName = GetBossVirusName(bossType);
            }
        }
        else
        {
            if (level == 2 || level % 5 == 0)
            {
                _data.HasBoss = true;
                _curBossName = GetBossVirusName(level);
            }
        }

        FillCache(level);
        FillVirusNames(level);
        FillNameRoll();
        FillColorRoll(level);
        int endValue = FillData();
        if (_data.HasBoss)
        {
            WaveVirusItem boss = new WaveVirusItem();
            boss.SplitLevel = SplitLevel.Level5;
            boss.ColorLevel = ColorLevel.Level0;
            boss.VirusName = _curBossName;
            _data.Data[MaxWave - 1].Insert(0, boss);
            endValue += VirusTool.GetChildSplit(SplitLevel.Level5, boss.VirusName);
        }
        return endValue;
    }

    public static List<WaveVirusItem> GetWaveVirus()
    {
        return _data.Data[WaveIndex];
    }

    public static bool IsShowBoss()
    {
        return _data.HasBoss;
    }
    

    private static int FillData()
    {
        List<WaveVirusItem> items = new List<WaveVirusItem>();
        foreach (var t1 in _data.Cache)
        {
            for (int i = 0; i < t1.Value; i++)
            {
                WaveVirusItem item = new WaveVirusItem();
                int nameIndex = _nameRoll.Roll();
                item.SplitLevel = t1.Key;
                item.VirusName = _virusNames[nameIndex];
                item.ColorLevel = (ColorLevel)_colorRoll.Roll();
                items.Add(item);
            }
        }

        int count = items.Count;
        items.Reshuffle();
        List<WaveVirusItem> list1 = new List<WaveVirusItem>();
        List<WaveVirusItem> list2 = new List<WaveVirusItem>();
        List<WaveVirusItem> list3 = new List<WaveVirusItem>();
        if (IGamerProfile.Instance != null)
        {
            int leval = VirusGameDataAdapter.GetLevel();
            FtGameLevel.MapData mpDt = IGamerProfile.gameLevel.mapData[leval - 1];
            int index = 0;
            int maxNpc = mpDt.spawnDatas[index].NpcNum;
            for (int i = 0; i < count; i++)
            {
                if (list1.Count < maxNpc && index == 0)
                {
                    list1.Add(items[i]);
                    if (list1.Count == maxNpc)
                    {
                        index++;
                        maxNpc = mpDt.spawnDatas[index].NpcNum;
                    }
                }
                else if (list2.Count < maxNpc && index == 1)
                {
                    list2.Add(items[i]);
                    if (list2.Count == maxNpc)
                    {
                        index++;
                        maxNpc = mpDt.spawnDatas[index].NpcNum;
                    }
                }
                else
                {
                    list3.Add(items[i]);
                }
            }
        }
        else
        {
            int t = count / MaxWave;
            for (int i = 0; i < count; i++)
            {
                if (i < t)
                    list1.Add(items[i]);
                else if (i < 2 * t)
                    list2.Add(items[i]);
                else
                    list3.Add(items[i]);
            }
        }
        _data.Data.Add(list1);
        _data.Data.Add(list2);
        _data.Data.Add(list3);

        int value = 0;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            value += VirusTool.GetChildSplit(item.SplitLevel, item.VirusName);
        }
        items.Clear();
        return value;
    }

    /// <summary>
    /// 这里用来分配病毒大小
    /// </summary>
    private static void FillCache(int level)
    {
        //这里进行npc数量控制
        if (IGamerProfile.Instance != null)
        {
            //根据关卡配置的刷怪总数和当前关卡信息进行npc大小分配
            int maxNpc = 0;
            FtGameLevel.MapData mpDt = IGamerProfile.gameLevel.mapData[level - 1];
            for (int i = 0; i < mpDt.spawnDatas.Length; i++)
            {
                maxNpc += mpDt.spawnDatas[i].NpcNum;
            }

            int split01 = GetSplitLevel1(level);
            int split02 = GetSplitLevel2(level);
            int split03 = GetSplitLevel3(level);
            int split04 = GetSplitLevel4(level);
            int splitMax = split01 + split02 + split03 + split04;
            if (splitMax > maxNpc)
            {
                int dVal = splitMax - maxNpc;
                if (dVal <= split04)
                {
                    split04 -= dVal;
                }
                else
                {
                    dVal -= split04;
                    split04 = 0;
                    if (dVal <= split03)
                    {
                        split03 -= dVal;
                    }
                    else
                    {
                        dVal -= split03;
                        split03 = 0;
                        if (dVal <= split02)
                        {
                            split02 -= dVal;
                        }
                        else
                        {
                            dVal -= split02;
                            split02 = 0;
                            if (dVal <= split01)
                            {
                                split01 -= dVal;
                            }
                        }
                    }
                }
            }
            else if (splitMax < maxNpc)
            {
                split01 += (maxNpc - splitMax);
            }

            _data.Cache.Add(SplitLevel.Level1, split01);
            _data.Cache.Add(SplitLevel.Level2, split02);
            _data.Cache.Add(SplitLevel.Level3, split03);
            _data.Cache.Add(SplitLevel.Level4, split04);
        }
        else
        {
            _data.Cache.Add(SplitLevel.Level1, GetSplitLevel1(level));
            _data.Cache.Add(SplitLevel.Level2, GetSplitLevel2(level));
            _data.Cache.Add(SplitLevel.Level3, GetSplitLevel3(level));
            _data.Cache.Add(SplitLevel.Level4, GetSplitLevel4(level));
        }
    }

    private static int GetSplitLevel1(int level)
    {
        int endValue;
        if (level <= 2)
        {
            endValue = Random.Range(8, 10);
        }
        else if (level <= 5)
        {
            endValue = Random.Range(8, 12);
        }
        else if (level <= 70)
        {
            int c = (level - 1) / 5;
            if (c > 6)
                endValue = Random.Range(6 + c, 12 + c);
            else
                endValue = Random.Range(5 + 12 - c, 10 + 12 - c);
        }
        else
        {
            endValue = Random.Range(10, 20);
        }
        return endValue;
    }

    private static int GetSplitLevel2(int level)
    {
        int endValue ;
        if (level <= 2)
        {
            endValue = Random.Range(6, 10);
        }
        else if (level <= 5)
        {
            endValue = Random.Range(8, 15);
        }
        else if (level <= 70)
        {
            int c = (level - 1) / 5;
            if (c > 6)
                endValue = Random.Range(6 + c, 12 + c);
            else
                endValue = Random.Range(10 + 12 - c, 15 + 12 - c);
        }
        else
        {
            endValue = Random.Range(10, 20);
        }
        return endValue;
    }

    private static int GetSplitLevel3(int level)
    {
        int endValue ;
        if (level <= 2)
        {
            endValue = 0;
        }
        else if (level <= 5)
        {
            endValue = Random.Range(1, 3);
        }
        else if (level <= 70)
        {
            int c = (level - 1) / 5;
            int v = Random.Range(0, c);
            endValue = Random.Range(2, 2 + v);
        }
        else
        {
            endValue = Random.Range(10, 20);
        }
        return endValue;
    }

    private static int GetSplitLevel4(int level)
    {
        int endValue;
        if (level <= 2)
        {
            endValue = 0;
        }
        else if (level <= 5)
        {
            endValue = 1;
        }
        else if (level <= 70)
        {
            endValue = Random.Range(2, 5);
        }
        else
        {
            endValue = Random.Range(3, 6);
        }
        return endValue;
    }

    private static VirusName GetBossVirusName(string bossType)
    {
        switch (bossType)
        {
            case "NormalVirus":
                return VirusName.NormalVirus;
            case "FastVirus":
                return VirusName.FastVirus;
            case "CureVirus":
                return VirusName.CureVirus;
            case "CollisionVirus":
                return VirusName.CollisionVirus;
            case "SlowDownVirus":
                return VirusName.SlowDownVirus;
            case "RegenerativeVirus":
                return VirusName.RegenerativeVirus;
            case "SwallowVirus":
                return VirusName.SwallowVirus;
            case "ExplosiveVirus":
                return VirusName.ExplosiveVirus;
            case "AdsorptionVirus":
                return VirusName.AdsorptionVirus;
            case "DefendingVirus":
                return VirusName.DefendingVirus;
            case "TrackingVirus":
                return VirusName.TrackingVirus;
            case "DartVirus":
                return VirusName.DartVirus;
            case "LaunchVirus":
                return VirusName.LaunchVirus;
            case "VampireVirus":
                return VirusName.VampireVirus;
            case "ExpansionVirus":
                return VirusName.ExpansionVirus;
            case "ThreePointsVirus":
                return VirusName.ThreePointsVirus;
            default:
                return VirusName.FastVirus;
        }
    }

    private static VirusName GetBossVirusName(int level)
    {
        switch (level)
        {
            case 2: return VirusName.FastVirus;
            case 5: return VirusName.CureVirus;
            case 10: return VirusName.CollisionVirus;
            case 15: return VirusName.SlowDownVirus;
            case 20: return VirusName.RegenerativeVirus;
            case 25: return VirusName.SwallowVirus;
            case 30: return VirusName.ExplosiveVirus;
            case 35: return VirusName.AdsorptionVirus;
            case 40: return VirusName.TrackingVirus;
            case 45: return VirusName.ExpansionVirus;
            case 50: return VirusName.ThreePointsVirus;
            case 55: return VirusName.LaunchVirus;
            case 60: return VirusName.VampireVirus;
            case 65: return VirusName.DefendingVirus;
            case 70:
                     _lastBossName = VirusName.DartVirus;
                     return VirusName.DartVirus;
            default:
                     VirusName bossName;
                     while (true)
                     {
                         int index = Random.Range(2, 16);
                         int t = (int)_lastBossName;
                         if (Mathf.Abs(index - t) > 5)
                         {
                             bossName = (VirusName)index;
                             break;
                         }
                     }
                     _lastBossName = bossName;
                     return bossName;
        }
    }

    private static void FillNameRoll()
    {
        for (int i = 0; i < _virusNames.Count; i++)
        {
            if (i == 0 || i == _virusNames.Count - 1)
                _nameRoll.Add(20);
            else
                _nameRoll.Add(5);
        }
    }

    private static void FillColorRoll(int level)
    {
        _colorRoll = new ChanceRoll();
        if (level == 1)
        {
            FillColorRoll(new List<float> { 0, 0, 0, 0, 0, 20, 30, 40, 100 });
        }
        if (level >= 2 && level <= 5)
        {
            FillColorRoll(new List<float> { 10, 10, 10, 10, 10, 30, 60, 60, 80 });
        }
        if (level > 5 && level <= 10)
        {
            FillColorRoll(new List<float> { 20, 20, 20, 20, 20, 30, 50, 50, 80 });
        }
        if (level > 10)
        {
            List<float> vv = new List<float> { 20, 20, 20, 20, 20, 30, 50, 50, 80 };
            for (int i = 0; i < vv.Count; i++)
            {
                vv[i] += level * 5;
            }
            FillColorRoll(vv);
        }
    }

    private static void InitFillName()
    {
        _virusNames = new List<VirusName>();
    }

    private static void FillName(VirusName name)
    {
        if (_virusNames.Contains(name) == false)
        {
            _virusNames.Add(name);
        }
    }

    private static void FillVirusNames(int level)
    {
        if (IGamerProfile.Instance != null)
        {
            InitFillName();
            int index = level - 1;
            FtGameLevel.MapData mpDt = IGamerProfile.gameLevel.mapData[index];
            string bossType = mpDt.bossData.BossType;
            if (bossType != "")
            {
                FillName(GetBossVirusName(bossType));
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.AdsorptionVirus)
            {
                FillName(VirusName.AdsorptionVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.CollisionVirus)
            {
                FillName(VirusName.CollisionVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.CureVirus)
            {
                FillName(VirusName.CureVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.DartVirus)
            {
                FillName(VirusName.DartVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.DefendingVirus)
            {
                FillName(VirusName.DefendingVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.ExpansionVirus)
            {
                FillName(VirusName.ExpansionVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.ExplosiveVirus)
            {
                FillName(VirusName.ExplosiveVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.FastVirus)
            {
                FillName(VirusName.FastVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.LaunchVirus)
            {
                FillName(VirusName.LaunchVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.NormalVirus)
            {
                FillName(VirusName.NormalVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.RegenerativeVirus)
            {
                FillName(VirusName.RegenerativeVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.SlowDownVirus)
            {
                FillName(VirusName.SlowDownVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.SwallowVirus)
            {
                FillName(VirusName.SwallowVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.ThreePointsVirus)
            {
                FillName(VirusName.ThreePointsVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.TrackingVirus)
            {
                FillName(VirusName.TrackingVirus);
            }
            if (FtGameLevel.MapData.SpawnState.Spawn == mpDt.spawnVirusData.VampireVirus)
            {
                FillName(VirusName.VampireVirus);
            }
        }
        else
        {
            if (level <= 2)
            {
                FillNames(1);
                //FillNames(16); //testSyq
            }
            else if (level <= 5)
            {
                FillNames(2);
            }
            else if (level <= 70)
            {
                int c = (level - 1) / 5;
                FillNames(c + 2);
            }
            else
            {
                FillNames();
            }
        }
    }

    private static void FillNames(int count)
    {
        _virusNames = new List<VirusName>();
        //_virusNames.Add(VirusName.ThreePointsVirus); return; //testSyq

        for (int i = 0; i < count; i++)
        {
            VirusName virusName = (VirusName)(1 + i);
            _virusNames.Add(virusName);
        }
    }

    private static void FillNames()
    {
        _virusNames = new List<VirusName>();
        for (int i = 1; i <= 16; i++)
        {
            VirusName virusName = (VirusName)i;
            _virusNames.Add(virusName);
        }
        if (_lastBossName > 0)
            _virusNames.Remove(_lastBossName);
        if (_curBossName > 0)
            _virusNames.Remove(_curBossName);
        if (_lastBossName > 0)
            _virusNames.Add(_lastBossName);

        _virusNames.Remove(VirusName.SwallowVirus);
        //_virusNames.Remove(VirusName.AdsorptionVirus);
    }

    private static void FillColorRoll(List<float> rollFloats)
    {
        for (int i = 0; i < rollFloats.Count; i++)
        {
            _colorRoll.Add(rollFloats[i]);
        }
    }


}

