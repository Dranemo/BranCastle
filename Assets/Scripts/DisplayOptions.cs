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
        List<string> options = new List<string> { "Plein écran", "Fenêtré", "Plein écran fenêtré" };
        displayModeDropdown.AddOptions(options);

        // Ajouter un listener pour détecter les changements de sélection
        displayModeDropdown.onValueChanged.AddListener(delegate { SetDisplayMode(displayModeDropdown.value); });

        // Initialiser le Dropdown avec le mode d'affichage actuel
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            displayModeDropdown.value = 0; // Plein écran
        }
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            displayModeDropdown.value = 1; // Fenêtré
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            displayModeDropdown.value = 2; // Plein écran fenêtré
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
