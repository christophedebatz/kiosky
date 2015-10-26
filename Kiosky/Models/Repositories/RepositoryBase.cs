using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiosky.Models.Repositories
{
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Get the next sequence id of the collection.
        /// </summary>
        /// <returns>The next id</returns>
        protected abstract int GetNextSequenceId();
    }
}