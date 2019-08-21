using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTLibrary.XML;

class FtGameLevel
{
#if _GameType_BaoYue
    private const string fileName = "gamelevel_by.xml";
#else
    private const string fileName = "gamelevel.xml";
#endif
    public struct NpcHealth
    {
        public int NormalVirus;
        public int FastVirus;
        public int CureVirus;
        public int CollisionVirus;
        public int SlowDownVirus;
        public int RegenerativeVirus;
        public int SwallowVirus;
        public int ExplosiveVirus;
        public int AdsorptionVirus;
        public int TrackingVirus;
        public int ExpansionVirus;
        public int ThreePointsVirus;
        public int LaunchVirus;
        public int VampireVirus;
        public int DefendingVirus;
        public int DartVirus;
    }
    /// <summary>
    /// 计算npc最大血量时使用
    /// </summary>
    NpcHealth npcHealth;
    public int GetNpcHealth(int index)
    {
        switch (index)
        {
            case 1:
                {
                    return npcHealth.NormalVirus;
                }
            case 2:
                {
                    return npcHealth.FastVirus;
                }
            case 3:
                {
                    return npcHealth.CureVirus;
                }
            case 4:
                {
                    return npcHealth.CollisionVirus;
                }
            case 5:
                {
                    return npcHealth.SlowDownVirus;
                }
            case 6:
                {
                    return npcHealth.RegenerativeVirus;
                }
            case 7:
                {
                    return npcHealth.SwallowVirus;
                }
            case 8:
                {
                    return npcHealth.ExplosiveVirus;
                }
            case 9:
                {
                    return npcHealth.AdsorptionVirus;
                }
            case 10:
                {
                    return npcHealth.TrackingVirus;
                }
            case 11:
                {
                    return npcHealth.ExpansionVirus;
                }
            case 12:
                {
                    return npcHealth.ThreePointsVirus;
                }
            case 13:
                {
                    return npcHealth.LaunchVirus;
                }
            case 14:
                {
                    return npcHealth.VampireVirus;
                }
            case 15:
                {
                    return npcHealth.DefendingVirus;
                }
            case 16:
                {
                    return npcHealth.DartVirus;
                }
            default:
                {
                    return 0;
                }
        }
    }

    public struct MapData
    {
        public struct LevelData
        {
            public int time;
            public int missiontarget;
            public int winawardmoney;
            public int failawardmoney;
            public int oneemenymoney;
            public int oneemenyhp;
        }
        public LevelData[] levelData;
        public string scenename;

        public struct BossData
        {
            public string BossType;
        }
        public BossData bossData;

        public enum SpawnState
        {
            NotSpawn = 0,
            Spawn = 1,
        }

        public struct SpawnVirusData
        {
            public SpawnState NormalVirus;
            public SpawnState FastVirus;
            public SpawnState CureVirus;
            public SpawnState CollisionVirus;
            public SpawnState SlowDownVirus;
            public SpawnState RegenerativeVirus;
            public SpawnState SwallowVirus;
            public SpawnState ExplosiveVirus;
            public SpawnState AdsorptionVirus;
            public SpawnState TrackingVirus;
            public SpawnState ExpansionVirus;
            public SpawnState ThreePointsVirus;
            public SpawnState LaunchVirus;
            public SpawnState VampireVirus;
            public SpawnState DefendingVirus;
            public SpawnState DartVirus;
        }
        public SpawnVirusData spawnVirusData;

        public struct SpawnData
        {
            int _NpcNum;
            public int NpcNum
            {
                set
                {
                    if (value != _NpcNum)
                    {
                        if (value > 1)
                        {
                            _NpcNum = value;
                        }
                        else
                        {
                            _NpcNum = 1;
                        }
                    }
                }
                get
                {
                    return _NpcNum;
                }
            }
        }
        public SpawnData[] spawnDatas;
    }
    public MapData[] mapData = null;
    //为了方便计算这里计算出每张地图的关卡数
    public int[] mapMaxLevel = null;

