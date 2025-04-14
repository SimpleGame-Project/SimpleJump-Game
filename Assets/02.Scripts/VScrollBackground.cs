using UnityEngine;

public class VScrollBackground : MonoBehaviour
{
    public float scrollSpeed;
    public int startIndex;
    public int endIndex;
    public Transform[] backgrounds;
    public float platformInterval;
    float cameraHeight;
    private float targetY;

    void Start()
    {
        cameraHeight = Camera.main.orthographicSize * 2;
        targetY = transform.position.y;
    }

    void Update()
    {
        float currentY = transform.position.y;
        float newY = Mathf.Lerp(currentY, targetY, scrollSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x , newY, transform.position.z);

        ScrollBackground();
    }

    public void ScrollBackground()
    {
        if (backgrounds[endIndex].position.y < -cameraHeight)
        {
            Vector3 backPos = backgrounds[startIndex].localPosition;
            backgrounds[endIndex].transform.localPosition = backPos + Vector3.up * cameraHeight;

            int pre_startIndex = startIndex;
            startIndex = endIndex;
            endIndex = pre_startIndex - 1 == -1 ? backgrounds.Length - 1 : pre_startIndex - 1;
        }
    }

    public void MoveToY(float diffY)
    {
        targetY -= diffY;

        if(diffY > (platformInterval - 1) * 2)
            GameManager.Instance.GameScore += 2;
        else
            GameManager.Instance.GameScore++; 
    }
}
