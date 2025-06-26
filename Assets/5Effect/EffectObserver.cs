using System;
using UniRx;

public class EffectObserver : IDisposable
{
    public readonly BoolReactiveProperty flag = new BoolReactiveProperty(false);
    
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    public EffectObserver()
    {
        flag.AddTo(_disposable);
    }
    
    public void Dispose()
    {
        if(_disposable.IsDisposed) return;
        _disposable.Dispose();
    }
}
