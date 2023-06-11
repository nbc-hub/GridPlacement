using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera sceneCam;

    [SerializeField]
    private Vector3 lastCamPosition;

    public event Action OnClicked,OnExit,OnRotate;

    [SerializeField] LayerMask placementLayermask;
    
    public Vector3 GetSelectedMapPosition(){
        Vector3 mousePos=Input.mousePosition;
        mousePos.z=sceneCam.nearClipPlane;
        Ray ray=sceneCam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,100,placementLayermask)){
            lastCamPosition=hit.point;
        }
        return lastCamPosition;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            OnClicked?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            OnExit?.Invoke();
        }
        if(Input.GetMouseButtonDown(1)){
            OnRotate?.Invoke();
        }
    }

    public bool IsPointerOverUI(){
        return EventSystem.current.IsPointerOverGameObject();
    }
}
