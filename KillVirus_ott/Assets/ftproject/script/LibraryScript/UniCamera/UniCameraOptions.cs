using UnityEngine;
using System;
using System.Collections.Generic;

[AddComponentMenu("UniCamera/UniCameraOptions")]
class UniCameraOptions : MonoBehaviourIgnoreGui
{
    private const int LAYERMASKCOUNT = 32;
    //û����ȷ���õ�Ĭ�ϲ�ľ���
    public float DefineLayerDistance = 100.0f;
    [System.Serializable]
    public class LayerDistance
    {
        public LayerMask layerMask = 0;
        public float Distance = 0.0f;
    }
    //û�����õĲ�ʹ��Ĭ�Ͼ���
    //�������Ϊ��ı�ʾʹ���������Զ�ü�ƽ��
    public LayerDistance[] layerDistance = null;

    //�Ƿ����βü�
    public bool layerCullSpherical = false;
    //͸������ģʽ
    public TransparencySortMode transparencySortMode = TransparencySortMode.Default;
    //�Ƿ�ʹ���ڵ��޳�
    //��������������Ҫ�ر������޳�
    public bool useOcclusionCulling = true;

    //�¼����Ʋ�,ʹ����������������������Ⱦ����������Ӧ�����Ϣ��
    //һ���������Ӧ�������㶼����Ϊ�㡣
    public LayerMask eventLayerMask = 0;
    private void SetLayerDistance(Camera myCamera)
    {
        //���ȴ���������Ĳ��趨
        try
        {
            float[] setLayerDistance = new float[LAYERMASKCOUNT];
            for (int i = 0; i < LAYERMASKCOUNT; i++)
            {
                setLayerDistance[i] = DefineLayerDistance;
            }
            if (layerDistance != null)
            {
                for (int i = 0; i < layerDistance.Length; i++)
                {
                    int value = LayerMaskChangeIndex(layerDistance[i].layerMask);
                    if (value <= 0 || value >= LAYERMASKCOUNT)
                        throw new Exception("Layer Distance Set Err!");
                    setLayerDistance[value] = layerDistance[i].Distance;
                }
            }
            myCamera.layerCullDistances = setLayerDistance;
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError(ex.ToString());
        }
        
    }

    public static int LayerMaskChangeIndex(LayerMask layerMask)
    {
        int nlayerMask = layerMask.value;
        int nIndex = -1;
        if (nlayerMask < 0)
        {
            return -1;
        }
        while (nlayerMask > 0)
        {
            nlayerMask = nlayerMask >> 1;
            nIndex++;
        }
        return nIndex;
    }

    //�Ƿ�������������
    public bool IsSetAspect = false;
    //���õĳ����,Ĭ�� 1280��720;
    public float Aspect = 1.78f;
    protected override void Awake()
    {
        base.Awake();
        Camera myCamera = GetComponent<Camera>();
        SetLayerDistance(myCamera);
        myCamera.layerCullSpherical = layerCullSpherical;
        myCamera.transparencySortMode = transparencySortMode;
        myCamera.useOcclusionCulling = useOcclusionCulling;
        //4.3��֧��
        myCamera.eventMask = eventLayerMask.value;
        if (IsSetAspect)
        {
            myCamera.aspect = Aspect;
            myCamera.ResetAspect();
        }
        
    }
}
