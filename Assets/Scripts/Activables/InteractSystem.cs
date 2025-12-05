using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] float radius;
    [SerializeField] CollectSystem collect;
    [SerializeField] Inventory inventory;
    public void Interact()
    {
        Collider[] collisions = Physics.OverlapSphere(pivot.position, radius);

        foreach (var item in collisions)
        {
            if (item.TryGetComponent(out ICollectable collectable))
            {
                collect.CollectItem(collectable);
            }

            if (item.TryGetComponent(out IInteractable interactable))
            {
                if (item.TryGetComponent(out IRequierement requierement))
                {
                    if (!inventory.Spend(requierement.Id, requierement.Ammount))
                    {
                        continue;
                    }
                }

                interactable.Interaction.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pivot.position, radius);
    }
}
