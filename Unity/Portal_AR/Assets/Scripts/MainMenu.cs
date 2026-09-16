using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionPanel; // Reference to the Instruction Panel
    public Button Start;
    public Button Exit;
    public Button Instructions;

    public void StartGame()
    {
        SceneManager.LoadScene("Asteroid");
    }

    public void Quit()
    {
        Debug.Log("Application is quitting");
        Application.Quit();
    }

    public void ShowInstructions()
    {
        instructionPanel.SetActive(true);  // Show the instructions
        Start.gameObject.SetActive(false);
        Exit.gameObject.SetActive(false);
        Instructions.gameObject.SetActive(false);

    }

    public void HideInstructions()
    {
        instructionPanel.SetActive(false); // Hide the instructions
        Start.gameObject.SetActive(true);
        Exit.gameObject.SetActive(true);    
        Instructions.gameObject.SetActive(true);
    }
}
