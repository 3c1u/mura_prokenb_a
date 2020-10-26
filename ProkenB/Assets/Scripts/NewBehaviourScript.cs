using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProkenB.Game;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject lineGroup; // for grouping
	public Camera cam;

    public GameObject panel;
    public Canvas myCanvas; // to obtain canvas.scale
    public int count;

    private float accTime = 0.0f;

    void DrawLine(List<Vector2> my2DVec, int startPos)
    {
        List<Vector3> myPoint = new List<Vector3>();
        for (int idx = 0; idx < 2; idx++)
        {
            myPoint.Add(new Vector3(my2DVec[startPos + idx].x, my2DVec[startPos + idx].y, 0.0f));
        }

        GameObject newLine = new GameObject("Line" + startPos.ToString());
        LineRenderer lRend = newLine.AddComponent<LineRenderer>();
        lRend.positionCount = 2;
        lRend.startWidth = 0.05f;
        lRend.endWidth = 0.05f;
        Vector3 startVec = myPoint[0];
        Vector3 endVec = myPoint[1];
        lRend.SetPosition(0, startVec);
        lRend.SetPosition(1, endVec);

        newLine.transform.parent = lineGroup.transform; // for grouping
    }

    void drawGraph(List<Vector2> my2DVec, GameObject panel)
    {
        lineGroup = new GameObject("LineGroup");

        for (int idx = 0; idx < my2DVec.Count - 1; idx++)
        {
            DrawLine(my2DVec, /* startPos=*/idx);
        }

        lineGroup.transform.parent = panel.transform; // to belong to panel
		lineGroup.transform.position=cam.WorldToScreenPoint(panel.transform.position);
    }

    void clearGraph(GameObject panel)
    {
        foreach (Transform line in panel.transform)
        {
            Destroy(line.gameObject);
        }
    }

    void addPointNormalized(List<Vector2> my2DVec, GameObject panel, Vector2 point)
    {
        // point: normalized point data [-1.0, 1.0] for each of x, y

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        float width = panelRect.rect.width;
        float height = panelRect.rect.height;

        RectTransform canvasRect = myCanvas.GetComponent<RectTransform>();

        Vector2 pointPos;

        // Bottom Left
        pointPos = panel.transform.position;
        pointPos.x += point.x * width * 0.5f * canvasRect.localScale.x;
        pointPos.y += point.y * height * 0.5f * canvasRect.localScale.y;
        my2DVec.Add(pointPos);
    }

    void Test_drawBox(List<Vector2> my2DVec, GameObject panel)
    {
        addPointNormalized(my2DVec, panel, new Vector2(-1.0f, -1.0f));
        addPointNormalized(my2DVec, panel, new Vector2(-1.0f, 1.0f));
        addPointNormalized(my2DVec, panel, new Vector2(1.0f, 1.0f));
        addPointNormalized(my2DVec, panel, new Vector2(1.0f, -1.0f));
        addPointNormalized(my2DVec, panel, new Vector2(-1.0f, -1.0f));

        drawGraph(my2DVec, panel);
    }

    void Start()
    {

    }

    void Update()
    {
        accTime += Time.deltaTime;
        count++;
        if (accTime < 0.3f)
        {
            return;
        }
        accTime = 0.0f;
        List<Vector2> my2DVec = new List<Vector2>();
		clearGraph (panel);
        Test_drawBox(my2DVec, panel);
        var fft = GameManager.Instance.Detector.fftResultBuffer;
        addPointNormalized(my2DVec, panel, new Vector2(count, fft[count]));
        drawGraph(my2DVec, panel);
    }
}
