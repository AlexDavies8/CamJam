using System;

namespace Optional
{
    public struct Lazy<T>
    {
        private Option<T> _value;
        public T Value
        {
            get {
                if (_value.IsNone())
                {
                    _value = Option<T>.Some(_factory());
                }

                return _value.UnwrapOr(default);
            }
        }

        private Func<T> _factory;

        public Lazy(Func<T> factory)
        {
            _value = Option<T>.None();
            _factory = factory;
        }

        public static implicit operator T(Lazy<T> lazy)
        {
            return lazy.Value;
        }
    }
}