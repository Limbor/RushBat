using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    private RawImage cover;

    public float fadeTime;

    private void Start()
    {
        cover = GetComponent<RawImage>();
        StartScene();
    }

    void StartScene()
    {
        cover.color = Color.black;
        cover.DOFade(0, fadeTime).OnComplete(() =>
        {
            cover.enabled = false;
        });
    }

    public void EndScene()
    {
        cover.enabled = true;
        cover.DOFade(1, fadeTime).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
