using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombinedLogoController : MonoBehaviour
{
    public int team0Number;
    public int team1Number;
    public Image logo0Image;
    public Image logo1Image;

    public void SetTeam0Number(int newTeamNumber) {
        team0Number = newTeamNumber;
        UpdateLogos();
    }
    public void SetTeam1Number(int newTeamNumber) {
        team1Number = newTeamNumber;
        UpdateLogos();
    }

    void Start() {
        // Set team number to 0 before we start
        team0Number = 0;
        team1Number = 0;
        UpdateLogos();
    }

    void UpdateLogos()
    {
        // Assuming logos are in a folder named "Logos" under "Assets/Resources"
        string logoPath0 = "Logos/" + team0Number.ToString();
        string logoPath1 = "Logos/" + team1Number.ToString();
        // Debug.Log("Loading Team Logo: " + logoPath);

        // Load the logo dynamically
        Sprite logo0Sprite =  Resources.Load<Sprite>(logoPath0);
        Sprite logo1Sprite =  Resources.Load<Sprite>(logoPath1);

        // Check if the sprite was loaded successfully
        if (logo0Sprite != null)
        {
            // Set the sprite for the Image component
            logo0Image.sprite = logo0Sprite;
        }
        else
        {
            Debug.LogError("Logo not found for team number: " + team0Number);
        }

        if (logo1Sprite != null)
        {
            // Set the sprite for the Image component
            logo1Image.sprite = logo1Sprite;
        }
        else
        {
            Debug.LogError("Logo not found for team number: " + team1Number);
        }
    }
}
