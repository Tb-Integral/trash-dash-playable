using UnityEngine;
using UnityEngine.UI;

public class PlayableEndCard : MonoBehaviour
{
    public static PlayableEndCard Instance { get; private set; }

    [SerializeField] GameObject root;
    [SerializeField] Text title;
    [SerializeField] string winTitle = "CONTINUE?";
    [SerializeField] string failTitle = "TRY AGAIN!";

    void Awake()
    {
        Instance = this;
        if (root != null)
            root.SetActive(false);
    }

    public void Show(bool won)
    {
        if (title != null)
            title.text = won ? winTitle : failTitle;
        if (root != null)
            root.SetActive(true);
    }
}