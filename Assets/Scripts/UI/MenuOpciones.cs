using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    // 🔥 Nuevo: Dropdown y lista de resoluciones
    public TMP_Dropdown dropdownResoluciones;
    private Resolution[] resoluciones;

    void Start()
    {
        // Cargar resoluciones disponibles
        resoluciones = Screen.resolutions;
        dropdownResoluciones.ClearOptions();

        List<string> opciones = new List<string>();
        int indiceActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                indiceActual = i;
            }
        }

        dropdownResoluciones.AddOptions(opciones);
        dropdownResoluciones.value = indiceActual;
        dropdownResoluciones.RefreshShownValue();
    }

    // ⭐ Nuevo: Cambiar resolución
    public void CambiarResolucion(int index)
    {
        Resolution res = resoluciones[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void PantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    public void CambiarVolumen(float volumen)
    {
        audioMixer.SetFloat("Volumen", volumen);
    }

    public void CambiarCalidad(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
