using UnityEngine;

public class Key : MonoBehaviour, ICollectable, IInteractable
{
    public string Id { get => _id; }
    [SerializeField] private string _id;

    public int Ammount { get => _ammount; }
    [SerializeField] private int _ammount;

    public BaseInteractable Interaction { get => _interactable; set => _interactable = value; }
    [SerializeField] private BaseInteractable _interactable;
}
