using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostRenderer : MonoBehaviour {
    // Dimensions of your wrap-around world (assume centered at (0,0))
    public float worldWidth = 50f;
    public float worldHeight = 50f;
    
    // List to hold ghost copies
    private List<GameObject> ghostCopies = new();
    private SpriteRenderer mainSpriteRenderer;

    void Start() {
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        
        // Create ghost copies for all 8 surrounding offsets
        for (int xOffset = -1; xOffset <= 1; xOffset++) {
            for (int yOffset = -1; yOffset <= 1; yOffset++) {
                // Skip the (0,0) offset because that's the original object
                if (xOffset == 0 && yOffset == 0)
                    continue;
                
                GameObject ghost = new GameObject("Ghost_" + xOffset + "_" + yOffset);
                // Optional: Set the ghost as a child (for easy hierarchy tracking)
                ghost.transform.parent = transform;
                
                // Add a SpriteRenderer to the ghost and copy properties from the main object
                SpriteRenderer ghostSR = ghost.AddComponent<SpriteRenderer>();
                ghostSR.sprite = mainSpriteRenderer.sprite;
                ghostSR.sortingLayerID = mainSpriteRenderer.sortingLayerID;
                ghostSR.sortingOrder = mainSpriteRenderer.sortingOrder;
                ghostSR.color = mainSpriteRenderer.color;
                
                ghostCopies.Add(ghost);
            }
        }
    }

    void LateUpdate() {
        int index = 0;
        // For each ghost copy, calculate its offset position relative to the main object
        for (int xOffset = -1; xOffset <= 1; xOffset++) {
            for (int yOffset = -1; yOffset <= 1; yOffset++) {
                if (xOffset == 0 && yOffset == 0)
                    continue;
                
                // Determine the offset based on world dimensions
                Vector3 offset = new Vector3(xOffset * worldWidth, yOffset * worldHeight, 0);
                // Position the ghost copy accordingly
                ghostCopies[index].transform.position = transform.position + offset;
                // Make sure its rotation and scale match the main object
                ghostCopies[index].transform.rotation = transform.rotation;
                ghostCopies[index].transform.localScale = transform.localScale;
                index++;
            }
        }
    }
}
