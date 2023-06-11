using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingState(
                         Grid grid,
                         PreviewSystem previewSystem,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    void IBuildingState.EndState()
    {
        previewSystem.StopShowingPreview();
    }

    void IBuildingState.OnAction(Vector3Int gridPosition,Quaternion quaternion,Vector3 childPos,Vector3 size)
    {
            
        GridData selectedData=null;
        if(!furnitureData.CanPlaceObjectAt(gridPosition,Vector2Int.one)){
            selectedData=furnitureData;
        }else if(!floorData.CanPlaceObjectAt(gridPosition,Vector2Int.one)){
            selectedData=floorData;
        }

        if(selectedData == null){
        }else{
            gameObjectIndex=selectedData.GetRePresentationIndex(gridPosition);
            if(gameObjectIndex==-1){
                return;
            }
            Debug.Log("değil");
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 cellPosition=grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition,CheckSelectionIsValid(gridPosition));
    }

    private bool CheckSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition,Vector2Int.one) 
                    && floorData.CanPlaceObjectAt(gridPosition,Vector2Int.one));
    }

    void IBuildingState.UpdateState(Vector3Int gridPosition)
    {
        bool validity=CheckSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),validity);
    }
}
