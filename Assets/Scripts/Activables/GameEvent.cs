using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Events/Game Event")]
public class GameEvent : ScriptableObject
{
    //una lista de funciones que se ejecutan cuando el evento se dispara.
    private UnityAction listeners;


    //Raise invoca a todos los listeners registrados.
    public void Raise()
    {
        listeners?.Invoke();
    }


    //Añade una funcion a la lista de listeners.
    public void RegisterListener(UnityAction listener)
    {
        listeners += listener;
    }


    //Remueve una funcion de la lista de listeners.
    public void RemoveListener(UnityAction listener)
    {
        listeners -= listener;
    }
}
