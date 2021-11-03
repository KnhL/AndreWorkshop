using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DecalController : MonoBehaviour
{
    public float spawnChance = 0.001f;
    private DecalProjector proj;
    public Material mat;
    public List<Material> decalMaterials;
    private void Start()
    {
        proj = GetComponent<DecalProjector>();
        if (Random.Range(0f,1f) < spawnChance)
        {
            proj.material = decalMaterials[Random.Range(0, decalMaterials.Count)];
            Debug.Log("Decal Spawned at: " + transform.position);
        }
    }
}
