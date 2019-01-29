using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationMenuItem : MonoBehaviour
{
    public RotationMenuManager rotationMenuManager;

    [Space()]
    public int Index;

    [Space()]
    public RotationMenuItem leftSetItem;
    public RotationMenuItem leftItem;// { get; set; }
    public RotationMenuItem rightItem;// { get; set; }
    public RotationMenuItem rightSetItem;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = Vector3.up;
        float factor = Vector3.Angle(-rotationMenuManager.CenterMenuDirection, transform.position - rotationMenuManager.transform.position) / 180f;

        float size = rotationMenuManager.MeneItemSizeCurve.Evaluate(factor);
        rectTransform.localScale = new Vector3(size, size, 1f);

        if (factor >= 0.99f)
        {
            rotationMenuManager.SetCenterItem(this);
        }
    }

    public void SetManager(RotationMenuManager manager)
    {
        rotationMenuManager = manager;
    }

    public void SetButton(int idx)
    {
        Index = idx;
    }

    public void In()
    {

    }

    public void Out()
    {

    }
}
