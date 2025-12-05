using UnityEngine;

public class Parallax : MonoBehaviour
{

    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float length, startPos;
    private float defaultYPos = -2f;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        Vector3 camPosition = cam.transform.position;
        float temp = camPosition.x * (1 - parallaxEffect);
        float dist = camPosition.x * parallaxEffect;

        float yPosition = defaultYPos;
        if (camPosition.y <= defaultYPos)
            yPosition += camPosition.y - defaultYPos;

        transform.position = new Vector3(startPos + dist, yPosition, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }

}
