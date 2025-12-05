using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class DamageFlashSimple : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private SkinnedMeshRenderer skinnedRenderer;

    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashIntensity = 2f;         // brillo / multiplicador
    [SerializeField] private float flashTime = 0.12f;

    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorID = Shader.PropertyToID("_Color");

    private Color[] originalColors;
    private bool[] hasBaseColorProp;

    void Start()
    {
        if (skinnedRenderer == null)
        {
            skinnedRenderer = GetComponent<SkinnedMeshRenderer>();
            if (skinnedRenderer == null)
            {
                Debug.LogError("DamageFlashSimple: No hay SkinnedMeshRenderer asignado.");
                enabled = false;
                return;
            }
        }

        var mats = skinnedRenderer.materials;
        int n = mats.Length;
        originalColors = new Color[n];
        hasBaseColorProp = new bool[n];

        for (int i = 0; i < n; i++)
        {
            Material m = mats[i];
            if (m.HasProperty(BaseColorID))
            {
                originalColors[i] = m.GetColor(BaseColorID);
                hasBaseColorProp[i] = true;
            }
            else if (m.HasProperty(ColorID))
            {
                originalColors[i] = m.GetColor(ColorID);
                hasBaseColorProp[i] = false;
            }
            else
            {
                originalColors[i] = Color.white;
            }
        }
    }

    public void Flash()
    {
        if (!enabled) return;
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        var mats = skinnedRenderer.materials;

        // Aplicar color * intensidad
        Color appliedColor = flashColor * flashIntensity;

        for (int i = 0; i < mats.Length; i++)
        {
            Material m = mats[i];
            if (m == null) continue;

            if (hasBaseColorProp[i])
                m.SetColor(BaseColorID, appliedColor);
            else
                m.SetColor(ColorID, appliedColor);
        }

        skinnedRenderer.materials = mats;

        yield return new WaitForSeconds(flashTime);

        // Restaurar color original
        mats = skinnedRenderer.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            Material m = mats[i];
            if (m == null) continue;

            if (hasBaseColorProp[i])
                m.SetColor(BaseColorID, originalColors[i]);
            else
                m.SetColor(ColorID, originalColors[i]);
        }

        skinnedRenderer.materials = mats;
    }
}
