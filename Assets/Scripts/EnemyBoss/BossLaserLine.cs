using UnityEngine;

public class BossLaserLine : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform shootPivot;

    [Header("Ajustes visuales")]
    [Range(0f, 1f)] public float alpha = 0.2f;
    public float width = 0.08f;
    public Color color = Color.magenta;


    private LineRenderer lr;


    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        lr.SetPosition(0, shootPivot.position);
        lr.SetPosition(1, player.position);

        Color finalColor = color;
        finalColor.a = alpha;

        lr.startColor = finalColor;
        lr.endColor = finalColor;
        lr.startWidth = width;
        lr.endWidth = width;
    }
}
