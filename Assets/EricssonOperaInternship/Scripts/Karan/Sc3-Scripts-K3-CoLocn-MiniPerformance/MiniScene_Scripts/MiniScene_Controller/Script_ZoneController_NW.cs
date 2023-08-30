using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class Script_ZoneController_NW : NetworkBehaviour
{
    //public NetworkObject VideoBoxPrefab;
    public GameObject sceneCamera;
    public NetworkObject remoteBall;
    private bool VideoBoxSpawnedFlag;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public GameObject ProfileMenuObj;
    public GameObject ViewerPanelObj;

    private int prevState;  // 1 for in, 0 for out
    private int currentState;  // 1 for in, 0 for out

    [SerializeField]
    private bool InZoneFlag;

    private Vector3 scaleChange;

    public TMP_Text hudUItext;

    public bool remoteStateAuthFlag;

    private void Start()
    {
        VideoBoxSpawnedFlag = false;
        originalPosition = transform.position;

        remoteBall.transform.position = originalPosition + new Vector3(0, -1.6f, 0);

        ViewerPanelObj.SetActive(false);
        ProfileMenuObj.SetActive(false);

        ViewerPanelObj.transform.position = sceneCamera.transform.position + new Vector3(0, 0.051f, 0.8f);
        ProfileMenuObj.transform.position = sceneCamera.transform.position + new Vector3(0, -0.25f, .8f);

        prevState = 0;
        currentState = 1;

        //targetPosition = new Vector3(0, 1.7f, 0);
        //targetRotation = Quaternion.identity;

        scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);

        remoteStateAuthFlag = false;
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        prevState = currentState;

        CheckInZoneFunc(sceneCamera);

        if (InZoneFlag == true)
        {
            currentState = 1;

            ForDebuggerFunc(currentState, prevState, "In Zone"); // current 1, prev 0

            if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.0f)
            {
                getRCfunc();

                ForDebuggerFunc(currentState, prevState, "In Zone, Got Remote.");

                if (OVRInput.GetUp(OVRInput.RawButton.X))
                {
                    //Debug.Log("Sphere Spawn Please");
                    //targetPosition = sceneCamera.transform.position;
                    //targetRotation = sceneCamera.transform.rotation;

                    //MiniPerf_Script_SceneManager.instance.DebugLogMessage("Spawn VideoBox");

                    //Runner.Spawn(VideoBoxPrefab, position: targetPosition, rotation: targetRotation);
                    //VideoBoxSpawnedFlag = true;
                }

                //VideoBoxSpawnedFlag = false;
            }
            else
            {
                ForDebuggerFunc(currentState, prevState, "In Zone, Released RemoteBall."); // current 0, prev 1

                releaseRCfunc();
            }
        }
        else
        {
            currentState = 0;

            ForDebuggerFunc(currentState, prevState, "Out of Zone, Released RemoteBall."); // current 0, prev 1

            releaseRCfunc();
        }

        //hudUItext.text = "X: " + sceneCamera.transform.position.x.ToString("F2") + "  Z: " + sceneCamera.transform.position.z.ToString("F2") +  "    inZoneFlag : " + InZoneFlag;
    }

    void getRCfunc()
    {
        
        remoteBall.RequestStateAuthority();

        // Checking if this Object has the StateAuthority of the RemoteBall Network Object ???
        if (remoteBall.HasStateAuthority)
        {
            MiniPerf_Script_SceneManager.instance.DebugLogMessage("Lycka Till");
        }
        
        remoteBall.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + new Vector3(0.02f, 0, 0.14f);
        
        if (remoteBall.transform.localScale.y > 0.02f && remoteBall.transform.localScale.y <= 0.1f)
        {
            remoteBall.transform.localScale -= scaleChange;
        }

        ViewerPanelObj.SetActive(true);
        ProfileMenuObj.SetActive(true);

        ViewerPanelObj.transform.position = sceneCamera.transform.position + new Vector3(0, 0.051f, 0.8f);
        ProfileMenuObj.transform.position = sceneCamera.transform.position + new Vector3(0, -0.25f, .8f);

        //transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
    }

    void releaseRCfunc()
    {
        remoteBall.ReleaseStateAuthority();
        
        remoteBall.transform.position = originalPosition  + new Vector3(0, -1.5f, 0); ;

        if (remoteBall.transform.localScale.y >= 0.02f && remoteBall.transform.localScale.y < 0.1f)
        {
            remoteBall.transform.localScale -= scaleChange;
        }

        ViewerPanelObj.SetActive(false);
        ProfileMenuObj.SetActive(false);
    }

    private void CheckInZoneFunc(GameObject cam)
    {
        float xPosMax = 0;
        float xPosMin = -3.0f;
        float zPosMax = 3.0f;
        float zPosMin = 0.0f;

        float camPosX = cam.transform.position.x;
        float camPosZ = cam.transform.position.z;

        bool internalBool;

        if (camPosX > xPosMin && camPosX < xPosMax && camPosZ > zPosMin && camPosZ < zPosMax)
        {
            InZoneFlag = true;
        }
        else 
        {
            InZoneFlag = false;
        }
    }

    private void ForDebuggerFunc(int current, int past, string msg) 
    {
        if (current != past)
        {
            MiniPerf_Script_SceneManager.instance.DebugLogMessage(msg);
        }
    }

    private void ShowViewerPanel()
    {

    }
}

