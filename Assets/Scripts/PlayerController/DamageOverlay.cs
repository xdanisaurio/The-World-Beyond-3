// DamageOverlay.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageOverlay : MonoBehaviour
{
    [Header("Image que hace el overlay")]
    public Image overlayImage;

    [Header("Alpha m�xima cuando recibe da�o")]
    public float maxAlpha = 0.35f;

    [Header("Velocidad (unidades por segundo)")]
    public float fadeInSpeed = 8f;
    public float fadeOutSpeed = 2f;

    private Coroutine runningRoutine;

    private void Start()
    {
        if (overlayImage == null)
            overlayImage = GetComponent<Image>();

        // Asegurar que empiece invisible
        if (overlayImage != null)
        {
            SetAlpha(0f);
        }
    }

    // M�todo amigable: puede llamarse desde otros scripts
    public void Flash()
    {
        // Compatibilidad: Flash() y ShowDamage() hacen lo mismo
        ShowDamage();
    }

    // Nombre alternativo que tambi�n exist�a antes
    public void ShowDamage()
    {
        if (overlayImage == null) return;

        // reinicia coroutine para evitar conflictos
        if (runningRoutine != null) StopCoroutine(runningRoutine);
        runningRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // FADE IN
        float a = overlayImage.color.a;
        while (a < maxAlpha)
        {
            a += Time.deltaTime * fadeInSpeed;
            if (a > maxAlpha) a = maxAlpha;
            SetAlpha(a);
            yield return null;
        }

        // FADE OUT
        while (a > 0f)
        {
            a -= Time.deltaTime * fadeOutSpeed;
            if (a < 0f) a = 0f;
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(0f);
        runningRoutine = null;
    }

    private void SetAlpha(float a)
    {
        if (overlayImage == null) return;
        Color c = overlayImage.color;
        c.a = a;
        overlayImage.color = c;
    }
}

