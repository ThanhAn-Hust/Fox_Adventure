using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Win : MonoBehaviour
{
    public GameObject winUI;
    public float delayBeforeRestart = 3f;
    public Text countdownText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            collision.GetComponent<Fox>().disabledByWin = true;
            winUI.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
