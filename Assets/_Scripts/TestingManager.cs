using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TestingManager : MonoBehaviour
{
    public List<Vector2> drawnPivot;
    public List<Vector2> spawnedPivot;
    private bool dataCollectingStarted;

    public void Update()
    {
        ClickPivot();
        ClickSpawnedPivot();
        Export();
    }

    public void ClickPivot()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0) || dataCollectingStarted) return;
        dataCollectingStarted = true;
        drawnPivot.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void ClickSpawnedPivot()
    {
        Vector2 mousePos = Vector2.zero;

        if (!Input.GetKeyDown(KeyCode.Mouse0) || !dataCollectingStarted) return;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D collider = Physics2D.OverlapCircle(mousePos, 1f);

        if (collider == null) return;

        if (collider.CompareTag("Ground"))
        {
            spawnedPivot.Add(new Vector2(collider.transform.position.x, collider.transform.position.y));
            dataCollectingStarted = false;
        }
    }

    public void Export()
    {
        if (!Input.GetKeyDown(KeyCode.UpArrow)) return;
        Debug.Log("Write Started");
        StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("Drawn Pivot X;Drawn Pivot Y;Spawned Pivot X;Spawned Pivot Y");

        for (int i = 0; i < drawnPivot.Count; i++)
        {
            sb.AppendLine(Mathf.RoundToInt(drawnPivot[i].x).ToString() + ';' + Mathf.RoundToInt(drawnPivot[i].y).ToString() + ';' + Mathf.RoundToInt(spawnedPivot[i].x).ToString() + ';' + Mathf.RoundToInt(spawnedPivot[i].y).ToString());
        }

        string filePath = Application.dataPath + "/export.csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        Debug.Log("Write Done");
    }
}
