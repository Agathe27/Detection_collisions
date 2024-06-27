using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UXF;

public class KeyPressHandler : MonoBehaviour
{
    public TMP_Text instructionText;
    private Session sessionActuelle;
    private string reponse;

    void Start()
    {
        // Initialiser le texte d'instruction
        if (instructionText != null)
        {
            instructionText.text = "Appuyez sur A ou B";
        }
        // Obtenir la session actuelle
        sessionActuelle = Session.instance;
        Debug.Log("KeyPressHandler démarré. Session actuelle : " + (sessionActuelle != null ? "Oui" : "Non"));
    }

    void OnEnable()
    {
        Debug.Log("KeyPressHandler activé");
    }

    void OnDisable()
    {
        Debug.Log("KeyPressHandler désactivé.");
        EnregistrerData();
    }

    void Update()
    {
        string reponse;
        // Vérif si la touche A ou a est pressée (QWERTY -> AZERTY)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdateText("A");
            reponse = "Vrai";
            sessionActuelle.CurrentTrial.result["réponse_utilisateur"] = reponse;
            Debug.Log("Réponse utilisateur enregistrée: " + reponse);
        }
        // Vérif si la touche B ou b est appuyée
        else if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateText("B");
            reponse = "Faux";
            sessionActuelle.CurrentTrial.result["réponse_utilisateur"] = reponse;
            Debug.Log("Réponse utilisateur enregistrée: " + reponse);
        }
    }

    // mise à jour du texte affiché
    void UpdateText(string newText)
    {
        if (instructionText != null)
        {
            instructionText.text = newText;
            //Debug.Log("Texte mis à jour à: " + newText);
            reponse = newText;
        }

        /*if (sessionActuelle != null && sessionActuelle.InTrial)
        {
            try
            {
                sessionActuelle.CurrentTrial.result["réponse_utilisateur"] = newText;
                Debug.Log("Réponse utilisateur enregistrée: " + newText);
            }
            catch (System.InvalidOperationException e)
            {
                Debug.LogWarning("Impossible d'enregistrer la réponse : " + e.Message);
            }
        }*/
        else
        {
            Debug.LogWarning("Aucun essai en cours, la réponse n'a pas été enregistrée.");
        }
    }


    public void EnregistrerData()
    {
        if (sessionActuelle != null && sessionActuelle.InTrial && !string.IsNullOrEmpty(reponse))
        {
            sessionActuelle.CurrentTrial.result["réponse_utilisateur"] = reponse;
            Debug.Log("Réponse utilisateur: " + reponse);
        }
        else
        {
            Debug.LogError("Session ou Trial est null, ou réponse est vide");
        }
    }
}