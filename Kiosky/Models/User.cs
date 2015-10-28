using KioskyInterfaces;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Kiosky.Models
{
    public class User : IUser
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement]
        [Required]
        public string Username { get; set; }

        [BsonElement]
        [Required]
        public string Password { get; set; }

        [BsonElement]
        [Required]
        public string Email { get; set; }

        [BsonElement]
        public string[] Roles { get; set; }

        [BsonElement]
        public DateTime LastModified { get; set; }

        [BsonElement]
        public DateTime CreatedAt { get; set; }

        public User() { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public User(int id, string username, string password, string[] roles)
        {
            this.Username = username;
            this.Password = password;
            this.Id = id;
            this.Roles = roles == null || roles.Length == 0 ? new[] { "user" } : roles;
        }
    }
}