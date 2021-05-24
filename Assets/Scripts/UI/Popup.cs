using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);
        this.text.text = text;
    }
}
