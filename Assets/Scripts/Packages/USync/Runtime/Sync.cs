using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Optional;
using UnityEngine;

namespace USync
{
    [Serializable]
    public class Sync<T>
    {
        [SerializeField] private List<SyncListener<T>> _listeners = new();

        [SerializeField] private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    UpdateListeners();
                }
            }
        }
        
        public Sync() {}
        public Sync(T value)
        {
            Value = value;
        }

        private void UpdateListeners()
        {
            foreach (var listener in  _listeners)
            {
                if (listener.setter.IsNone()) BuildSetter(listener);
                listener.setter.Apply(x => x.DynamicInvoke(listener.target, Value));
            }
        }

        private void BuildSetter(SyncListener<T> listener)
        {
            listener.setter = Option<Component>.Wrap(listener.target)
                .Map(target => target.GetType())
                .Bind(type =>
                    Option<FieldInfo>.Wrap(type.GetField(listener.fieldPath,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                        .Map(field => (type, field))
                )
                .Map(data => BuildDelegate(data.type, data.field));
        }

        private Delegate BuildDelegate(Type type, FieldInfo field)
        {
            ParameterExpression paramExpression = Expression.Parameter(type, "target");

            MemberExpression fieldExpression = Expression.Field(paramExpression, field.Name);

            ParameterExpression valueExpression = Expression.Parameter(field.FieldType, field.Name);

            BinaryExpression operationExpression = Expression.Assign(fieldExpression, valueExpression);

            LambdaExpression lambdaExpression = Expression.Lambda(typeof(Action<,>).MakeGenericType(type, field.FieldType), operationExpression, paramExpression, valueExpression);

            return lambdaExpression.Compile();
        }
    }

    [Serializable]
    public class SyncListener<T> // Type argument used to determined value fields in propertydrawer
    {
        [SerializeField] public string fieldPath;
        [SerializeField] public Component target;

        [NonSerialized] public Option<Delegate> setter;
    }
}