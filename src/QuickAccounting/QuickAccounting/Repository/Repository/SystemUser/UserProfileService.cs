using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.SystemUser;
using QuickAccounting.Data.ViewModel.SystemUser;
using QuickAccounting.Repository.Interface.Security;
using QuickAccounting.Repository.Interface.SystemUser;
using QuickAccounting.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.SystemUser
{
    public class UserProfileService : IUserProfile
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;
        private readonly IEncryption _encryption;

        #region Constructor
        // Constructor to initialize the service with the database context and auth
        public UserProfileService(ApplicationDbContext dbContext, AuthenticationStateProvider authState, IEncryption encryptionService)
        {
            try
            {
                _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
                _authState = authState;
                _encryption = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService), "Encryption service cannot be null.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing UserProfile Service: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fetch Methods
        // Fetches a list of all user profiles, ordered by user ID in ascending order.
        public async Task<List<UserProfileView>> GetAllAsync()
        {
            try
            {
                var result = await (from up in _context.UserProfile
                                    join c in _context.CompanyDup on up.CompanyId equals c.CompanyId into cGroup
                                    from c in cGroup.DefaultIfEmpty()
                                    join ur in _context.UserRole on up.UserRoleId equals ur.UserRoleId into urGroup
                                    from ur in urGroup.DefaultIfEmpty()
                                    orderby up.UserId ascending
                                    select new UserProfileView
                                    {
                                        UserId = up.UserId,
                                        CompanyId = up.CompanyId,
                                        CompanyName = c != null ? c.CompanyName : "N/A",
                                        UserRoleId = up.UserRoleId,
                                        UserRoleName = ur != null ? ur.UserRoleName : "N/A",
                                        FirstName = up.FirstName,
                                        LastName = up.LastName,
                                        UserName = up.UserName,
                                        Email = up.Email,
                                        PhoneNumber = up.PhoneNumber,
                                        Verified = up.Verified,
                                        Blocked = up.Blocked,
                                        LastLoginDate = up.LastLoginDate,
                                        CreatedBy = up.CreatedBy,
                                        CreatedDate = up.CreatedDate,
                                        ModifiedBy = up.ModifiedBy,
                                        ModifiedDate = up.ModifiedDate,
                                        Active = up.Active,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user profiles: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active user profiles, ordered by user name in ascending order.
        public async Task<List<UserProfile>> GetActiveAsync()
        {
            try
            {
                var result = await (from up in _context.UserProfile
                                    where up.Active == true
                                    orderby up.UserName ascending
                                    select up
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving active user profiles: {ex.Message}");
                throw;
            }
        }

        // Fetches a single user profile by its ID.
        public async Task<UserProfile> GetByIdAsync(int userId)
        {
            try
            {
                var result = await (from up in _context.UserProfile
                                    where up.UserId == userId
                                    select new UserProfile
                                    {
                                        UserId = up.UserId,
                                        CompanyId = up.CompanyId,
                                        UserRoleId = up.UserRoleId,
                                        FirstName = up.FirstName,
                                        LastName = up.LastName,
                                        UserName = up.UserName,
                                        PlainPassword = _encryption.Decrypt(up.EncryptedPassword),
                                        EncryptedPassword = up.EncryptedPassword,
                                        Email = up.Email,
                                        PhoneNumber = up.PhoneNumber,
                                        AvatarPath = up.AvatarPath,
                                        Verified = up.Verified,
                                        Blocked = up.Blocked,
                                        LastLoginDate = up.LastLoginDate,
                                        CreatedBy = up.CreatedBy,
                                        CreatedDate = up.CreatedDate,
                                        ModifiedBy = up.ModifiedBy,
                                        ModifiedDate = up.ModifiedDate,
                                        Active = up.Active,
                                    }).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"User profile with ID {userId} was not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user profile with ID {userId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Methods
        // Creates or updates a user profile based on user id.
        public async Task<UserProfile> UpsertAsync(UserProfile userProfile)
        {
            try
            {
                // Null check
                if (userProfile == null)
                    throw new ArgumentNullException(nameof(userProfile), "User profile cannot be null.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Trim and standardize inputs
                userProfile.FirstName = StringFormatter.ToTitleCase(userProfile.FirstName?.Trim());
                userProfile.LastName = StringFormatter.ToTitleCase(userProfile.LastName?.Trim());
                userProfile.UserName = userProfile.UserName.Trim().ToLower();
                userProfile.EncryptedPassword = _encryption.Encrypt(userProfile.PlainPassword.Trim());
                userProfile.Email = userProfile.Email?.Trim().ToLower();
                userProfile.PhoneNumber = userProfile.PhoneNumber?.Trim();
                userProfile.CreatedBy = userName;
                userProfile.CreatedDate = userProfile.CreatedDate == default ? DateTime.Now : userProfile.CreatedDate;

                // Validate the UserProfile object
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(userProfile);
                if (!Validator.TryValidateObject(userProfile, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                // Check if the email or username already exists
                var existingProfile = await _context.UserProfile
                    .FirstOrDefaultAsync(up => (up.Email == userProfile.Email || up.UserName == userProfile.UserName) && up.UserId != userProfile.UserId);
                if (existingProfile != null)
                {
                    // Check if the email already exists
                    if (existingProfile.Email == userProfile.Email)
                        throw new ValidationException($"A user profile with the email '{userProfile.Email}' already exists. Please choose a different email.");

                    // Check if the username already exists
                    if (existingProfile.UserName == userProfile.UserName)
                        throw new ValidationException($"A user profile with the username '{userProfile.UserName}' already exists. Please choose a different username.");
                }

                // Check if the user profile already exists
                var existingUserProfile = await _context.UserProfile.FirstOrDefaultAsync(up => up.UserId == userProfile.UserId);
                if (existingUserProfile != null)
                {
                    // Update existing record
                    existingUserProfile.UserRoleId = userProfile.UserRoleId;
                    existingUserProfile.CompanyId = userProfile.CompanyId;
                    existingUserProfile.FirstName = userProfile.FirstName;
                    existingUserProfile.LastName = userProfile.LastName;
                    existingUserProfile.EncryptedPassword = userProfile.EncryptedPassword;
                    existingUserProfile.Email = userProfile.Email;
                    existingUserProfile.PhoneNumber = userProfile.PhoneNumber;
                    existingUserProfile.AvatarPath = userProfile.AvatarPath;
                    existingUserProfile.Verified = userProfile.Verified;
                    existingUserProfile.Blocked = userProfile.Blocked;
                    existingUserProfile.ModifiedBy = userName;
                    existingUserProfile.ModifiedDate = DateTime.Now;
                    existingUserProfile.Active = userProfile.Active;

                    _context.UserProfile.Update(existingUserProfile);
                }
                else
                {
                    // Add new record
                    _context.UserProfile.Add(userProfile);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return userProfile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the user profile: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a user profile by its ID.
        public async Task<UserProfile> ToggleActiveAsync(int userId)
        {
            try
            {
                // Fetch the user profile by ID
                var userProfile = await _context.UserProfile.FirstOrDefaultAsync(up => up.UserId == userId);

                // Null check
                if (userProfile == null)
                    throw new KeyNotFoundException($"User profile with ID {userId} was not found.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Toggle the active status
                userProfile.Active = !userProfile.Active;
                userProfile.ModifiedBy = userName;
                userProfile.ModifiedDate = DateTime.Now;

                // Update the user profile in the database
                _context.UserProfile.Update(userProfile);
                await _context.SaveChangesAsync();

                return userProfile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling Active status for user profile with ID {userId}: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
