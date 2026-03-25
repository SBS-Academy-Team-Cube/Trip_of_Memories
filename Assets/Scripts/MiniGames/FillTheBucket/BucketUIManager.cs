using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketUIManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button HowOpenButton;
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button NextButton;
    [SerializeField] private Button HowExitButton;

    [Header("UI canvas")]
    [SerializeField] private GameObject Panel;
    [SerializeField] private List<Canvas> Canvases;


    private int CurPageIndex = 0;

    private void Awake()
    {
        HowOpenButton.onClick.AddListener(OpenHowToPlay);
        PrevButton.onClick.AddListener(PrevPage);
        NextButton.onClick.AddListener(NextPage);
        HowExitButton.onClick.AddListener(CloseHowToPlay);
        Panel.SetActive(false);
    }

    private void OpenHowToPlay()
    {
        CloseAllCanvas();
        CurPageIndex = 0;
        if(Canvases.Count > 0)
            Canvases[CurPageIndex].gameObject.SetActive(true);
        Panel.SetActive(true);
    }
    private void PrevPage()
    {
        if (CurPageIndex <= 0)
            return;
        Canvases[CurPageIndex--].gameObject.SetActive(false);
        Canvases[CurPageIndex].gameObject.SetActive(true);

    }
    private void NextPage()
    {
        if (CurPageIndex >= Canvases.Count - 1)
            return;
        Canvases[CurPageIndex++].gameObject.SetActive(false);
        Canvases[CurPageIndex].gameObject.SetActive(true);
    }
    private void CloseHowToPlay()
    {
        Panel.SetActive(false);
    }
    private void CloseAllCanvas()
    {
        foreach(var canvas in Canvases)
        {
            if(canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }
}
