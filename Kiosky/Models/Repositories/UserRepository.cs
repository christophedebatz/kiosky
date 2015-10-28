using System;
using Kiosky.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using MongoDB.Driver;
using Kiosky.Services.Internal;
using MongoDB.Bson;
using Kiosky.Services.Auth;
using KioskyInterfaces;
using System.Threading.Tasks;
using Kiosky.Models.Repositories;

namespace Kiosky.Services.Repositories
{
    public class UserRepository : RepositoryBase
    {
        protected IMongoCollection<IUser> userCollection;

        /// <summary>
        /// Ctor.
        /// </summary>
        public UserRepository()
        {
            this.userCollection = DatabaseService.Instance.Database.GetCollection<IUser>("user");
        }

        /// <summary>
        /// Get user from username and password.
        /// </summary>
        /// <param name="username">The user name</param>
        /// <param name="password">The associated password to the given username</param>
        /// <returns>The user that has been recognized</returns>
        public IUser GetByUsernameAndPassword(string username, string password)
        {
            var builder = Builders<IUser>.Filter;

            var filter = builder.Eq("Username", username) & 
                         builder.Eq("Password", AuthService.EncryptWithSHA1(password));

            var user = this.userCollection.Find(filter).SingleAsync().Result;

            return user;
        }

        /// <summary>
        /// Create new user and return it with its new id.
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns></returns>
        public IUser CreateUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Id = this.GetNextSequenceId();

            this.userCollection.InsertOneAsync(user);
           
            return user;
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="user">The user to update</param>
        public void UpdateUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var builder = Builders<IUser>.Filter;

            var filter = builder.Eq("_id", user.Id);

            this.userCollection.ReplaceOneAsync(filter, user);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        public IList<IUser> GetUsers()
        {
            var users = new List<IUser>();

            this.userCollection.FindAsync<User>(new BsonDocument()).Result.ForEachAsync<User>(u => users.Add(u));

            return users;
        }

        /// <summary>
        /// Get user from its id.
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        public IUser GetUser(int id)
        {
            var builder = Builders<IUser>.Filter;

            var filter = builder.Eq("_id", id);

            return this.userCollection.Find<IUser>(filter).FirstOrDefaultAsync().Result;
        }

        /// <summary>
        /// Get the next sequence id of the collection.
        /// </summary>
        /// <returns>The next id</returns>
        protected override int GetNextSequenceId()
        {
            int maxId = this.GetUsers().Max(u => u.Id);

            return maxId + 1;
        }
    }
}