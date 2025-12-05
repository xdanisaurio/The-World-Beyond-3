using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    public delegate void EstadoJuegoDelegado();
    public event EstadoJuegoDelegado JuegoIniciado;
    public event EstadoJuegoDelegado JuegoPausado;
    public event EstadoJuegoDelegado JuegoReanudado;
    public event EstadoJuegoDelegado JuegoFinalizado;

    public enum EstadoJuego { Menu, Jugando, Pausado, Finalizado }
    public EstadoJuego estadoActual = EstadoJuego.Menu;

    private void Awake()
    {
       if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1;

        string escena = SceneManager.GetActiveScene().name;
        if (escena != "Menu")
        {
            estadoActual = EstadoJuego.Jugando;
            JuegoIniciado?.Invoke();
        }
    }

    public void IniciarJuego()
    {
        if (estadoActual == EstadoJuego.Jugando) return;
        estadoActual = EstadoJuego.Jugando;
        Time.timeScale = 1;
        JuegoIniciado?.Invoke();
    }

    public void PausarJuego()
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        estadoActual = EstadoJuego.Pausado;
        Time.timeScale = 0;
        JuegoPausado?.Invoke();
    }

    public void ReanudarJuego()
    {
        if (estadoActual != EstadoJuego.Pausado) return;
        estadoActual = EstadoJuego.Jugando;
        Time.timeScale = 1;
        JuegoReanudado?.Invoke();
    }

    public void GameOver()
    {
        if (estadoActual == EstadoJuego.Finalizado) return;
        estadoActual = EstadoJuego.Finalizado;
        Time.timeScale = 0;
        JuegoFinalizado?.Invoke();
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
