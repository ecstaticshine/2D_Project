using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

}
