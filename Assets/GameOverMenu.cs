using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public TMP_Text ScoreField;

    // Start is called before the first frame update
    private void Start()
    {
        ScoreField.text = $"{GameLogic.Score}";
    }

    // Return to the main menu
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
