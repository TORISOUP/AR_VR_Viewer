using System;
using UniRx;

namespace VrmArPlayer
{
    /// <summary>
    /// Unit型を発行するAsyncSubjectのラッパー
    /// </summary>
    public class SingleEvent : IObservable<Unit>, IDisposable
    {
        private readonly AsyncSubject<Unit> _asyncSubject = new AsyncSubject<Unit>();
        private readonly object _gate = new object();

        public void Done()
        {
            lock (_gate)
            {
                if (_asyncSubject.IsCompleted) return;
                _asyncSubject.OnNext(Unit.Default);
                _asyncSubject.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            return _asyncSubject.Subscribe(observer);
        }

        public void Dispose()
        {
            _asyncSubject.Dispose();
        }
    }
}
