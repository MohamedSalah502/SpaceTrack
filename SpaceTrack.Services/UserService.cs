using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SpaceTrack.DAL;
using SpaceTrack.DAL.Model;
//using SpaceTrack.DAL.Models;

namespace SpaceTrack.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDbContext _context;

        public UserService(MongoDbContext context)
        {
            _context = context;
        }

        // Register a new user
           public async Task<User> Register(User userInput)
        {
            // Check if user with same email exists
            var existingUser = await _context.Users.Find(u => u.Email == userInput.Email).FirstOrDefaultAsync();
            if (existingUser != null)
                return null; // User already exists
            // Create a new User object with only the required fields
            var newUser = new User
            {
                Name = userInput.Name,
                Email = userInput.Email,
                Password = userInput.Password, // You might want to hash the password here
                Role = "user", // Default role assignment
                SatelliteNames = new List<string>(), // Initialize empty list
                LastLoginTime = DateTime.UtcNow // Set default login time
            };

            // Insert the new user into the database
            await _context.Users.InsertOneAsync(newUser);

            return newUser;
        }
        public async Task<User> Login(string name, string password)
        {

            var filter = Builders<User>.Filter.Eq(u => u.Name, name);
            var user = await _context.Users.Find(filter).FirstOrDefaultAsync();

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }
            return null;

        }

        // Get all users
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.Find(_ => true).ToListAsync();
        }

        // Add a new user
        public async Task AddUser(User user)
        {
            await _context.Users.InsertOneAsync(user);
        }

        // Update an existing user
        public async Task UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            await _context.Users.ReplaceOneAsync(filter, user);
        }

        public async Task DeleteUser(string userId)
        {
            await _context.Users.DeleteOneAsync(u => u.Id == userId);
        }
    }
}