    public void Load()
    {
        XmlDocument doc = GameRoot.gameResource.LoadResource_XmlFile(fileName);
        XmlNode root = doc.SelectSingleNode("GameLevel");
        XmlNode node = root.SelectSingleNode("LevelData");
        XmlNodeList itemlist = node.SelectNodes("Map");
        mapData = new MapData[itemlist.Count];
        for (int i = 0; i < mapData.Length; i++)
        {
            XmlNode mapNode = itemlist[i];
            mapData[i].scenename = mapNode.Attribute("scenename");
            XmlNodeList nodelist = mapNode.SelectNodes("Level");
            mapData[i].levelData = new MapData.LevelData[nodelist.Count];
            for (int j = 0; j < mapData[i].levelData.Length; j++)
            {
                XmlNode n = nodelist[j];
                mapData[i].levelData[j].time = Convert.ToInt32(n.Attribute("time"));
                mapData[i].levelData[j].missiontarget = Convert.ToInt32(n.Attribute("missiontarget"));
                mapData[i].levelData[j].winawardmoney = Convert.ToInt32(n.Attribute("winawardmoney"));
                mapData[i].levelData[j].failawardmoney = Convert.ToInt32(n.Attribute("failawardmoney"));
                mapData[i].levelData[j].oneemenymoney = Convert.ToInt32(n.Attribute("oneemenymoney"));
                mapData[i].levelData[j].oneemenyhp = Convert.ToInt32(n.Attribute("oneemenyhp"));
            }

            XmlNodeList bosslist = mapNode.SelectNodes("Boss");
            mapData[i].bossData = new MapData.BossData{ BossType = bosslist[0].Attribute("BossType") };

            XmlNodeList spawnViruslist = mapNode.SelectNodes("SpawnVirus");
            mapData[i].spawnVirusData = new MapData.SpawnVirusData();
            if (spawnViruslist.Count > 0)
            {
                XmlNode n = spawnViruslist[0];
                mapData[i].spawnVirusData.NormalVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("NormalVirus"));
                mapData[i].spawnVirusData.FastVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("FastVirus"));
                mapData[i].spawnVirusData.CureVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("CureVirus"));
                mapData[i].spawnVirusData.CollisionVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("CollisionVirus"));
                mapData[i].spawnVirusData.SlowDownVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("SlowDownVirus"));
                mapData[i].spawnVirusData.RegenerativeVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("RegenerativeVirus"));
                mapData[i].spawnVirusData.SwallowVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("SwallowVirus"));
                mapData[i].spawnVirusData.ExplosiveVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("ExplosiveVirus"));
                mapData[i].spawnVirusData.AdsorptionVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("AdsorptionVirus"));
                mapData[i].spawnVirusData.TrackingVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("TrackingVirus"));
                mapData[i].spawnVirusData.ExpansionVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("ExpansionVirus"));
                mapData[i].spawnVirusData.ThreePointsVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("ThreePointsVirus"));
                mapData[i].spawnVirusData.LaunchVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("LaunchVirus"));
                mapData[i].spawnVirusData.VampireVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("VampireVirus"));
                mapData[i].spawnVirusData.DefendingVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("DefendingVirus"));
                mapData[i].spawnVirusData.DartVirus = (MapData.SpawnState)Convert.ToByte(n.Attribute("DartVirus"));
            }

            XmlNodeList spawnlist = mapNode.SelectNodes("Spawn");
            mapData[i].spawnDatas = new MapData.SpawnData[spawnlist.Count];
            for (int j = 0; j < mapData[i].spawnDatas.Length; j++)
            {
                XmlNode n = spawnlist[j];
                mapData[i].spawnDatas[j].NpcNum = Convert.ToInt32(n.Attribute("NpcNum"));
            }
        }

        mapMaxLevel = new int[mapData.Length];
        for (int i = 0; i < mapMaxLevel.Length; i++)
        {
            mapMaxLevel[i] = mapData[i].levelData.Length;
        }

        node = root.SelectSingleNode("GameData");
        itemlist = node.SelectNodes("NpcHealth");
        if (itemlist.Count > 0)
        {
            XmlNode npcHealthNode = itemlist[0];
            XmlNodeList nodelist = npcHealthNode.SelectNodes("Health");
            XmlNode healthNode = nodelist[0];
            npcHealth = new NpcHealth
            {
                NormalVirus = Convert.ToInt32(healthNode.Attribute("NormalVirus")),
                FastVirus = Convert.ToInt32(healthNode.Attribute("FastVirus")),
                CureVirus = Convert.ToInt32(healthNode.Attribute("CureVirus")),
                CollisionVirus = Convert.ToInt32(healthNode.Attribute("CollisionVirus")),
                SlowDownVirus = Convert.ToInt32(healthNode.Attribute("SlowDownVirus")),
                RegenerativeVirus = Convert.ToInt32(healthNode.Attribute("RegenerativeVirus")),
                SwallowVirus = Convert.ToInt32(healthNode.Attribute("SwallowVirus")),
                ExplosiveVirus = Convert.ToInt32(healthNode.Attribute("ExplosiveVirus")),
                AdsorptionVirus = Convert.ToInt32(healthNode.Attribute("AdsorptionVirus")),
                TrackingVirus = Convert.ToInt32(healthNode.Attribute("TrackingVirus")),
                ExpansionVirus = Convert.ToInt32(healthNode.Attribute("ExpansionVirus")),
                ThreePointsVirus = Convert.ToInt32(healthNode.Attribute("ThreePointsVirus")),
                LaunchVirus = Convert.ToInt32(healthNode.Attribute("LaunchVirus")),
                VampireVirus = Convert.ToInt32(healthNode.Attribute("VampireVirus")),
                DefendingVirus = Convert.ToInt32(healthNode.Attribute("DefendingVirus")),
                DartVirus = Convert.ToInt32(healthNode.Attribute("DartVirus"))
            };
        }
    }
}

