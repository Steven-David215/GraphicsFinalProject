﻿/*
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
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class DoodleUI : MonoBehaviour
{
    public DoodlePen pen;

    const float UIHeight = 60;      // this is the height of the DoodlePanel


    public GameObject scanSurfacePanel;
    public GameObject doodlePanel;

    public ARPlaneManager planeManager;
    public bool arMode;

    [SerializeField] Text unitText;
    
    // Start is called before the first frame update
    void Start()
    {
        // Define the boundry of the doodle pen prevent conflict UI touches
        SetPenDrawingBound();

        if (arMode)
        {
            // 1
            SetDoodleUIVisible(false);
            SetCoachingUIVisible(true);

            // 2
            planeManager.planesChanged += PlanesChanged;
        }
        else
        {
            // 3
            SetDoodleUIVisible(true);
            SetCoachingUIVisible(false);
        }


    }


    void SetPenDrawingBound()
    {
        if(pen == null)
        {
            return;
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            return;
        }

        pen.drawingBound = canvas.scaleFactor * UIHeight;
    }


    public void OnClearClicked()
    {
        if (pen != null)
        {
            pen.ClearLines();
        }
    }

    public void OnColorClicked(int index)
    {
        if (pen != null)
        {
            pen.ChangeColorIndex(index);
        }
    }

    public void SetCoachingUIVisible(bool flag)
    {
        scanSurfacePanel.SetActive(flag);
    }

    public void SetDoodleUIVisible(bool flag)
    {
        doodlePanel.SetActive(flag);
    }

    public void OnLineWidthChange(float value)
    {
        
        if (pen != null)
        {
            pen.ChangeLineWidth(value * 0.001f);
        }

        if(unitText != null)
        {
            unitText.text = string.Format("{0:0.0} cm", (value * 0.1f));
        }
    }

    private void PlanesChanged(ARPlanesChangedEventArgs planeEvent)
    {
        if (planeEvent.added.Count > 0 || planeEvent.updated.Count > 0)
        {
            SetDoodleUIVisible(true);
            SetCoachingUIVisible(false);

            planeManager.planesChanged -= PlanesChanged;
        }
    }

}
