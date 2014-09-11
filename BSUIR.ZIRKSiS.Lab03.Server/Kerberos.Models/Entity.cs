using System;

namespace Kerberos.Models
{
    public abstract class Entity<TKey> : EntityBase, IEquatable<Entity<TKey>>
    {
        private TKey _key;

        protected Entity()
        {

        }

        protected Entity(TKey key)
        {
            this._key = key;
        }

        public TKey Key
        {
            get { return this._key; }
            protected set { this._key = value; }
        }

        public bool Equals(Entity<TKey> other)
        {
            if (other == null)
                return false;
            return this.Key.Equals(other.Key);
        }

        public override bool Equals(object right)
        {
            if (object.ReferenceEquals(right, null))
            {
                return false;
            }
            if (object.ReferenceEquals(this, right))
            {
                return true;
            }
            if (this.GetType() != right.GetType())
                return false;

            return this.Equals(right as Entity<TKey>);
        }

        public override int GetHashCode()
        {
            return this._key.GetHashCode();
        }
    }
}
