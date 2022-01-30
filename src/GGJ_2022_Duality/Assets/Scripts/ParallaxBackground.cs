using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private Vector2 parallaxEffectMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCamPos;
    private PlayerController pc;
    private SpriteRenderer sr;
    float textureUnitSizeX;
    private void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;

        cameraTransform = pc.GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        lastCamPos = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPos = cameraTransform.position;

        if (cameraTransform.position.x - transform.position.x >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
