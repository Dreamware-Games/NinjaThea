using System.Collections;
using TMPro;
using UnityEngine;

namespace NinjaThea.GameElements
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tutorialText;

        private bool shown;

        private void Awake()
        {
            tutorialText.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (shown || !coll.gameObject.CompareTag("Player")) return;
            shown = true;
            tutorialText.gameObject.SetActive(true);
            StartCoroutine(CountDownAndDestroy());
        }

        IEnumerator CountDownAndDestroy()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

    }
}
