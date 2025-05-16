using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimplePotionPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        public List<string> ingredientIds;
        public GameObject resultPrefab;
    }
    [Header("Puzzle Setup")]
    public List<GameObject> ingredientObjects; // Assign draggable ingredient GameObjects in Inspector
    public Transform cauldronTransform;
    public float cauldronRadius = 2f;
    public List<Recipe> recipes;
    public string targetRecipeName;

    [Header("Feedback")]
    public UnityEvent onPuzzleComplete;
    public UnityEvent onWrongCombination;

    private List<string> currentIngredients = new List<string>();
    private bool puzzleCompleted = false;

    void Start()
    {
        foreach (var obj in ingredientObjects)
        {
            // Add or get ItemDraggable
            var drag = obj.GetComponent<ItemDraggable>();
            if (drag == null)
                drag = obj.AddComponent<ItemDraggable>();

            // Subscribe to drag end event
            drag.onDragEnd.AddListener(OnIngredientDragEnd);
        }
    }

    // Handler for when dragging ends
    private void OnIngredientDragEnd(ItemDraggable draggable, Vector3 dropPos)
    {
        if (puzzleCompleted) return;

        float dist = Vector3.Distance(dropPos, cauldronTransform.position);
        if (dist <= cauldronRadius)
        {
            string id = draggable.gameObject.name; // Or use a custom ID script/component
            if (!currentIngredients.Contains(id))
                currentIngredients.Add(id);

            draggable.gameObject.SetActive(false); // Hide after use
            draggable.SetDroppedInValidZone(true);

            CheckRecipe();
        }
        else
        {
            // Snap back to original position
            draggable.ResetPosition();
        }
    }

    void CheckRecipe()
    {
        foreach (var recipe in recipes)
        {
            if (MatchIngredients(recipe.ingredientIds, currentIngredients))
            {
                if (recipe.resultPrefab != null)
                    Instantiate(recipe.resultPrefab, cauldronTransform.position + Vector3.up, Quaternion.identity);

                if (recipe.recipeName == targetRecipeName)
                {
                    puzzleCompleted = true;
                    onPuzzleComplete?.Invoke();
                }
                else
                {
                    onWrongCombination?.Invoke();
                }
                currentIngredients.Clear();
                return;
            }
        }
        // Optionally, handle no match (wrong combination)
        onWrongCombination?.Invoke();
        currentIngredients.Clear();
    }

    bool MatchIngredients(List<string> recipe, List<string> attempt)
    {
        if (recipe.Count != attempt.Count) return false;
        var temp = new List<string>(attempt);
        foreach (var id in recipe)
        {
            if (!temp.Remove(id)) return false;
        }
        return temp.Count == 0;
    }
}