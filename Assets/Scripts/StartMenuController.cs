using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
public void OnStartClick()
{
    Debug.Log("Start button clicked!"); // Esto debe aparecer en la consola
    SceneManager.LoadScene("SampleScene");
}

public void OnExitClick()
{
    Debug.Log("Exit button clicked!"); // Para verificar que se llama correctamente.

    #if UNITY_EDITOR
        // Este código solo funciona en el editor de Unity.
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        // Este cierra el juego al ejecutarlo como compilado.
        Application.Quit();
    #endif
}
}