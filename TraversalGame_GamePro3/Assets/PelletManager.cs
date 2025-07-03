using UnityEngine;

public class PelletManager : MonoBehaviour
{
    [SerializeField] Exit_Manager exitLevelScript;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is a pellet
        if (other.CompareTag("Player"))
        {
            exitLevelScript.pelletsGathered++;
            Destroy(gameObject);
        }
    }
}
