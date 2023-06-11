using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position,Quaternion rotation,Vector3 childPos)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.GetChild(0).rotation = rotation;
        newObject.transform.GetChild(0).localPosition = childPos;
        newObject.transform.position = position;
        Vector3 placementPositon = newObject.transform.position;
        if(prefab.name != "FloorParent"){
        Tween.Position(newObject.transform, placementPositon + new Vector3(0, 0.5f, 0), placementPositon, 1f, 0, Tween.EaseBounce);
        }
        placedGameObjects.Add(newObject);
        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex 
            || placedGameObjects[gameObjectIndex] == null)
        {
            return;
        }
        Vector3 placementPositon = placedGameObjects[gameObjectIndex].transform.position;
        Tween.Position(placedGameObjects[gameObjectIndex].transform, placementPositon, placementPositon - new Vector3(0, 1f, 0), 1f, 0, Tween.EaseLinear);
        Destroy(placedGameObjects[gameObjectIndex],1f);
        
        placedGameObjects[gameObjectIndex] = null;
    }
}
