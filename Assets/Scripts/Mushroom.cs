using UnityEngine;
using UnityEngine.UI;

public class Mushroom : MonoBehaviour
{
    public Text mushroomText;  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (mushroomText != null)
            {
                mushroomText.text = "Mushroom: Yes";
            }

            AudioManager.instance.PlaySFX("mushroom");
            Destroy(gameObject);
        }
    }
}

