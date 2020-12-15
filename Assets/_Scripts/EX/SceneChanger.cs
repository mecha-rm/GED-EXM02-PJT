using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// changes the scene
public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // changes the scene using its name
    public void ChangeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    // changes the scene using its number.
    public void ChangeScene(int newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
