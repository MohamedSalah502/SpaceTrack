using SpaceTrack.DAL.Model;
namespace SpaceTrack.Services
{
    public interface IUserService
    {
        // Register a new user
        Task<User> Register(User user);

        // User login
        Task<User> Login(string name, string password);

        // Get all users
        Task<List<User>> GetAllUsers();

        // Add a new user
        Task AddUser(User user);

        // Update an existing user
        Task UpdateUser(User user);

        // Delete a user
        Task DeleteUser(string userId);
        //Task<List<User>> GetAllUsersAsync();           // Retrieve all users
        //Task<User> GetUserByIdAsync(string id);        // Retrieve a user by ID
        //Task<User> GetUserByEmailAsync(string email);  // Retrieve a user by email
        //Task<bool> AddUserAsync(User user);            // Add a new user
        //Task<bool> UpdateUserAsync(string id, User user); // Update user by ID
        //Task<bool> DeleteUserAsync(string id);         // Delete user by ID
        //Task<User?> AuthenticateAsync(string name, string password); // Authenticate user
    }
}
