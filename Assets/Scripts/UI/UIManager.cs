using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool bloqueoEscape = false;

    [Header("Paneles (solo en Level)")]
    public GameObject panelPausa;
    public GameObject panelGameOver;
    public GameObject panelGameplay;


    [Header("Panel Especial Trigger")]
    public GameObject panelTriggerEspecial;
    public GameObject panelTriggerEspecial2;
    public GameObject panelTriggerEspecial3;

    [Header("Panel Final del Juego")]
    public GameObject panelFinalJuego;


    [Header("Botones (Menu o Level)")]
    public Button botonJugar;
    public Button botonPausa;        // <-- AñADE ESTE SI QUIERES ASIGNARLO DESDE AQUi
    public Button botonReanudar;
    public Button botonReiniciar;
    public Button botonSalir;

    [Header("Botones Game Over")]
    public Button botonReintentarGO;
    public Button botonSalirGO;

    private void Start()
    {
        string escenaActual = SceneManager.GetActiveScene().name;

        if (botonJugar != null) botonJugar.onClick.AddListener(CargarNivel);
        if (botonReanudar != null) botonReanudar.onClick.AddListener(ReanudarJuego);
        if (botonReiniciar != null) botonReiniciar.onClick.AddListener(ReiniciarJuego);
        if (botonSalir != null) botonSalir.onClick.AddListener(SalirDelJuego);
        if (botonReintentarGO != null) botonReintentarGO.onClick.AddListener(ReiniciarJuego);
        if (botonSalirGO != null) botonSalirGO.onClick.AddListener(SalirDelJuego);

        
        if (botonPausa != null) botonPausa.onClick.AddListener(PausarJuego);

        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelTriggerEspecial != null) panelTriggerEspecial.SetActive(false);
        if (panelTriggerEspecial2 != null) panelTriggerEspecial2.SetActive(false);
        if (panelTriggerEspecial3 != null) panelTriggerEspecial3.SetActive(false);
        if (panelFinalJuego != null) panelFinalJuego.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        DesuscribirseEventos();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DesuscribirseEventos();
        SuscribirseEventos();

        if (scene.name != "Menu")
            GameManager.Instancia.IniciarJuego();
    }

    private void SuscribirseEventos()
    {
        if (GameManager.Instancia == null) return;
        if (SceneManager.GetActiveScene().name == "Menu") return;

        GameManager.Instancia.JuegoPausado += MostrarPausa;
        GameManager.Instancia.JuegoReanudado += OcultarPausa;
        GameManager.Instancia.JuegoFinalizado += MostrarGameOver;
    }

    private void DesuscribirseEventos()
    {
        if (GameManager.Instancia == null) return;

        GameManager.Instancia.JuegoPausado -= MostrarPausa;
        GameManager.Instancia.JuegoReanudado -= OcultarPausa;
        GameManager.Instancia.JuegoFinalizado -= MostrarGameOver;
    }

    private void CargarNivel() => SceneManager.LoadScene("Level-1");

    
    public void PausarJuego()
    {
        GameManager.Instancia.PausarJuego();
    }

    public void ReanudarJuego() => GameManager.Instancia.ReanudarJuego();
    public void ReiniciarJuego() => GameManager.Instancia.ReiniciarJuego();

    private void MostrarPausa()
    {
        if (panelPausa != null) panelPausa.SetActive(true);
        if (panelGameplay != null) panelGameplay.SetActive(false);
    }

    private void OcultarPausa()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameplay != null) panelGameplay.SetActive(true);
    }

    private void MostrarGameOver()
    {
        if (panelGameOver != null) panelGameOver.SetActive(true);
        if (panelGameplay != null) panelGameplay.SetActive(false);
        if (panelTriggerEspecial != null) panelTriggerEspecial.SetActive(false);
        if (panelTriggerEspecial2 != null) panelTriggerEspecial2.SetActive(false);
        if (panelTriggerEspecial3 != null) panelTriggerEspecial3.SetActive(false);
    }

    public void SalirDelJuego()
    {
        string escena = SceneManager.GetActiveScene().name;

        if (escena == "Menu")
        {
            Application.Quit();
        }
        else
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
    }

    private void Update()
    {
        if (GameManager.Instancia == null) return;

        if (bloqueoEscape) return;//nuevo

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var estado = GameManager.Instancia.estadoActual;

            if (estado == GameManager.EstadoJuego.Jugando)
                GameManager.Instancia.PausarJuego();
            else if (estado == GameManager.EstadoJuego.Pausado)
                GameManager.Instancia.ReanudarJuego();
        }
    }



    //Panel Nuevo Especial Historia.
    public void MostrarPanelTrigger()
    {
        bloqueoEscape = true;//Bloquea el escape

        if (panelPausa != null) panelPausa.SetActive(false);
        


        if (panelTriggerEspecial != null)
        {
            panelTriggerEspecial.SetActive(true);
        }
        
    }

    public void MostrarPanelTrigger2()
    {
        bloqueoEscape = true;

        if (panelPausa != null) panelPausa.SetActive(false);


        if (panelTriggerEspecial2 != null)
        {
            {
                panelTriggerEspecial2.SetActive(true);
            }
        }
    }



    public void MostrarPanelTrigger3()
    {
        bloqueoEscape = true;

        if (panelPausa != null) panelPausa.SetActive(false);


        if (panelTriggerEspecial3 != null)
        {
            {
                panelTriggerEspecial3.SetActive(true);
            }
        }
    }

    public void OcultarPanelTrigger()
    {
        if (panelTriggerEspecial != null)
            panelTriggerEspecial.SetActive(false);
        
        if (panelTriggerEspecial2 != null)
            panelTriggerEspecial2.SetActive(false);

        if (panelTriggerEspecial3 != null)
            panelTriggerEspecial3.SetActive(false);
    }

    public void MostrarFinalJuego()
    {
        bloqueoEscape = true;//Bloqueamos el escape otra vez

        if(panelPausa != null) panelPausa.SetActive(false);
        if (panelGameplay!= null) panelGameplay.SetActive(false);
        if (panelTriggerEspecial != null) panelTriggerEspecial.SetActive(false);

        if (panelFinalJuego != null)
        {
            panelFinalJuego.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
