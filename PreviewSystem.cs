using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYoffSet = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialsInstance;

    private Renderer cellIndicatorRenderer;
    [SerializeField]
    InputManager inputManager;
    Quaternion lastRotationPreviewObject;
    Vector3 lastPositionPreviewObject;         



    private void Start()
    {
        previewMaterialsInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int Size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(Size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {

        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, size.y, 1);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialsInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);

    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.cyan : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;

        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    public void RotateObject()
    {
        if (previewObject.name != "FloorParent(Clone)")
        {
            previewObject.transform.GetChild(0).transform.Rotate(new Vector3(0, 90f, 0));
            lastRotationPreviewObject = previewObject.transform.GetChild(0).transform.rotation;

            previewObject.transform.GetChild(0).localPosition = new Vector3(previewObject.transform.GetChild(0).localPosition.z, 0, previewObject.transform.GetChild(0).localPosition.x);
            lastPositionPreviewObject = previewObject.transform.GetChild(0).localPosition;
            cellIndicator.transform.localScale = new Vector3(cellIndicator.transform.localScale.y, cellIndicator.transform.localScale.x, 1);

        }
    }

    private void MovePreview(Vector3 position)
    {

        previewObject.transform.position = new Vector3(position.x, position.y + previewYoffSet, position.z);
        lastPositionPreviewObject = previewObject.transform.GetChild(0).localPosition;

    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }

    public Quaternion GetPreviewRotateQuarternion()
    {
        return lastRotationPreviewObject;
    }

    public Vector3 GetPreviewChildPosition()
    {
        return lastPositionPreviewObject;
    }

    public Vector3 GetPreviewSize(){
        return cellIndicator.transform.localScale;
    }
}

