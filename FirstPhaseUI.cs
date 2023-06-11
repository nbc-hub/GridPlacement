using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class FirstPhaseUI : MonoBehaviour
{
    [SerializeField] GameObject firstPhaseUIPanel;
    [SerializeField] GameObject furnitureUIPanel;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] List<GameObject> gameObjectsPrefabList;

    public void SpawnTemplate(int id)
    {
        if (id < gameObjectsPrefabList.Count)
        {
            GameObject templateObject = Instantiate(gameObjectsPrefabList[id]);

            templateObject.transform.parent = spawnPoint.transform;
            templateObject.transform.localPosition = new Vector3(3.88016f, -5.266243f, 1.181351f);
            Vector3 placementPositon = templateObject.transform.position;
            Tween.Position(templateObject.transform, placementPositon + new Vector3(0, 0.5f, 0), placementPositon, 1f, 0, Tween.EaseBounce);
            firstPhaseUIPanel.SetActive(false);
            furnitureUIPanel.SetActive(true);
        }
    }
}
