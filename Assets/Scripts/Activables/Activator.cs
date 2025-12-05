using UnityEngine;

public class Activator : MonoBehaviour, IInteractable, IRequierement
{
    //Evento global que este objeto escuchara.
    public GameEvent gameEvent;

    //Objeto que se activara / desactivara cuando el evento se dispara.
    public GameObject targetObject;

    //Si ,activate, es true, el objeto se activara, si es false, se desactivara.
    public bool activate = true;

    public string Id { get => _id; }
    [SerializeField] private string _id;

    public int Ammount { get => _ammount; }
    [SerializeField] private int _ammount;

    public BaseInteractable Interaction { get => _interactable; set => _interactable = value; }
    [SerializeField] private BaseInteractable _interactable;


    //OneEnable, se llama cuando el objeto se activa en la escena.
    private void OnEnable()
    {
        //Registra el metodo , OnEventRaised, como listener del evento.
        gameEvent?.RegisterListener(OnEventRaised);
    }

    //OnDisable, se llama cuando el objeto se desactiva en la escena.
    private void OnDisable()
    {
        //Remueve el metodo , OnEventRaised, de los listeners del evento.
        gameEvent?.RemoveListener(OnEventRaised);
    }

    //Metodo que se ejecutara cuando el evento se dispare.
    private void OnEventRaised()
    {
        //targetObject.SetActive(activate);
    }
}
