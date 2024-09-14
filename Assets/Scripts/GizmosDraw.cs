using UnityEngine;

public class GizmosDraw : MonoBehaviour
{
    [SerializeField] private Color _gizmosColor;
    public Color GizmosColor
    {
        get { return _gizmosColor; }
        set { _gizmosColor = value; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

}
