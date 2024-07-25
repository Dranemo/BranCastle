using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    public GameObject[] items; // Les items que vous pouvez s�lectionner
    private int selectedIndex = 0; // L'index de l'item actuellement s�lectionn�

    void Update()
    {
        // Utilisez la molette de la souris pour changer l'item s�lectionn�
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0) selectedIndex--; // Molette vers le haut
            else selectedIndex++; // Molette vers le bas

            // Assurez-vous que l'index reste dans les limites du tableau
            if (selectedIndex < 0) selectedIndex = items.Length - 1;
            if (selectedIndex >= items.Length) selectedIndex = 0;
        }

        // Utilisez une touche pour placer l'item s�lectionn�
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Instantiate(items[selectedIndex], transform.position, Quaternion.identity);
        }
    }
}

