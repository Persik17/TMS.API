﻿using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class Board : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public bool IsPrivate { get; set; }

        public List<Column> Columns { get; set; } = [];
        public List<TaskType> TaskTypes { get; set; } = [];

        public Board() { }
    }
}
