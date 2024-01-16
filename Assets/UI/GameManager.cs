using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    
    public static void LoadGrassWorld() => SceneManager.LoadScene(1);
    public static void LoadDesertWorld() => SceneManager.LoadScene(2);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
            }

    }
}
