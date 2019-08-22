using System;
using System.Collections.Generic;
using FTLibrary.XML;

class GameCharacter
{
#if _GameType_BaoYue
    private const string fileName = "gamecharacter_by.xml";
#else
    private const string fileName = "gamecharacter.xml";
#endif
    public struct CharacterData
    {
        public int maxlevelA;
        public int maxlevelB;
        /// <summary>
        /// 多少关之后可以解锁该装备(关闭该功能)
        /// </summary>
        public int unlock;
        /// <summary>
        /// 装备购买价格
        /// </summary>
        public int buyMoney;
        public struct Curvei2
        {
            public int A;
            public int B;
            public Curvei2(int a, int b)
            {
                A = a;
                B = b;
            }
            //x可以取任意值
            public int GetValue(int x)
            {
                return B + A * x;
            }
        }
        public Curvei2 LevelAToMoney;
        public Curvei2 LevelBToMoney;
        public Curvei2 LevelAToVal;
        public Curvei2 LevelBToVal;
        /// <summary>
        /// 用于属性A的真实数值计算范围
        /// </summary>
        public Curvef2 LevelARange;
        /// <summary>
        /// 用于属性B的真实数值计算范围
        /// </summary>
        public Curvef2 LevelBRange;
        public Curvef2 LevelCRange;
        public Curvef2 LevelDRange;
    }
    public CharacterData[] characterDataList = null;
    public void Load()
    {
        XmlDocument doc = GameRoot.gameResource.LoadResource_XmlFile(fileName);
        XmlNode root = doc.SelectSingleNode("GameCharacter");
        XmlNode node = root.SelectSingleNode("CharacterData");
        XmlNodeList nodelist = node.SelectNodes("Character");
        XmlNode n, n1;
        characterDataList = new CharacterData[nodelist.Count];
        for (int i = 0; i < characterDataList.Length; i++)
        {
            n = nodelist[i];

            characterDataList[i].maxlevelA = Convert.ToInt32(n.Attribute("maxlevelA"));
            characterDataList[i].maxlevelB = Convert.ToInt32(n.Attribute("maxlevelB"));
            characterDataList[i].unlock = Convert.ToInt32(n.Attribute("unlock"));
            characterDataList[i].buyMoney = Convert.ToInt32(n.Attribute("buyMoney"));

            n1 = n.SelectSingleNode("LevelAToMoney");
            characterDataList[i].LevelAToMoney = new CharacterData.Curvei2(Convert.ToInt32(n1.Attribute("a")),
                Convert.ToInt32(n1.Attribute("b")));
            n1 = n.SelectSingleNode("LevelBToMoney");
            characterDataList[i].LevelBToMoney = new CharacterData.Curvei2(Convert.ToInt32(n1.Attribute("a")),
                Convert.ToInt32(n1.Attribute("b")));
            n1 = n.SelectSingleNode("LevelAToVal");
            characterDataList[i].LevelAToVal = new CharacterData.Curvei2(Convert.ToInt32(n1.Attribute("a")),
                Convert.ToInt32(n1.Attribute("b")));
            n1 = n.SelectSingleNode("LevelBToVal");
            characterDataList[i].LevelBToVal = new CharacterData.Curvei2(Convert.ToInt32(n1.Attribute("a")),
                Convert.ToInt32(n1.Attribute("b")));
            n1 = n.SelectSingleNode("LevelARange");
            characterDataList[i].LevelARange = new Curvef2(QyConvert.StringToFloat(n1.Attribute("min")),
                QyConvert.StringToFloat(n1.Attribute("max")));
            n1 = n.SelectSingleNode("LevelBRange");
            characterDataList[i].LevelBRange = new Curvef2(QyConvert.StringToFloat(n1.Attribute("min")),
                QyConvert.StringToFloat(n1.Attribute("max")));
            n1 = n.SelectSingleNode("LevelCRange");
            characterDataList[i].LevelCRange = new Curvef2(QyConvert.StringToFloat(n1.Attribute("min")),
                QyConvert.StringToFloat(n1.Attribute("max")));
            n1 = n.SelectSingleNode("LevelDRange");
            characterDataList[i].LevelDRange = new Curvef2(QyConvert.StringToFloat(n1.Attribute("min")),
                QyConvert.StringToFloat(n1.Attribute("max")));
        }

    }
}