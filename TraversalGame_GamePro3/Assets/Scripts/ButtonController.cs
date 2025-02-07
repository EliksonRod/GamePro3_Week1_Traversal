using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject buttonEffect;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void BackToMenu(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonEffect.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonEffect.SetActive(false);
    }
}
