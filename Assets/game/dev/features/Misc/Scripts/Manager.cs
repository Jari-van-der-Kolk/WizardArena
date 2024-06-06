using Saxon.HashGrid;
using Saxon.NodePositioning;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private static Manager instance;

    public static Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Manager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("Manager");
                    instance = obj.AddComponent<Manager>();
                }
            }
            return instance;
        }
    }

    // Your manager's variables and methods go here
    public SpatialHashGrid<HashNode> nodeHashGrid;

private void Awake()
    {
        // Ensure only one instance of the manager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy any additional instances
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        
    }

    // Your manager's functionality goes here

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        // Code to execute when a new scene is loaded
        Debug.Log("Scene loaded: " + scene.name);

        // Check if it's the scene where you want to perform specific actions
        if (scene.name == "YourTargetScene")
        {
            // Perform actions specific to the target scene
        }
    }

    
}
