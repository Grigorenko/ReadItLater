using System;
using System.ComponentModel.DataAnnotations;

namespace ReadItLater.Data.Dtos.Folder
{
    public class CreateFolderDto
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
