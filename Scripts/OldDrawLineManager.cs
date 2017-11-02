using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldDrawLineManager : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    private LineRenderer currentLine;
    private int numClicks = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            GameObject draw = new GameObject();
            currentLine = draw.AddComponent<LineRenderer>();
            currentLine.SetWidth(0.1f, 0.1f);
            numClicks = 0;
        }
        else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            currentLine.SetVertexCount(numClicks + 1);
            currentLine.SetPosition(numClicks, trackedObj.transform.position);

            numClicks++;
        }

    }
}
