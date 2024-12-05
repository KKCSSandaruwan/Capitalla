﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.Navigation;
using QuickAccounting.Repository.Interface.Navigation;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.Navigation
{
    public class SubMenuService : ISubMenu
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;

        #region Constructor
        // Constructor to initialize the service with the database context and auth
        public SubMenuService(ApplicationDbContext context, AuthenticationStateProvider authState)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
                _authState = authState;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Sub Menu Service: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fetch Methods
        // Fetches a list of all sub menus.
        public async Task<List<SubMenu>> GetAllAsync()
        {
            try
            {
                var result = await (from sm in _context.SubMenu
                                    orderby sm.SubMenuName ascending
                                    select sm).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active sub menus.
        public async Task<List<SubMenu>> GetActiveAsync()
        {
            try
            {
                var result = await (from sm in _context.SubMenu
                                    where sm.Active == true
                                    orderby sm.SubMenuName ascending
                                    select sm).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active sub menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of SubMenus that are not assigned to any NavigationMenu.
        public async Task<List<SubMenu>> GetUnassignedAsync()
        {
            try
            {
                // Fetches a list of SubMenuIds that are already assigned to NavigationMenus.
                var assignedSubMenuIds = await (from nm in _context.NavigationMenu
                                                select nm.SubMenuId).Distinct().ToListAsync();

                // Fetches a list of unassigned SubMenus by checking for IDs not in the assigned SubMenuIds list.
                var result = await (from sm in _context.SubMenu
                                    where !assignedSubMenuIds.Contains(sm.SubMenuId)
                                    select sm).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching unassigned sub menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a object of sub menu for a specified sub menu id.
        public async Task<SubMenu> GetByIdAsync(int subMenuId)
        {
            try
            {
                var result = await (from sm in _context.SubMenu
                                    where sm.SubMenuId == subMenuId
                                    select sm).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"Sub menu with ID {subMenuId} was not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub menu with ID {subMenuId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Methods
        // Creates a new sub menu or updates an existing one based on SubMenuId.
        public async Task<SubMenu> UpsertAsync(SubMenu subMenu)
        {
            try
            {
                if (subMenu == null)
                    throw new ArgumentNullException(nameof(subMenu), "Sub menu cannot be null.");

                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                subMenu.SubMenuName = subMenu.SubMenuName.Trim();
                subMenu.Code = subMenu.Code?.Trim().ToUpper();
                subMenu.Url = subMenu.Url.Trim();
                subMenu.IconName = subMenu.IconName?.Trim();
                subMenu.Description = subMenu.Description?.Trim();
                subMenu.CreatedBy = userName;
                subMenu.CreatedDate = subMenu.CreatedDate == default ? DateTime.Now : subMenu.CreatedDate;

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(subMenu);
                if (!Validator.TryValidateObject(subMenu, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                var existingSubMenu = await _context.SubMenu.FirstOrDefaultAsync(sm => sm.SubMenuId == subMenu.SubMenuId);

                if (existingSubMenu != null)
                {
                    existingSubMenu.SubMenuName = subMenu.SubMenuName;
                    existingSubMenu.Code = subMenu.Code;
                    existingSubMenu.Url = subMenu.Url;
                    existingSubMenu.IconName = subMenu.IconName;
                    existingSubMenu.Description = subMenu.Description;
                    existingSubMenu.ModifiedBy = userName;
                    existingSubMenu.ModifiedDate = DateTime.Now;
                    existingSubMenu.Active = subMenu.Active;

                    _context.SubMenu.Update(existingSubMenu);
                }
                else
                {
                    _context.SubMenu.Add(subMenu);
                }

                await _context.SaveChangesAsync();
                return subMenu;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the sub menu: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a sub menu by its ID.
        public async Task<SubMenu> ToggleActiveAsync(int subMenuId)
        {
            try
            {
                var subMenu = await _context.SubMenu.FirstOrDefaultAsync(sm => sm.SubMenuId == subMenuId);

                if (subMenu == null)
                    throw new KeyNotFoundException($"Sub menu with ID {subMenuId} was not found.");

                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                subMenu.Active = !subMenu.Active;
                subMenu.ModifiedBy = userName;
                subMenu.ModifiedDate = DateTime.Now;

                _context.SubMenu.Update(subMenu);
                await _context.SaveChangesAsync();

                return subMenu;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling Active status for sub menu with ID {subMenuId}: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
