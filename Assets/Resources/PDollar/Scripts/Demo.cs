﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

public class Demo : MonoBehaviour {

	//public Transform gestureOnScreenPrefab;

	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	public LineRenderer currentGestureLineRenderer;

	//GUI
	private string message;
	private bool recognized;
    private bool addAs;
	private string newGestureName = "";

    private Vector2 prePos;
    private Vector2 currPos;



	void Start () {

		platform = Application.platform;
		drawArea = new Rect(0, 0, Screen.width - Screen.width /10 -100, Screen.height);

        ////Load pre-made gestures
        //TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        //foreach (TextAsset gestureXml in gesturesXml)
        //    trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        ////Load user custom gestures
        //string[] filePaths = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "/Document", "*.xml");
        //foreach (string filePath in filePaths)
        //    trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
        string[] filePaths = Directory.GetFiles(Application.dataPath + "/GestureData/", "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));


        currPos = new Vector2();
        prePos = new Vector2();
	}

	void Update () {

		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		} else {
			if (Input.GetMouseButton(0)) {
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}
		}
        if (Input.GetMouseButtonDown(1) == true)
        {
            strokeId = 0;
            vertexCount = 0;
            points.Clear();
            currentGestureLineRenderer.SetVertexCount(vertexCount);
        }
        if (drawArea.Contains(virtualKeyPosition)) {
		    if (Input.GetMouseButton(0)) {
               
                currPos = virtualKeyPosition;
                if (Vector2.Distance(currPos, prePos) > 1.0f)
                {

                    points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
                    currentGestureLineRenderer.SetVertexCount(++vertexCount);
                    currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
                }

                
		    }
            prePos = currPos;
        }
	
	}

	void OnGUI() {

		GUI.Box(drawArea, "Draw Area");

		GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);

		if (GUI.Button(new Rect(Screen.width - 100, 10, 100, 30), "Recognize")) {

			recognized = true;

			Gesture candidate = new Gesture(points.ToArray());
			Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
			
			message = gestureResult.GestureClass + " " + gestureResult.Score;
		}

		GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as: ");
		newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);

		if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add") && points.Count > 0 && newGestureName != "") {

            addAs = true;

            string fileName = String.Format("{0}/{1}-{2}.xml", System.IO.Directory.GetCurrentDirectory() + "/Document", newGestureName, DateTime.Now.ToFileTime());

			#if !UNITY_WEBPLAYER
				GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
			#endif

			trainingSet.Add(new Gesture(points.ToArray(), newGestureName));

			newGestureName = "";
		}
	}
}
