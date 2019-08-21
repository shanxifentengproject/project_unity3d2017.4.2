using System;
using System.Collections;
using UnityEngine;
[AddComponentMenu("UniLua/UniLuaApplication/LuaScriptMonoBehaviour")]
class LuaScriptMonoBehaviour : MonoBehaviourIgnoreGui
{
    protected LuaScriptController Lua = null;
    
    //��ȡ������ʽ
    protected virtual LuaScriptType luaScriptType { get { return LuaScriptType.Type_Name; } }
    //��ȡ�ű�����
    protected virtual string LuaScriptName { get { return ""; } }
    //��ýű��ļ�·��
    protected virtual string LuaScriptFile { get { return ""; } }
    //��ýű��ַ���
    protected virtual string LuaScriptString { get { return ""; } }
    //��ýű��ֽ���
    protected virtual byte[] LuaScriptBytes { get { return null; } }
    protected virtual string LuaScriptBytesName { get { return ""; } }
    //��ð󶨺�������
    protected virtual uint LuaStoreMethodMask { get { return 0xFFFFFFFF; } }
    //��ð󶨺����ص�
    protected virtual LuaScriptController.OnStoreMethod LuaStoreMethod { get { return null; } }
    //��ʼ��LUA
    protected virtual void InitializationLua()
    {
        try
        {
            Lua = new LuaScriptController();
            switch (luaScriptType)
            {
                case LuaScriptType.Type_Name:
                    Lua.InitializationName(LuaScriptName, LuaStoreMethodMask, LuaStoreMethod);
                    break;
                case LuaScriptType.Type_File:
                    Lua.InitializationFile(LuaScriptFile, LuaStoreMethodMask, LuaStoreMethod);
                    break;
                case LuaScriptType.Type_String:
                    Lua.Initialization(LuaScriptString, LuaStoreMethodMask, LuaStoreMethod);
                    break;
                case LuaScriptType.Type_Bytes:
                    Lua.Initialization(LuaScriptBytes, LuaScriptBytesName, LuaStoreMethodMask, LuaStoreMethod);
                    break;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }
}