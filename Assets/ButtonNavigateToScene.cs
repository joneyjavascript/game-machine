using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigateToScene : MonoBehaviour
{
    public SceneName sceneName = SceneName.Menu;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            GameManager.instance.PlaySoundEffect("ButtonClick");
            GameManager.instance.GoToScene(sceneName);
        });
    }
}
