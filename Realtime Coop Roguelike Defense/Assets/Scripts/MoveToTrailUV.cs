using UnityEngine;

// Trail Renderer�� Head�� ������ �� �̵��� �Ÿ��� ������ ��ũ�� UV�� �����ϴ� ��ũ��Ʈ
// Trail Renderer�� Texture Mode�� Tile�̿�����
// ������ ���޵Ǵ� ���� 0~1 ������ ���̴�.
[ExecuteAlways]
public class MoveToTrailUV : MonoBehaviour
{
    [System.Serializable]
    public struct MaterialData
    {
        public MaterialData(TrailRenderer trailRenderer, Material material, Vector2 uvScale, float move)
        {
            m_trailRenderer = trailRenderer;
            m_uvTiling = uvScale;
            m_move = move;
        }

        public TrailRenderer m_trailRenderer;
        [HideInInspector] public Vector2 m_uvTiling;
        [HideInInspector] public float m_move;
    }

#if UNITY_EDITOR
    //public bool m_overrideMaterial = true;
#endif
    public Transform m_moveObject;
    public string m_shaderPropertyName = "_MoveToMaterialUV"; // ���̴����� UV ���� �޾Ƶ��� ������Ƽ �̸�
    public int m_shaderPropertyID; // ���̴� ������Ƽ�� ���ڿ��� ������� �ʱ� ���� ID
    public MaterialData[] m_materialData = new MaterialData[1] { new MaterialData(null, null, new Vector2(1, 1), 0f) };

    private Vector3 m_beforePosW = Vector3.zero;
    void Start()
    {
        Initialize();
    }

    void LateUpdate()
    {
        if (m_moveObject == null)
            return;
        if (m_materialData == null || m_materialData.Length == 0)
            return;

        Vector3 nowPosW = m_moveObject.transform.position;
        if (nowPosW == m_beforePosW)
            return; // ��ġ ��ȭ�� ������ �ƹ� �۾��� ����

        float distance = Vector3.Distance(nowPosW, m_beforePosW);
        m_beforePosW = nowPosW;

        for (int i = 0; i < m_materialData.Length; i++)
        {
            if (m_materialData[i].m_trailRenderer == null)
                continue;

            m_materialData[i].m_move += distance * m_materialData[i].m_uvTiling.x;
            // m_move ���� ����ġ�� Ŀ���� �ʵ��� �ϱ� ���� 1 �̻��� ������ ���� ����. (�̹� m_uvTiling.x �� ������ ���̾����)
            if (m_materialData[i].m_move > 1f)
            {
                m_materialData[i].m_move = m_materialData[i].m_move % 1f;
            }

            // ������Ƽ ���� üũ ���� ���. ������Ƽ�� �����ϸ� ���� ������ ��� ����� ������ ó���Ǵ� ������ ����.
            TrailRenderer trailRenderer = m_materialData[i].m_trailRenderer;
            if (trailRenderer != null)
            {
                Material mat = trailRenderer.sharedMaterial;
                if (mat != null)
                {
                    mat.SetFloat(m_shaderPropertyID, m_materialData[i].m_move);
                }
            }
        }
    }

    public void Initialize()
    {
        if (m_materialData == null || m_materialData.Length == 0)
            return;

        m_shaderPropertyID = Shader.PropertyToID(m_shaderPropertyName);

        for (int i = 0; i < m_materialData.Length; i++)
        {
            m_materialData[i].m_move = 0f;
            TrailRenderer trailRenderer = m_materialData[i].m_trailRenderer;
            if (trailRenderer != null)
            {
                Material mat = trailRenderer.sharedMaterial;
                if (mat != null)
                {
                    m_materialData[i].m_uvTiling = mat.mainTextureScale;
                }
            }
        }
    }
}