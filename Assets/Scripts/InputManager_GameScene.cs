using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

//using GestureRecognizer;

public class InputManager_GameScene : MonoBehaviour {


    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    public LineRenderer currentGestureLineRenderer;

    //GUI
    private string message;
    private bool recognized;
    private string newGestureName = "";



    private Vector2 prePos;
    private Vector2 currPos;

    private int currFigerId = 0;
    public Camera DrawCam;

    void Start()
    {
        DrawCam = GameObject.Find("Camera(DrawLine)").GetComponent<Camera>();
        platform = Application.platform;

        //Load pre-made gestures
        //TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        //foreach (TextAsset gestureXml in gesturesXml)
        //    trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        //Load user custom gestures
        string[] filePaths = Directory.GetFiles(Application.dataPath+ "/GestureData/", "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));

        currPos = new Vector2();
        prePos = new Vector2();

    }

    public void Input_LeapMotion()
    {


    }

    public void Input_Mobile()
    {
        if (Input.touchCount > 0)
        {
            virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);


            bool isTouch = false;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {

                vertexCount = 0;
                points.Clear();
                currentGestureLineRenderer.SetVertexCount(0);
                currPos = virtualKeyPosition;
                prePos = currPos;
                points.Add(new Point(currPos.x, -currPos.y, 0));
                currFigerId = Input.GetTouch(0).fingerId;
                isTouch = true;



            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved && currFigerId == Input.GetTouch(0).fingerId)
            {
                currPos = virtualKeyPosition;
                if (Vector2.Distance(currPos, prePos) >= 1.0f)
                {
                    points.Add(new Point(currPos.x, -currPos.y, 0));
                    isTouch = true;

                }
            }
            bool isEnded = false;
            if (Input.GetTouch(0).phase == TouchPhase.Ended && currFigerId == Input.GetTouch(0).fingerId)
            {
                if (points.Count > 0)
                {
                    currPos = virtualKeyPosition;
                    if (Vector2.Distance(currPos, prePos) >= 3.0f)
                    {
                        points.Add(new Point(currPos.x, -currPos.y, 0));
                    }
                    isTouch = true;
                    isEnded = true;
                }
            }



            if (isTouch == true)
            {

                if (points.Count > 0)
                {

                    if (points.Count > 1)
                    {
                        List<Vector3> point_list = new List<Vector3>();
                        for (float t = 0.1f; t <= 1; t += 0.1f)
                        {
                            Vector3 from = new Vector3(points[points.Count - 2].X, -points[points.Count - 2].Y);
                            Vector3 to = new Vector3(points[points.Count - 1].X, -points[points.Count - 1].Y);
                            Vector3 point = Vector3.Lerp(from, to, t);
                            vertexCount += 1;
                            currentGestureLineRenderer.SetVertexCount(vertexCount);
                            currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(point.x, point.y, 10)));
                            

                            point_list.Add(point);
                        }

                        for (int i = 1; i < point_list.Count - 1; ++i)
                        {
                            points.Insert(points.Count - 2, new Point(point_list[i].x, -point_list[i].y, 0));

                        }

                    }
                    else
                    {
                        vertexCount = 1;
                        currentGestureLineRenderer.SetVertexCount(vertexCount);
                        currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(points[points.Count - 1].X, -points[points.Count - 1].Y, 10)));
                    }


                }



                if (isEnded == true)
                {
                    Gesture candidate = new Gesture(points.ToArray());
                    Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                    gameObject.GetComponent<GameUpdater>().OnGestureResult(gestureResult.GestureClass);
                }
            }

        }
    }

    public void Input_Mouse()
    {
        virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);


        bool isTouch = false;
        if (Input.GetMouseButtonDown(0))
        {
            // strokeId = -1;
            vertexCount = 0;
            points.Clear();
            currentGestureLineRenderer.SetVertexCount(0);
            currPos = virtualKeyPosition;
            prePos = currPos;
            points.Add(new Point(currPos.x, -currPos.y, 0));
            isTouch = true;



        }


        if (Input.GetMouseButton(0))
        {
            currPos = virtualKeyPosition;
            if (Vector2.Distance(currPos, prePos) > 1.0f)
            {
                points.Add(new Point(currPos.x, -currPos.y, 0));
                isTouch = true;

            }


        }
        bool isEnded = false;
        if (Input.GetMouseButtonUp(0))
        {

            if (points.Count > 0)
            {
                currPos = virtualKeyPosition;
                if (Vector2.Distance(currPos, prePos) > 3.0f)
                {
                    points.Add(new Point(currPos.x, -currPos.y, 0));
                }
                isTouch = true;
                isEnded = true;
            }


        }

        if (isTouch == true)
        {

            if (points.Count > 0)
            {

                if (points.Count > 1)
                {
                    List<Vector3> point_list = new List<Vector3>();
                    for (float t = 0; t <= 1; t += 0.1f)
                    {
                        Vector3 from = new Vector3(points[points.Count - 2].X, -points[points.Count - 2].Y);
                        Vector3 to = new Vector3(points[points.Count - 1].X, -points[points.Count - 1].Y);
                        Vector3 point = Vector3.Lerp(from, to, t);
                        vertexCount += 1;

                        currentGestureLineRenderer.SetVertexCount(vertexCount);
                        currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(point.x, point.y, 10)));
                        point_list.Add(point);
                    }

                    for (int i = 1; i < point_list.Count - 1; ++i)
                    {
                        points.Insert(points.Count - 2, new Point(point_list[i].x, -point_list[i].y, 0));

                    }

                }
                else
                {
                    vertexCount = 1;
                    currentGestureLineRenderer.SetVertexCount(vertexCount);
                    currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(points[points.Count - 1].X, -points[points.Count - 1].Y, 10)));
                }




            }



            if (isEnded == true)
            {

                if (points.Count > 1)
                {
                    Gesture candidate = new Gesture(points.ToArray());
                    Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                    gameObject.GetComponent<GameUpdater>().OnGestureResult(gestureResult.GestureClass);
                }
            }
        }




        prePos = currPos;



    }


    void Update()
    {


        
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
            Input_Mobile();
        else
        {
            Input_Mouse();
        }







    }



  
}
