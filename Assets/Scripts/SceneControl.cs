using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            int currNdx = SceneManager.GetActiveScene().buildIndex;

            if (++currNdx >= SceneManager.sceneCountInBuildSettings)
                currNdx = 0;

            SceneManager.LoadScene(currNdx);

            // will die immediately, stop doing anything else 
            return;
        }
    }
}

