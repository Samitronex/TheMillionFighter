using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    // Función para volver al menú principal
    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo no está pausado
        SceneManager.LoadScene("StartScene"); // Cambia "StartScene" al nombre exacto de tu escena de menú
    }
}