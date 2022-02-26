using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class lightGi : MonoBehaviour
{
    [HideInInspector]
    public LightType lightType;
    //public int count = 5;
    Light light;
    Light[] lights;
    public float GIIntensity = 0.04f;
    public bool UseSurfaceColor = true;
    void Awake()
    {
        light = GetComponent<Light>();
        lights = new Light[5];
        lightType = light.type;
        CreateLights();
        DisableLights();
    }
    private void OnDisable()
    {
        DisableLights();
    }
    public void CreateLights()
    {
        for (int x = 0; x < 5; x++)
        {
            GameObject go = new GameObject("Light");
            go.transform.SetParent(transform);
            lights[x] = go.AddComponent<Light>();
            //lights[x].type = LightType.Spot;
            /*lights[x].spotAngle = 179f;
            lights[x].innerSpotAngle = 0;*/
        }
    }
    public void DisableLights()
    {
        foreach (Light item in lights)
        {
            item.enabled = false;
        }
    }
    void Update()
    {
        switch (lightType)
        {
            case LightType.Spot:
                float spotAngle = light.spotAngle;
                float width = spotAngle / 150;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, light.range, light.renderingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    lights[0].enabled = true;
                    lights[0].transform.position = hit.point;
                    //lights[0].transform.rotation = Quaternion.LookRotation(hit.normal);
                    lights[0].color = (UseSurfaceColor ? getColor(hit) : Color.white) * light.color * GIIntensity * (1 - (hit.distance / light.range)) * light.intensity;
                }
                else
                {
                    lights[0].enabled = false;
                }

                RaycastHit hit2;
                if (Physics.Raycast(transform.position, transform.forward + new Vector3(width, 0), out hit2, light.range, light.renderingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    lights[1].enabled = true;
                    lights[1].transform.position = hit2.point;
                    //lights[1].transform.rotation = Quaternion.LookRotation(hit2.normal);
                    lights[1].color = (UseSurfaceColor ? getColor(hit2) : Color.white) * light.color * GIIntensity * (1 - (hit2.distance / light.range)) * light.intensity;
                }
                else
                {
                    lights[1].enabled = false;
                }

                RaycastHit hit3;
                if (Physics.Raycast(transform.position, transform.forward + new Vector3(-width, 0), out hit3, light.range, light.renderingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    lights[2].enabled = true;
                    lights[2].transform.position = hit3.point;
                    //lights[2].transform.rotation = Quaternion.LookRotation(hit3.normal);
                    lights[2].color = (UseSurfaceColor ? getColor(hit3) : Color.white) * light.color * GIIntensity * (1 - (hit3.distance / light.range)) * light.intensity;
                }
                else
                {
                    lights[2].enabled = false;
                }

                RaycastHit hit4;
                if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, width), out hit4, light.range, light.renderingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    lights[3].enabled = true;
                    lights[3].transform.position = hit4.point;
                    //lights[3].transform.rotation = Quaternion.LookRotation(hit4.normal);
                    lights[3].color = (UseSurfaceColor ? getColor(hit4) : Color.white) * light.color * GIIntensity * (1 - (hit4.distance / light.range)) * light.intensity;
                }
                else
                {
                    lights[3].enabled = false;
                }

                RaycastHit hit5;
                if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, -width), out hit5, light.range, light.renderingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    lights[4].enabled = true;
                    lights[4].transform.position = hit5.point;
                    //lights[4].transform.rotation = Quaternion.LookRotation(hit5.normal);
                    lights[4].color = (UseSurfaceColor ? getColor(hit5) : Color.white) * light.color * GIIntensity * (1 - (hit5.distance / light.range)) * light.intensity;
                }
                else
                {
                    lights[4].enabled = false;
                }
                break;
            case LightType.Directional:
                break;
            case LightType.Point:
                break;
            default:
                break;
        }
    }
    public Color getColor(RaycastHit raycastHit)
    {
        Renderer renderer = raycastHit.collider.GetComponent<MeshRenderer>();
        Texture2D texture2D = renderer.material.mainTexture as Texture2D;
        Color color;
        if (texture2D != null)
        {
            Vector2 pCoord = raycastHit.textureCoord;
            pCoord.x *= texture2D.width;
            pCoord.y *= texture2D.height;

            Vector2 tiling = renderer.material.mainTextureScale;
            color = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x), Mathf.FloorToInt(pCoord.y * tiling.y));
        }
        else
        {
            color = renderer.material.color;
        }

        // Debug.Log(raycastHit.point.x + " " + raycastHit.point.y);
        return color;
    }
}
