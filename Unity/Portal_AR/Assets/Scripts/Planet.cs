using UnityEngine;
using UnityEngine.SceneManagement;

public class Planet : MonoBehaviour
{
  
    public void OnClick()
    {
        Debug.Log("Planet Clicked: " + gameObject.name);

        if (gameObject.name == "Earth")
        {
            Debug.Log("Loading Scene");
            SceneManager.LoadScene("Earth");
        }
    }

}
