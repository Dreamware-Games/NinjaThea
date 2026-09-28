using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DefaultButtonHighlighter : MonoBehaviour
{
    [SerializeField] private Button defaultHighlightedButton;

    private readonly WaitForSecondsRealtime waitReselect = new WaitForSecondsRealtime(3f);

    private Coroutine checkCoroutine;

    private void OnEnable()
    {
        HighlightDefaultButton();
        if (checkCoroutine != null) StopCoroutine(checkCoroutine);
        checkCoroutine = StartCoroutine(ReselectIfNoneSelected());
    }

    private void OnDisable()
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);
    }

    public void HighlightDefaultButton()
    {
        if (defaultHighlightedButton == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        if (!defaultHighlightedButton.gameObject.activeInHierarchy || !defaultHighlightedButton.interactable) return;
        EventSystem.current.SetSelectedGameObject(defaultHighlightedButton.gameObject);
    }

    private IEnumerator ReselectIfNoneSelected()
    {
        while (true)
        {
            yield return waitReselect;

            if (EventSystem.current.currentSelectedGameObject == null)
            {
                HighlightDefaultButton();
            }
        }
    }
}