using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BaseInteractable
{
    //Evento global que se dispara al interactuar con este objeto.
    public GameEvent onInteract;

    //Evento local para acciones especificas de este objeto.
    public UnityEvent onLocalInteract;

    public virtual void Interact()
    {
        //Y se dispara el evento global.
        onInteract?.Raise();
        //Invoca el evento local (reproducir un sonido o animacion).
        onLocalInteract?.Invoke();
    }
}
