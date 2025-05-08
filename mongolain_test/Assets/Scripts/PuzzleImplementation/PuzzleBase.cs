public abstract class PuzzleBase : MonoBehaviour, IPuzzle
{
    [SerializeField] protected string puzzleId;
    [SerializeField] protected bool startActive = false;
    [SerializeField] protected bool UnityEvent onPuzzleComplete;

    public bool isCompleted { get; private set; }
    public event System.Action<IPuzzle> OnPuzzleCompleted;

    protected virtual void Awake()
    {
        // Initialize puzzle state
        isCompleted = false;
    }
    protected virtual void Start()
    {
        if (startActive)
        {
            StartPuzzle();
        }
    }

    public virtual void StartPuzzle()
    {
        gameObject.SetActive(true);
        isCompleted = false;
    }

    public virtual void Reset()
    {
        isCompleted = false;
    }

    protected void CompletePuzzle()
    {
        isCompleted = true;
        OnPuzzleCompleted?.Invoke(this);

    }
}