﻿using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class BookISBNVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ISBN { get; set; }
        public string? UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public bool isIssue { get; set; } = false;
    }
}
