using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DisplayOptions : MonoBehaviour
{
    public TMP_Dropdown displayModeDropdown;

    void Start()
    {
        // Initialiser les options du Dropdown
        displayModeDropdown.ClearOptions();
        List<string> options = new List<string> { "Plein �cran", "Fen�tr�", "Plein �cran fen�tr�" };
        displayModeDropdown.AddOptions(options);

        // Ajouter un listener pour d�tecter les changements de s�lection
        displayModeDropdown.onValueChanged.AddListener(delegate { SetDisplayMode(displayModeDropdown.value); });

        // Initialiser le Dropdown avec le mode d'affichage actuel
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            displayModeDropdown.value = 0; // Plein �cran
        }
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            displayModeDropdown.value = 1; // Fen�tr�
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            displayModeDropdown.value = 2; // Plein �cran fen�tr�
        }

        displayModeDropdown.RefreshShownValue();
    }

    public void SetDisplayMode(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }
}
