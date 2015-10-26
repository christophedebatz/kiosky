using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiosky.Services.Internal
{
    public class Paginator<T>
    {
        private readonly IQueryable<T> _collection;

        private readonly int _offset;

        private readonly int _limit;

        private readonly int _size;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public Paginator(IList<T> collection, int offset, int limit)
        {
            this._collection = collection.AsQueryable();
            this._size = collection.Count();
            this._offset = offset;
            this._limit = limit;
        }

        public IQueryable<T> Items
        {
            get
            {
                return this._collection == null ? null : this._collection.Skip(this._offset).Take(this._limit);
            }
        }

        public Link First
        {
            get
            {
                return new Link(0, this._limit);
            }
        }

        public Link Previous
        {
            get
            {
                int previousOffset = 0;

                if (this._offset - this._limit > 0)
                {
                    previousOffset = this._offset - this._limit;
                }

                return new Link(previousOffset, this._limit);
            }
        }

        public Link Next
        {
            get
            {
                int nextOffset = this._offset;

                if (nextOffset + this._limit < this._size)
                {
                    nextOffset = this._offset + this._limit;
                }

                return new Link(nextOffset, this._limit);
            }
        }

        public Link Last
        {
            get
            {
                int lastOffset = this._offset + this._limit;

                while (lastOffset < this._size)
                {
                    lastOffset += this._offset + this._limit;
                }

                return new Link(lastOffset, this._limit);
            }
        }
    }

    public class Link
    {
        public int Offset { get; set; }

        public int Limit { get; set; }

        public Link(int offset, int limit)
        {
            this.Offset = offset;
            this.Limit = limit;
        }
    }
}