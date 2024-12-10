﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting.Navigation
{
    public class MainMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MainMenuId { get; set; }

        [Required(ErrorMessage = "Main menu name is required.")]
        [StringLength(100, ErrorMessage = "Main menu name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Main menu name can only contain alphabetic characters and spaces.")]
        public string MainMenuName { get; set; }

        [StringLength(15, ErrorMessage = "Code cannot exceed 15 characters.")]
        [RegularExpression(@"^[A-Za-z0-9-]+$", ErrorMessage = "Code must only contain letters, numbers, and hyphens (no spaces).")]
        public string? Code { get; set; }

        [StringLength(255, ErrorMessage = "URL cannot exceed 255 characters.")]
        [RegularExpression(@"^\/[a-zA-Z0-9\-\/]*$", ErrorMessage = "Router path must start with a (/) and only contain letters, numbers, hyphens, and slashes.")]
        public string? Url { get; set; }

        [StringLength(50, ErrorMessage = "Icon name cannot exceed 50 characters.")]
        public string? IconName { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Created by is required.")]
        [StringLength(50, ErrorMessage = "Created by name cannot exceed 50 characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Modified by name cannot exceed 50 characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool Active { get; set; } = true;
    }
}
