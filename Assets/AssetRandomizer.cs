using System.Collections.Generic;
using UnityEngine;

public class AssetRandomizer : MonoBehaviour
{
    int childCount;
    [SerializeField] List<Transform> assetList;
    [SerializeField] Vector3 originalGOScale;
    [SerializeField] Vector3 minScaleOffset, maxScaleOffset;
    [SerializeField] bool allowFlipping = false;

    public void FillTheList()
    {
        assetList.Clear();
        childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            assetList.Add(transform.GetChild(i));
        }
    }

    public void ResetSize()
    {
        if (assetList.Count == 0)
        {
            Debug.Log("liste Vide");
            return;
        }
        foreach (Transform asset in assetList)
        {
            asset.localScale = originalGOScale;
        }
    }

    public void RandomizeSize()
    {
        if (assetList.Count == 0)
        {
            Debug.Log("liste Vide");
            return;
        }
        foreach (Transform asset in assetList)
        {
            asset.localScale = new Vector3(
                Random.Range(minScaleOffset.x, maxScaleOffset.x),
                Random.Range(minScaleOffset.y, maxScaleOffset.y),
                Random.Range(minScaleOffset.z, maxScaleOffset.z));
            if (allowFlipping && Random.value > 0.5f)
            {
                asset.localScale = new Vector3(
                    -asset.localScale.x,
                    asset.localScale.y,
                    asset.localScale.z);
            }
        }
    }



}
