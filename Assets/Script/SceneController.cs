using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void TrocarCena(string Game)
    {
        SceneManager.LoadScene(Game);
    }
}