using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotionCreationPuzzle : PuzzleBase
{
    //refactor this to use the draggable item components to make it have less code
    [System.Serializable]
    public class Ingredient
    {
        public GameObject ingredientObject;
        public string ingredientId;
    }

    [System.Serializable]
    public class PotionRecipe
    {
        public string potionName;
        public List<string> requiredIngredients;
        public GameObject potionPrefab;
    }

    [Header("Ingredients")]
    [SerializeField] private List<Ingredient> availableIngredients;

    [Header("Recipes")]
    [SerializeField] private List<PotionRecipe> validRecipes;
    [SerializeField] private PotionRecipe targetRecipe; // The recipe the player needs to make

    [Header("Cauldron")]
    [SerializeField] private Transform cauldronTransform;
    [SerializeField] private float cauldronRadius = 1.5f;
    [SerializeField] private ParticleSystem brewingEffect;

    [Header("Feedback")]
    [SerializeField] private AudioClip? ingredientAddedSound;
    [SerializeField] private AudioClip? correctPotionSound;
    [SerializeField] private AudioClip? wrongPotionSound;

    // Events
    public UnityEvent<string> onIngredientAdded;
    public UnityEvent<string> onPotionCreated;

    // State tracking
    private List<string> currentIngredients = new List<string>();
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (ingredientAddedSound != null || correctPotionSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (brewingEffect != null)
        {
            brewingEffect.Stop();
        }
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();

        currentIngredients.Clear();

        // Set up all ingredients with drag behaviors
        foreach (var ingredient in availableIngredients)
        {
            if (ingredient.ingredientObject != null)
            {
                ingredient.ingredientObject.SetActive(true);

                DragDrop dragDrop = ingredient.ingredientObject.GetComponent<DragDrop>();
                if (dragDrop == null)
                {
                    dragDrop = ingredient.ingredientObject.AddComponent<DragDrop>();
                }

                // Remove any existing handlers to prevent duplicates
                dragDrop.onPieceDropped -= OnIngredientDropped;
                dragDrop.onPieceDropped += OnIngredientDropped;
            }
        }
    }

    private void OnIngredientDropped(GameObject ingredientObject, Vector3 position)
    {
        // Check if ingredient was dropped in cauldron
        if (cauldronTransform != null)
        {
            float distance = Vector2.Distance(
                new Vector2(position.x, position.z),
                new Vector2(cauldronTransform.position.x, cauldronTransform.position.z)
            );

            if (distance <= cauldronRadius)
            {
                // Find the ingredient ID
                Ingredient droppedIngredient = availableIngredients.Find(i => i.ingredientObject == ingredientObject);

                if (droppedIngredient != null)
                {
                    // Add to current ingredients
                    currentIngredients.Add(droppedIngredient.ingredientId);

                    // Play effect
                    if (brewingEffect != null)
                    {
                        brewingEffect.Play();
                    }

                    // Play sound
                    if (audioSource != null && ingredientAddedSound != null)
                    {
                        audioSource.PlayOneShot(ingredientAddedSound);
                    }

                    // Trigger event
                    onIngredientAdded?.Invoke(droppedIngredient.ingredientId);

                    // Remove or hide the ingredient
                    ingredientObject.SetActive(false);

                    // Check if a potion is ready
                    CheckPotionCreation();
                }
            }
            else
            {
                // Return to original position
                DragDrop dragDrop = ingredientObject.GetComponent<DragDrop>();
                if (dragDrop != null)
                {
                    dragDrop.ResetPosition();
                }
            }
        }
    }

    private void CheckPotionCreation()
    {
        if (currentIngredients.Count < 2) return; // Need at least 2 ingredients

        // Check if current ingredients match any recipe
        foreach (var recipe in validRecipes)
        {
            bool isMatch = true;

            // Check if all required ingredients are present
            foreach (string requiredIngredient in recipe.requiredIngredients)
            {
                if (!currentIngredients.Contains(requiredIngredient))
                {
                    isMatch = false;
                    break;
                }
            }

            // Check if no extra ingredients were added
            if (isMatch && currentIngredients.Count == recipe.requiredIngredients.Count)
            {
                // Potion created!
                onPotionCreated?.Invoke(recipe.potionName);

                // Play correct effect
                if (audioSource != null)
                {
                    if (recipe == targetRecipe)
                    {
                        audioSource.PlayOneShot(correctPotionSound);
                        // Correct potion created
                        CompletePuzzle();
                    }
                    else
                    {
                        audioSource.PlayOneShot(wrongPotionSound);
                        // Wrong potion, reset
                        StartCoroutine(ResetAfterDelay(2f));
                    }
                }

                // Instantiate potion object if available
                if (recipe.potionPrefab != null)
                {
                    Instantiate(recipe.potionPrefab, cauldronTransform.position + Vector3.up, Quaternion.identity);
                }

                return;
            }
        }
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Reset();
    }

    public override void Reset()
    {
        base.Reset();

        // Clear current ingredients
        currentIngredients.Clear();

        // Reset all ingredients
        foreach (var ingredient in availableIngredients)
        {
            if (ingredient.ingredientObject != null)
            {
                ingredient.ingredientObject.SetActive(true);

                DragDrop dragDrop = ingredient.ingredientObject.GetComponent<DragDrop>();
                if (dragDrop != null)
                {
                    dragDrop.ResetPosition();
                }
            }
        }
        // Stop effects
        if (brewingEffect != null)
        {
            brewingEffect.Stop();
        }
    }
    public override void OnDisable()
    {
        // Clean up event handlers
        foreach (var ingredient in availableIngredients)
        {
            if (ingredient.ingredientObject != null)
            {
                DragDrop dragDrop = ingredient.ingredientObject.GetComponent<DragDrop>();
                if (dragDrop != null)
                {
                    dragDrop.onPieceDropped -= OnIngredientDropped;
                }
            }
        }
    }
}