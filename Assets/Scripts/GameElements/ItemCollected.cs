using UnityEngine;

namespace NinjaThea.GameElements
{
    public class ItemCollected : StateMachineBehaviour
    {

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }

    }
}
