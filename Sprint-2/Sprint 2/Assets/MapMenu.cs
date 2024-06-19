using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMenu : MonoBehaviour
{
    public void OpenLevel(string nameMap)
    {
        SceneManager.LoadScene(nameMap);
    }
}
