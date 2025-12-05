using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextGlowOutlineScale_Unscaled : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text text;
    private Material mat;

    private bool hovering = false;

    [Header("Glow Settings")]
    public Color glowColor = Color.cyan;
    public float glowMax = 0.6f;
    public float glowMin = 0f;

    [Header("Outline Settings")]
    public float outlineMax = 0.22f;
    public float outlineMin = 0f;
    public Color outlineColor = Color.cyan;

    [Header("Scale Settings")]
    public float scaleMax = 1.08f;   // Aumento suave
    public float scaleMin = 1f;

    [Header("Transition Speed")]
    public float speed = 7f;

    private float glowValue = 0f;
    private float outlineValue = 0f;
    private float scaleValue = 1f;

    void Awake()
    {
        RefreshComponents();
    }

    void OnEnable()
    {
        // Se vuelve a ejecutar cuando el botón se activa durante timeScale=0
        RefreshComponents();
    }

    private void RefreshComponents()
    {
        text = GetComponentInChildren<TMP_Text>();
        text.ForceMeshUpdate(true);

        mat = text.fontMaterial;

        mat.SetFloat("_GlowPower", 0);
        mat.SetColor("_GlowColor", glowColor);

        mat.SetFloat("_OutlineWidth", 0);
        mat.SetColor("_OutlineColor", outlineColor);

        scaleValue = scaleMin;
        text.transform.localScale = Vector3.one * scaleMin;
    }

    void Update()
    {
        float delta = Time.unscaledDeltaTime; // funciona incluso en pausa

        float targetGlow = hovering ? glowMax : glowMin;
        float targetOutline = hovering ? outlineMax : outlineMin;
        float targetScale = hovering ? scaleMax : scaleMin;

        // transiciones suaves
        glowValue = Mathf.Lerp(glowValue, targetGlow, delta * speed);
        outlineValue = Mathf.Lerp(outlineValue, targetOutline, delta * speed);
        scaleValue = Mathf.Lerp(scaleValue, targetScale, delta * speed);

        // aplicar efectos
        mat.SetFloat("_GlowPower", glowValue);
        mat.SetFloat("_OutlineWidth", outlineValue);
        text.transform.localScale = Vector3.one * scaleValue;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
