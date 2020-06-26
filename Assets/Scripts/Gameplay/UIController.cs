using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text counterText;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateProgress(int free, int total)
    {
        counterText.text = String.Format("{0}/{1}", free, total);
    }
}
