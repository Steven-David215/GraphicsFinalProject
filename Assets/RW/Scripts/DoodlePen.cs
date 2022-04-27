/*
Copyright (c) 2020 Razeware LLC

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom
the Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

Notwithstanding the foregoing, you may not use, copy, modify,
merge, publish, distribute, sublicense, create a derivative work,
and/or sell copies of the Software in any work that is designed,
intended, or marketed for pedagogical or instructional purposes
related to programming, coding, application development, or
information technology. Permission for such use, copying,
modification, merger, publication, distribution, sublicensing,
creation of derivative works, or sale is expressly withheld.

This project and source code may use libraries or frameworks
that are released under various Open-Source licenses. Use of
those libraries and frameworks are governed by their own
individual licenses.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoodlePen : MonoBehaviour
{
    public const float UIHeight = 60;        // the screen height of the UI 

    new public Camera camera;
    

    public DoodleLine linePrefab = null;
    public Gradient[] colorTheme = null;

    public float drawingBound = 0;      // The lower bound where touch is valid

    private int mySelectedColorIndex = 0;
    private float myLineWidth = 0.005f;
    private int mySortingOrder = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            // skip the touches fall on the UI
            if (Input.mousePosition.y < drawingBound)
            {
                return;
            }

            SpawnNewLine();
        }
    }

    Gradient getLineGradient()
    {
        return colorTheme[mySelectedColorIndex];
    }

    public void ChangeColorIndex(int index)
    {
        mySelectedColorIndex = index;
    }

    public void ChangeLineWidth(float lineWidth)
    {
        myLineWidth = lineWidth;
    }

    public void SpawnNewLine()
    {
        if(linePrefab == null)
        {
            return;
        }

        var newLine = Instantiate(linePrefab);
        SetupRaycastLogic(newLine);

        newLine.lineGradient = getLineGradient();
        newLine.SetLineOrder(mySortingOrder);
        newLine.ChangeLineWidth(myLineWidth);

        Transform t = newLine.transform;
        t.parent = transform;

        mySortingOrder++;
    }

    public void ClearLines()
    {
        DoodleLine[] lines = GetComponentsInChildren<DoodleLine>();
        // Debug.Log("ClearLines: lines.count=" + lines.Length);
        foreach(DoodleLine line in lines)
        {
           //  Debug.Log("line: " + line);
            Destroy(line.gameObject);
        }
    }

    void SetupRaycastLogic(DoodleLine doodleLine)
    {
        doodleLine.raycastDelegate = GetNonArRaycastLogic;
        
        doodleLine.gameObject.SetActive(true);
    }

    bool GetNonArRaycastLogic(out Vector3 hitPosition)
    {
        var point = Input.mousePosition;


        Ray ray = camera.ScreenPointToRay(point);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitPosition = hit.point;
            return true;
        }
        else
        {
            hitPosition = Vector3.zero;
            return false;
        }
    }
    
}
