using TMPro;
using UnityEngine;

namespace NinjaThea.UI
{
    // Stage/Game complete container: exposes the text that shows the completion time
    public class CompletionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        public TextMeshProUGUI TimeText => timeText;
    }
}
