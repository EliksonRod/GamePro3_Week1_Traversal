using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit_Manager : MonoBehaviour
{
    [SerializeField] int numberOfPelletsInLevel = 3; // Set the number of pellets required to unlock the exit
    public int pelletsGathered = 0; // Counter for pellets gathered
    bool exitUnlocked = false; // Flag to check if the exit is unlocked

    int buildIndex; // Variable to store the current scene's build index
    [SerializeField] SpriteRenderer Door; // Reference to the exit barrier renderer
    [SerializeField] Sprite Closed_Door; // Sprite for the closed door
    [SerializeField] Sprite Opened_Door; // Sprite for the open door

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Door.sprite = Closed_Door;
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(numberOfPelletsInLevel);
        UnlockExit();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player" && exitUnlocked)
        {
            //Loads next scene in buildIndex
            SceneManager.LoadScene(buildIndex + 1);
        }
    }

    void UnlockExit()
    {
        if (pelletsGathered >= numberOfPelletsInLevel)
        {
            Door.sprite = Opened_Door;
            exitUnlocked = true;
        }
    }
}


