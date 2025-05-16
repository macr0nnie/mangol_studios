using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotionCreationPuzzle : PuzzleBase
{
    [SerializeField] private IngredientRepository ingredientRepository;
    [SerializeField] private PuzzleRecipieValidator recipeValidator;
    [SerializeField] private PuzzleDropZone cauldronZone;
    [SerializeField] private PuzzleFeedbackController feedbackController;

    [SerializeField] private float brewingDelay = 1.5f;
    [SerializeField] private GameObject brewingEffectPrefab;

    // Events
    public UnityEvent<string> onPotionCreated;

    private List<GameObject> activeBrewingEffects = new List<GameObject>();

    private void OnEnable()
    {
        // Subscribe to events
        if (cauldronZone != null)
        {
            cauldronZone.onItemDropped.AddListener(OnItemDroppedInCauldron);
        }
        
        if (ingredientRepository != null)
        {
            ingredientRepository.onIngredientsChanged.AddListener(OnIngredientsChanged);
        }
        
        if (recipeValidator != null)
        {
            recipeValidator.onRecipeMatch.AddListener(OnRecipeMatch);
            recipeValidator.onTargetRecipeMatch.AddListener(OnTargetRecipeMatch);
        }
    }
    
    private void OnDisable()
    {
        // Unsubscribe from events
        if (cauldronZone != null)
        {
            cauldronZone.onItemDropped.RemoveListener(OnItemDroppedInCauldron);
        }
        
        if (ingredientRepository != null)
        {
            ingredientRepository.onIngredientsChanged.RemoveListener(OnIngredientsChanged);
        }
        
        if (recipeValidator != null)
        {
            recipeValidator.onRecipeMatch.RemoveListener(OnRecipeMatch);
            recipeValidator.onTargetRecipeMatch.RemoveListener(OnTargetRecipeMatch);
        }
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();
        
        // Reset all systems
        ResetPuzzleState();
        
        // Setup ingredient draggables
        SetupIngredients();
    }
    
    private void SetupIngredients()
    {
        if (ingredientRepository != null)
        {
            // Access all ingredient objects and ensure they have draggable components
            var ingredients = ingredientRepository.GetAllIngredientObjects();
            
            foreach (var ingredient in ingredients)
            {
                // Get or add draggable component
                ItemDraggable draggable = ingredient.GetComponent<ItemDraggable>();
                if (draggable == null)
                {
                    draggable = ingredient.AddComponent<ItemDraggable>();
                }
                
                // Reset position and make draggable
                draggable.ResetPosition();
                draggable.SetDraggable(true);
                
                // Connect drag end event
                draggable.onDragEnd.RemoveListener(OnIngredientDragEnd); // Avoid duplicates
                draggable.onDragEnd.AddListener(OnIngredientDragEnd);
            }
        }
    }
    
    private void OnIngredientDragEnd(PuzzleItemDraggable draggable, Vector3 position)
    {
        // This method is actually not needed with our modular approach
        // as the cauldron drop zone will handle the dropping logic
        // and broadcast its own events, which we listen to.
    }
    
    private void OnItemDroppedInCauldron(GameObject item)
    {
        if (ingredientRepository == null || item == null) return;
        
        // Get the ingredient ID from the GameObject
        string ingredientId = ingredientRepository.GetIngredientIdByGameObject(item);
        
        if (!string.IsNullOrEmpty(ingredientId))
        {
            // Add the ingredient to our active ingredients
            ingredientRepository.AddIngredient(ingredientId);
            
            // Play a brewing effect
            CreateBrewingEffect(cauldronZone.transform.position);
            
            // Tell the draggable it was dropped in a valid zone
            ItemDraggable draggable = item.GetComponent<ItemDraggable>();
            if (draggable != null)
            {
                draggable.SetDroppedInValidZone(true);
            }
            
            // Play feedback
            if (feedbackController != null)
            {
                feedbackController.ShowActionFeedback(cauldronZone.transform.position);
            }
        }
    }
    
    private void OnIngredientsChanged(List<string> currentIngredients)
    {
        if (recipeValidator == null || currentIngredients.Count == 0) return;
        
        // Validate the current ingredients against recipes
        recipeValidator.ValidateRecipe(currentIngredients);
    }
    
    private void OnRecipeMatch(RecipeData recipe)
    {
        // A recipe was matched (any recipe, not necessarily the target)
        
        // Spawn the result object if available
        if (recipe.resultPrefab != null && cauldronZone != null)
        {
            Instantiate(recipe.resultPrefab, 
                cauldronZone.transform.position + Vector3.up, 
                Quaternion.identity);
        }
        
        // Notify listeners
        onPotionCreated?.Invoke(recipe.recipeName);
        
        // Play success feedback
        if (feedbackController != null)
        {
            feedbackController.ShowSuccessFeedback(cauldronZone.transform.position);
        }
    }
    
    private void OnTargetRecipeMatch(RecipeData recipe)
    {
        // The target recipe was matched - this completes the puzzle!
        
        // A slight delay before completing looks more natural
        StartCoroutine(DelayedComplete(recipe));
    }
    
    private IEnumerator DelayedComplete(RecipeData recipe)
    {
        yield return new WaitForSeconds(brewingDelay);
        
        // Play extra success feedback
        if (feedbackController != null)
        {
            feedbackController.ShowSuccessFeedback(cauldronZone.transform.position);
        }
        
        // Complete the puzzle
        CompletePuzzle();
    }
    
    private void CreateBrewingEffect(Vector3 position)
    {
        if (brewingEffectPrefab == null) return;
        
        GameObject effect = Instantiate(brewingEffectPrefab, position, Quaternion.identity);
        activeBrewingEffects.Add(effect);
        
        // Clean up effect after delay
        StartCoroutine(DestroyEffectAfterDelay(effect, 3f));
    }
    
    private IEnumerator DestroyEffectAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (effect != null)
        {
            activeBrewingEffects.Remove(effect);
            Destroy(effect);
        }
    }
    
    private void ResetPuzzleState()
    {
        // Reset ingredient repository
        if (ingredientRepository != null)
        {
            ingredientRepository.ClearIngredients();
            ingredientRepository.ResetIngredients();
        }
        
        // Reset cauldron zone
        if (cauldronZone != null)
        {
            cauldronZone.ResetZone();
        }
        
        // Clean up any effects
        foreach (var effect in activeBrewingEffects)
        {
            if (effect != null)
            {
                Destroy(effect);
            }
        }
        activeBrewingEffects.Clear();
    }
    
    public override void Reset()
    {
        base.Reset();
        ResetPuzzleState();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        // Clean up any effects
        foreach (var effect in activeBrewingEffects)
        {
            if (effect != null)
            {
                Destroy(effect);
            }
        }
    }
}