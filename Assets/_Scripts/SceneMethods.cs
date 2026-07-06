using UnityEngine;

public class SceneMethods : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {
        SceneController.Instance.ChangeScene(sceneIndex);
    }

    public void QuitApplication()
    {
        SceneController.Instance.QuitApplication();
    }
}
