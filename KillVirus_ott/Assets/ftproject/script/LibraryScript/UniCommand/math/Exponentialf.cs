using System;
/*
 * ���߷���Ϊ��y=a*b^(x/c)
 * ������Ϊ��x
 * �����Ϊ: y
 * 
 * */
struct Exponentialf
{
    //����ȡ�κ�ֵ
    public float m_a;
    //����ȡ-�޵��ޣ�����Ϊ1
    public float m_b;
    // Ĭ��Ϊ1.0������Ϊ0
    public float m_c;
    public Exponentialf(float a, float b, float c)
    {
        m_a = a;
        //b����Ϊ1
        if (UnityEngine.Mathf.Abs(b - 1.0f) > UnityEngine.Mathf.Epsilon)
            m_b = b;
        else
            throw new Exception("Exponential: b cannot equal to 1.0");
        //c����Ϊ0
        if (UnityEngine.Mathf.Abs(c - 0.0f) > UnityEngine.Mathf.Epsilon)
            m_c = c;
        else
            throw new Exception("Exponential: c cannot equal to 0");
    }

    //x����ȡ����ֵ
    public float GetValue(float x)
    {
        return m_a * UnityEngine.Mathf.Pow(m_b, x / m_c);
    }
}


[System.Serializable]
class SerExponentialf
{
    //����ȡ�κ�ֵ
    public float m_a;
    //����ȡ-�޵��ޣ�����Ϊ1
    public float m_b;
    // Ĭ��Ϊ1.0������Ϊ0
    public float m_c;
    public SerExponentialf(float a, float b, float c)
    {
        m_a = a;
        //b����Ϊ1
        if (UnityEngine.Mathf.Abs(b - 1.0f) > UnityEngine.Mathf.Epsilon)
            m_b = b;
        else
            throw new Exception("Exponential: b cannot equal to 1.0");
        //c����Ϊ0
        if (UnityEngine.Mathf.Abs(c - 0.0f) > UnityEngine.Mathf.Epsilon)
            m_c = c;
        else
            throw new Exception("Exponential: c cannot equal to 0");
    }

    //x����ȡ����ֵ
    public float GetValue(float x)
    {
        return m_a * UnityEngine.Mathf.Pow(m_b, x / m_c);
    }
}