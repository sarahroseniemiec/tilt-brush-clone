using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour {

    public Material lineMaterial;

    public SteamVR_TrackedObject trackedObj;
    private MeshLineRenderer currentLine;
    private int numClicks = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
            GameObject draw = new GameObject();
            draw.tag = "line";
            draw.AddComponent<MeshFilter>();
            draw.AddComponent<MeshRenderer>();
            currentLine = draw.AddComponent<MeshLineRenderer>();
            currentLine.lmat = lineMaterial;
            currentLine.setWidth(0.05f);
            numClicks = 0;
        } else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            //currentLine.SetVertexCount(numClicks + 1);
            //currentLine.SetPosition(numClicks, trackedObj.transform.position);
            currentLine.AddPoint(trackedObj.transform.position);
            numClicks ++;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis0))
        {
            
            if ( currentLine != null) {
                GameObject[] lines;
                lines = GameObject.FindGameObjectsWithTag("line");

                for (var i = 0; i < lines.Length; i++)
                {
                    Destroy(lines[i]);
                   
                }
            }
            
        }

    }
    
    
    
}
