using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int gridPosition,Quaternion quaternion,Vector3 childPos,Vector3 size);
    void UpdateState(Vector3Int gridPosition);
}
