using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private static readonly int ItemCollectedHash = Animator.StringToHash("ItemCollected");

    [SerializeField] private AudioSource itemCollectedSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            Item item = collision.gameObject.transform.GetComponent<Item>();
            if (!item.Collected)
            {
                item.Collected = true;
                itemCollectedSound.Play();
                GameManager.Instance.ItemCollected();
                collision.gameObject.GetComponent<Animator>().SetTrigger(ItemCollectedHash);
                // Item is destroyed by animator transition triggred by ItemCollected (OnStateExit)
            }
        }
    }

}
