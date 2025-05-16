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
            var drag = obj.AddComponent<SimpleDraggable>();
            drag.onDropped += OnIngredientDropped;
        }
    }

    void OnIngredientDropped(GameObject ingredientObj, Vector3 dropPos)
    {
        if (puzzleCompleted) return;

        float dist = Vector3.Distance(dropPos, cauldronTransform.position);
        if (dist <= cauldronRadius)
        {
            string id = ingredientObj.name; // Use GameObject name as ID, or add a custom script for IDs
            if (!currentIngredients.Contains(id))
                currentIngredients.Add(id);

            ingredientObj.SetActive(false); // Hide after use

            CheckRecipe();
        }
        else
        {
            // Snap back to original position
            ingredientObj.GetComponent<SimpleDraggable>().ResetPosition();
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
public class SimpleDraggable : MonoBehaviour
{
    public System.Action<GameObject, Vector3> onDropped;
    private Vector3 startPos;
    private bool dragging;

    void Start() => startPos = transform.position;

    void OnMouseDown() => dragging = true;

    void OnMouseUp()
    {
        dragging = false;
        onDropped?.Invoke(gameObject, transform.position);
    }

    void Update()
    {
        if (dragging)
        {
            var mouse = Input.mousePosition;
            mouse.z = Camera.main.WorldToScreenPoint(transform.position).z;
            transform.position = Camera.main.ScreenToWorldPoint(mouse);
        }
    }

    public void ResetPosition() => transform.position = startPos;
}