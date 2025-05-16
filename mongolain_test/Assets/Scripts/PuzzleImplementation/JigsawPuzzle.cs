using UnityEngine;
using System.Collections.Generic;

public class JigsawPuzzle : PuzzleBase
{
    //this is just a test puzzle to see how the drop zone and stuff works (experimental not needed for the game)
    [Header("Puzzle Pieces")]
    public GameObject[] puzzlePieces; //take an image and split it up in a grid in kita.
    public GameObject[] dropZones;
    
    [Header("Settings")]
    [SerializeField] private bool scramblePiecesOnStart = true;
    [SerializeField] private Transform pieceContainer;
    [SerializeField] private Rect scrambleArea = new Rect(-5, -3, 10, 6);
    
    [Header("Feedback")]
    [SerializeField] private PuzzleFeedbackController feedbackController;
    
    private int correctPiecesPlaced = 0;
    private Dictionary<string, Vector3> originalPositions = new Dictionary<string, Vector3>();
    
    protected override void Awake()
    {
        base.Awake();
        
        // Store original positions of all pieces
        foreach (var piece in puzzlePieces)
        {
            originalPositions[piece.name] = piece.transform.position;
        }
    }
    
    public override void StartPuzzle()
    {
        base.StartPuzzle();
        
        // Setup puzzle pieces
        correctPiecesPlaced = 0;
        
        // Setup drag and drop handlers
        SetupPieces();
        
        // Optional: Scramble pieces
        if (scramblePiecesOnStart)
        {
            ScramblePieces();
        }
    }
    
    private void SetupPieces()
    {
        // Connect pieces to drop zones
        if (puzzlePieces.Length != dropZones.Length)
        {
            Debug.LogError("Number of puzzle pieces doesn't match number of drop zones!");
            return;
        }
        
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            var piece = puzzlePieces[i];
            
            // Setup draggable component
            var draggable = piece.GetComponent<ItemDraggable>();
            if (draggable == null)
                draggable = piece.AddComponent<ItemDraggable>();
            
            // Set puzzle ID to match our puzzle
            draggable.puzzleId = this.puzzleID;
            
            // Connect drag end event
            draggable.onDragEnd.AddListener(OnPieceDragEnd);
            
            // Setup drop zone for this piece
            var dropZone = dropZones[i].GetComponent<DropZone>();
            if (dropZone == null)
                dropZone = dropZones[i].AddComponent<DropZone>();
            
            // Set same puzzle ID
            dropZone.puzzleId = this.puzzleID;
            
            // Connect drop event
            dropZone.onItemDropped.AddListener(OnPieceDropped);
        }
    }
    
    private void ScramblePieces()
    {
        foreach (var piece in puzzlePieces)
        {
            // Random position within scramble area
            float randomX = Random.Range(scrambleArea.xMin, scrambleArea.xMax);
            float randomY = Random.Range(scrambleArea.yMin, scrambleArea.yMax);
            
            piece.transform.position = new Vector3(randomX, randomY, piece.transform.position.z);
        }
    }
    
    private void OnPieceDropped(GameObject pieceObj)
    {
        // Find the correct drop zone for this piece
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            if (puzzlePieces[i] == pieceObj)
            {
                // Check if it was dropped in its correct zone
                var dropZoneCollider = dropZones[i].GetComponent<Collider2D>();
                if (dropZoneCollider.bounds.Contains(pieceObj.transform.position))
                {
                    // Correct placement!
                    correctPiecesPlaced++;
                    
                    // Snap to exact position
                    pieceObj.transform.position = dropZones[i].transform.position;
                    
                    // Play success feedback
                    if (feedbackController != null)
                    {
                        feedbackController.ShowSuccessFeedback(pieceObj.transform.position);
                    }
                    
                    // Disable dragging now that it's correctly placed
                    var draggable = pieceObj.GetComponent<ItemDraggable>();
                    if (draggable != null)
                    {
                        draggable.SetDraggable(false);
                    }
                    
                    // Check if puzzle is complete
                    CheckPuzzleCompletion();
                    
                    return;
                }
            }
        }
        
        // If we get here, it was dropped in wrong zone
        if (feedbackController != null)
        {
            feedbackController.ShowFailureFeedback(pieceObj.transform.position);
        }
    }
    
    private void OnPieceDragEnd(ItemDraggable draggable, Vector3 position)
    {
        // This is handled by the DropZone's OnTriggerEnter
    }
    
    private void CheckPuzzleCompletion()
    {
        if (correctPiecesPlaced >= puzzlePieces.Length)
        {
            CompletePuzzle();
        }
    }
    
    public override void Reset()
    {
        base.Reset();
        
        correctPiecesPlaced = 0;
        
        // Reset pieces to original positions
        foreach (var piece in puzzlePieces)
        {
            if (originalPositions.TryGetValue(piece.name, out Vector3 originalPos))
            {
                piece.transform.position = originalPos;
            }
            
            // Re-enable dragging
            var draggable = piece.GetComponent<ItemDraggable>();
            if (draggable != null)
            {
                draggable.SetDraggable(true);
            }
        }
    }
}