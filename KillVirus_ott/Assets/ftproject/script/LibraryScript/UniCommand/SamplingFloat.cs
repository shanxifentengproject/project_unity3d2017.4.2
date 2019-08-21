using System;
using UnityEngine;
//�����������
[System.Serializable]
struct SamplingFloat
{
    //ǿ�Ȳ����㣬Ĭ��Ϊȫ�ֵĲ�����
    public float[] FloatSamplingPoint;
    private const float from = 0.0f;
    private const float to = 1.0f;
    public SamplingFloat(params float[] pointlist)
    {
        FloatSamplingPoint = pointlist;
    }
    /// <summary>
    /// ����һ��ֵ����ò���ֵ
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float GetValue(float v)
    {
        if (FloatSamplingPoint.Length == 0)
            return 0.0f;
        else if (v <= from || FloatSamplingPoint.Length == 1)
            return FloatSamplingPoint[0];
        else if (v >= to)
            return FloatSamplingPoint[FloatSamplingPoint.Length - 1];
        float idx = (FloatSamplingPoint.Length - 1) * (v - from) / (to - from);
        return Mathf.Lerp(FloatSamplingPoint[(int)idx], FloatSamplingPoint[(int)idx + 1], idx - (int)idx);
    }
}
//�����������
[System.Serializable]
class SerSamplingFloat
{
    //ǿ�Ȳ����㣬Ĭ��Ϊȫ�ֵĲ�����
    public float[] FloatSamplingPoint;
    private const float from = 0.0f;
    private const float to = 1.0f;
    public SerSamplingFloat(params float[] pointlist)
    {
        FloatSamplingPoint = pointlist;
    }
    /// <summary>
    /// ����һ��ֵ����ò���ֵ
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float GetValue(float v)
    {
        if (FloatSamplingPoint.Length == 0)
            return 0.0f;
        else if (v <= from || FloatSamplingPoint.Length == 1)
            return FloatSamplingPoint[0];
        else if (v >= to)
            return FloatSamplingPoint[FloatSamplingPoint.Length - 1];
        float idx = (FloatSamplingPoint.Length - 1) * (v - from) / (to - from);
        return Mathf.Lerp(FloatSamplingPoint[(int)idx], FloatSamplingPoint[(int)idx + 1], idx - (int)idx);
    }
}