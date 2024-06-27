using UnityEngine;
using UnityEngine.SceneManagement;

public class ChgtScène : MonoBehaviour
{
    public string scene; 

    // Méthode à appeler lorsque le bouton est cliqué
    public void OnButtonClick()
    {
        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogError("Le nom de la scène n'est pas défini");
        }
    }
}