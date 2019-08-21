using UnityEngine;

public class QyChangeMatrial : QyRoot
{
    public enum ChangeState
    {
        Null = -1,
        Old = 0,
        New = 1,
    }

    [System.Serializable]
    public class MeshMatData
    {
        public MeshRenderer m_Mesh;
        public Material oldMesh;
        public Material newMesh;
        public void ChangeMatrial(ChangeState st)
        {
            if (m_Mesh != null)
            {
                Material mat = null;
                switch (st)
                {
                    case ChangeState.Old:
                        {
                            mat = oldMesh;
                            break;
                        }
                    case ChangeState.New:
                        {
                            mat = newMesh;
                            break;
                        }
                }

                if (mat != null)
                {
                    m_Mesh.material = mat;
                }
            }
        }
    }
    public MeshMatData m_MeshMatData;

    public void ChangeMatrial(ChangeState st)
    {
        if (m_MeshMatData != null)
        {
            m_MeshMatData.ChangeMatrial(st);
        }
    }
}
