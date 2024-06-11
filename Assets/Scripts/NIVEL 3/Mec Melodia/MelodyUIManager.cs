using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyUIManager : MonoBehaviour
{
    public List<GameObject> melodyPanels; // Asigna los paneles de las melodías en el inspector

    // Método para mostrar la melodía basada en su índice
    public void ShowMelody(int melodyIndex)
    {
        if (melodyIndex < melodyPanels.Count)
        {
            HideAllMelodies(); // Ocultar todas las melodías antes de mostrar una nueva
            melodyPanels[melodyIndex].SetActive(true);
        }
    }

    // Método para ocultar todas las melodías
    public void HideAllMelodies()
    {
        foreach (var panel in melodyPanels)
        {
            panel.SetActive(false);
        }
    }
}
