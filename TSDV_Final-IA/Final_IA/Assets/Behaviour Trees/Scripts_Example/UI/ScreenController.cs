using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BehaviourTree
{
    public class ScreenController : MonoBehaviour
    {
        public void LoadScene(string nameScene)
        {
            SceneManager.LoadScene(nameScene);
        }
        public void Salir()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
