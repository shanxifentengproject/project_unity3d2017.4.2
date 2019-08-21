using System;
using UnityEngine;
struct SamplingColor
{
    //��ɫ������
    public Color[] colorSamplingPoint;
    private const float from = 0.0f;
    private const float to = 1.0f;
    public SamplingColor(params Color[] colorlist)
    {
        colorSamplingPoint = colorlist;
    }
    /// <summary>
    /// ����һ��ֵ����ò���ֵ
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Color GetValue(float v)
    {
        if (colorSamplingPoint.Length == 0)
            return Color.red;
        else if (v <= from || colorSamplingPoint.Length == 1)
            return colorSamplingPoint[0];
        else if (v >= to)
            return colorSamplingPoint[colorSamplingPoint.Length - 1];
        float idx = (colorSamplingPoint.Length - 1) * (v - from) / (to - from);
        return Color.Lerp(colorSamplingPoint[(int)idx], colorSamplingPoint[(int)idx + 1], idx - (int)idx);
    }
}






//������ɫ��������
//ʹ��Color�Ĳ�ֵ����
[System.Serializable]
class SerSamplingColor
{
    //��ɫ������
    public Color[] colorSamplingPoint = null;
    private const float from = 0.0f;
    private const float to = 1.0f;
    public SerSamplingColor(params Color[] colorlist)
    {
        colorSamplingPoint = colorlist;
    }
    /// <summary>
    /// ����һ��ֵ����ò���ֵ
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Color GetValue(float v)
    {
        if (colorSamplingPoint.Length == 0)
            return Color.red;
        else if (v <= from || colorSamplingPoint.Length == 1)
            return colorSamplingPoint[0];
        else if (v >= to)
            return colorSamplingPoint[colorSamplingPoint.Length - 1];
        float idx = (colorSamplingPoint.Length - 1) * (v - from) / (to - from);
        return Color.Lerp(colorSamplingPoint[(int)idx], colorSamplingPoint[(int)idx + 1], idx - (int)idx);
    }
}