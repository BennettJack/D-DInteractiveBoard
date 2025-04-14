using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<Button> Buttons;

    public void ClickContinue()
    {
        Debug.Log("Click Continue");
    }
    
    public void ClickMapEditor()
    {
        Debug.Log("Click Map Editor");
        SceneManager.LoadScene(1);
    }
    
    public void ClickChooseMap()
    {
        Debug.Log("Click Choose Map");
    }
}
