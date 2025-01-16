using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        Vector3 camPosition = player.position;
        camPosition.y = transform.position.y;
        transform.position = camPosition;
    }
}
