using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ConeVisualizer : MonoBehaviour
{
    [SerializeField] private float length = 5f;
    [SerializeField] private float angle = 60f;
    [SerializeField] private int segments = 20;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 2;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.05f;
    }

    void Update()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 origin = transform.position;
        lineRenderer.SetPosition(0, origin);

        float currentAngle = -angle;
        for (int i = 1; i <= segments; i++)
        {
            Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * forward;
            Vector3 point = origin + dir.normalized * length;
            lineRenderer.SetPosition(i, point);
            currentAngle += (2 * angle) / segments;
        }

        lineRenderer.SetPosition(segments + 1, origin);
    }
}
