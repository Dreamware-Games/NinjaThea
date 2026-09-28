using UnityEngine;
using UnityEngine.EventSystems;

namespace NinjaThea.UI
{
    public class ButtonSelector : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
