﻿namespace Tapir.Core.Domain
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public static bool operator ==(ValueObject? a, ValueObject? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject? a, ValueObject? b)
        {
            return !(a == b);
        }

        public virtual bool Equals(ValueObject? other)
        {
            return other is not null && ValuesAreEqual(other);
        }

        public override bool Equals(object? obj)
        {
            return obj is ValueObject valueObject && ValuesAreEqual(valueObject);
        }

        public override int GetHashCode()
        {
            return GetAtomicValues().Aggregate(default(int), (hashcode, value) => HashCode.Combine(hashcode, value.GetHashCode()));
        }

        private bool ValuesAreEqual(ValueObject valueObject)
        {
            return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
        }

        protected abstract IEnumerable<object> GetAtomicValues();
    }
}
