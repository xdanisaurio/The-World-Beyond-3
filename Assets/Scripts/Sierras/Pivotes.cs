using System.Collections.Generic;
using UnityEngine;

public class Pivotes : MonoBehaviour
{
    [SerializeField] List<Transform> pivots = new List<Transform>();
    private int indexPivot = 1;

    [SerializeField] private float timeBetweenPivots = 1f;
    [SerializeField] private float moveSpeed;
    private float elapsedTime = 0f;

    private void Update()
    {
        if (pivots.Count == 0) return;
        transform.position = Vector3.MoveTowards(transform.position, pivots[indexPivot].position, moveSpeed * Time.deltaTime);

        elapsedTime += Time.deltaTime;

        if (Vector3.Distance(transform.position, pivots[indexPivot].position) < 0.01f &&
            elapsedTime >= timeBetweenPivots)
        {
            indexPivot = (indexPivot + 1) % pivots.Count;
            elapsedTime = 0f;
        }
    }
}
