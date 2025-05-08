public interface IPuzzle{
    bool isCompleted{get;}
    void StartPuzzle();
    void Reset();
    event System.Action<IPuzzle> OnPuzzleCompleted;
}