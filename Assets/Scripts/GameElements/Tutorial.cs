using System.Collections;
using TMPro;
using UnityEngine;

namespace NinjaThea.GameElements
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tutorialText;

        private void Awake()
        {
            tutorialText.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                tutorialText.gameObject.SetActive(true);
            }
            StartCoroutine(CountDownAndDestroy());
        }

        IEnumerator CountDownAndDestroy()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

    }
}
