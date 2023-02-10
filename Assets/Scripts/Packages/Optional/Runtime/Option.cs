using System;
using System.Collections.Generic;
using UnityEngine;

namespace Optional
{
    [Serializable]
    public struct Option<T>
    {
        [SerializeField] private T _value;
        [SerializeField] private bool _isSome;

        public static Option<T> None() => default;
        public static Option<T> Some(T value) => !EqualityComparer<T>.Default.Equals(value, default) ? new Option<T>(value) : throw new NoNullAllowedException(); // Null checks break with generics

        public static Option<T> Wrap(T value) => value != null ? Some(value) : None();

        public Option(T value)
        {
            _value = value;
            _isSome = true;
        }

        public readonly bool IsSome() => _isSome;
        public readonly bool IsNone() => !_isSome;

        public T Unwrap()
        {
            if (IsNone()) throw new UnwrapException();
            return _value;
        }

        public T Expect(string message)
        {
            if (IsNone()) throw new UnwrapException(message);
            return _value;
        }

        public override string ToString() => this.Match(value => $"Some({value.ToString()})", () => "None");
    }

    public static class OptionExtensions
    {
        public static U Match<T, U>(this Option<T> option, Func<T, U> onSome, Func<U> onNone) =>
            option.IsSome() ? onSome(option.Unwrap()) : onNone();
        public static Option<T> Match<T>(this Option<T> option, Action<T> onSome, Action onNone)
        {
            if (option.IsSome()) onSome(option.Unwrap());
            else onNone();
            return option;
        }

        public static Option<U> Bind<T, U>(this Option<T> option, Func<T, Option<U>> onSome) =>
            option.Match(onSome, Option<U>.None);

        public static Option<U> Map<T, U>(this Option<T> option, Func<T, U> f) =>
            option.Bind(value => Option<U>.Wrap(f(value)));
        
        public static Option<T> Apply<T>(this Option<T> option, Action<T> onSome)
        {
            option.Match(onSome, () => { });
            return option;
        }

        public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate) =>
            option.Bind(value => predicate(value) ? option : Option<T>.None());

        public static T UnwrapOr<T>(this Option<T> option, T defaultValue)
        {
            if (option.IsNone()) return defaultValue;
            return option.Unwrap();
        }

        public static T UnwrapOrElse<T>(this Option<T> option, Func<T> generator)
        {
            if (option.IsNone()) return generator();
            return option.Unwrap();
        }

        public static Option<T> Or<T>(this Option<T> option, Option<T> or) => option.IsSome() ? option : or;
        public static Option<T> OrElse<T>(this Option<T> option, Func<Option<T>> generator) => option.IsSome() ? option : generator();
        public static Option<T> OrWrap<T>(this Option<T> option, T or) => option.IsSome() ? option : Option<T>.Wrap(or);
        public static Option<T> OrWrapElse<T>(this Option<T> option, Func<T> generator) => option.IsSome() ? option : Option<T>.Wrap(generator());

        public static Option<V> GetValue<K, V>(this Dictionary<K, V> dictionary, K key)
        {
            return dictionary.TryGetValue(key, out V value) ? Option<V>.Some(value) : Option<V>.None();
        }
    }

    public class UnwrapException : Exception
    {
        public UnwrapException()
        {
        }

        public UnwrapException(string message) : base(message)
        {
        }
    }

    public class NoNullAllowedException : Exception {}
}
