public class QyChangeMatrialManage : QyRoot
{
    public QyChangeMatrial[] m_ChangeMatrialArray;

    public void ChangeMatrial(QyChangeMatrial.ChangeState st)
    {
        for (int i = 0; i < m_ChangeMatrialArray.Length; i++)
        {
            m_ChangeMatrialArray[i].ChangeMatrial(st);
        }
    }
}
