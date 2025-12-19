using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleSelectionManager : MonoBehaviour
{
    public void BackToRoleSelection()
    {
        SceneManager.LoadScene("AdminLoginScene");
    }
}