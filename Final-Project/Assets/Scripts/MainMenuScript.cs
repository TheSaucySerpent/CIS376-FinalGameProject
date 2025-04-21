using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private UIDocument _document;
    private Button _button;
    private Button _quit_button;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _button = _document.rootVisualElement.Q("StartButton") as Button;
        _button.RegisterCallback<ClickEvent>(PlayGame);

        _quit_button = _document.rootVisualElement.Q("QuitButton") as Button;
        _quit_button.RegisterCallback<ClickEvent>(QuitGame);
    }

    private void OnDisable()
    {

        _button.UnregisterCallback<ClickEvent>(PlayGame);
    }
    private void PlayGame(ClickEvent evt)
    {
        _button.text = "Loading Game";
        SceneManager.LoadScene("SampleScene");
    }

    private void QuitGame(ClickEvent evt){
        Application.Quit();
    }
}
