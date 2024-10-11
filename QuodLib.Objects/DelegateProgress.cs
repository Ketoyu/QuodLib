namespace QuodLib.Objects;

/// <summary>
/// A delegate implementation of <see cref="IProgress{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Use this when you need an <see cref="IProgress{T}"/> to consume &amp; process results; <see cref="Progress{T}"/> is
/// intended only for sending info to the UI
/// [source <see href="https://stackoverflow.com/questions/32917597/not-every-result-of-iprogressint-comes-out-of-the-task">here</see>].
/// </remarks>
public class DelegateProgress<T> : IProgress<T> {
    private Action<T> OnChange { get; }

    public DelegateProgress(Action<T> onChange) {
        OnChange = onChange;
    }
    
    /// <summary>
    /// Reports a progress update.
    /// </summary>
    /// <param name="value">The value of the updated progress.</param>
    public void Report(T value) {
        OnChange.Invoke(value);
    }
}