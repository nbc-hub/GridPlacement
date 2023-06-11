using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] ObjectsDatabaseSO database;

    [SerializeField]
    private ObjectPlacer objectPlacer;
    [SerializeField]
    private GameObject gridVisualization;

    [SerializeField]
    private GridData floorData, furnitureData;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private List<GameObject> placedGameObjects = new();

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();

    }
    void Update()
    {
        if (buildingState == null)
            return;
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }

    }

    private void StopPlacement()
    {
        if(buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnRotate -= StartRotate;
        lastDetectedPosition = Vector3Int.zero;
        buildingState=null;
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState= new PlacementState(ID,
                                          grid,
                                          previewSystem,
                                          database,
                                          floorData,
                                          furnitureData,
                                          objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        inputManager.OnRotate += StartRotate;
    }
    
    public void StartRemoving(){
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState= new RemovingState(grid,previewSystem,floorData,furnitureData,objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

       buildingState.OnAction(gridPosition,previewSystem.GetPreviewRotateQuarternion(),previewSystem.GetPreviewChildPosition(),previewSystem.GetPreviewSize());
    }

    private void StartRotate(){
        previewSystem.RotateObject();
    }

    // private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    // {
    //     GridData selectedData = database.objectDatas[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
    //     return selectedData.CanPlaceObjectAt(gridPosition, database.objectDatas[selectedObjectIndex].Size);
    // }
}
