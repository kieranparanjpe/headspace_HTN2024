using System.Collections;
using System.Xml.Linq;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public float moveDistance = 1000f;    // Adjusted distance to move
    public float moveDuration = 1f;       // Time it takes to move

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;
    private Coroutine leftCoroutine;
    private Coroutine rightCoroutine;
    public GameObject text;

    void Start()
    {
        // Save the initial anchored positions of both objects
        leftStartPos = left.GetComponent<RectTransform>().anchoredPosition;
        rightStartPos = right.GetComponent<RectTransform>().anchoredPosition;
    }

    public void StartLoading()
    {
        if (leftCoroutine != null) StopCoroutine(leftCoroutine);
        if (rightCoroutine != null) StopCoroutine(rightCoroutine);
        text.SetActive(true);

        leftCoroutine = StartCoroutine(MoveObject(left, leftStartPos,new Vector2(0, 0)));
        rightCoroutine = StartCoroutine(MoveObject(right, rightStartPos, new Vector2(0, 0)));
    }

    public void StopLoading()
    {
        text.SetActive(false);
        // Move the left object right and the right object left back to their original positions
        if (leftCoroutine != null) StopCoroutine(leftCoroutine);
        if (rightCoroutine != null) StopCoroutine(rightCoroutine);


        leftCoroutine = StartCoroutine(MoveObject(left, left.GetComponent<RectTransform>().anchoredPosition, leftStartPos));
        rightCoroutine = StartCoroutine(MoveObject(right, right.GetComponent<RectTransform>().anchoredPosition, rightStartPos));
    }

    private IEnumerator MoveObject(GameObject obj, Vector2 start, Vector2 end)
    {
        float elapsedTime = 0f;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        while (elapsedTime < moveDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;  
        }

        rectTransform.anchoredPosition = end; 

    }
}
