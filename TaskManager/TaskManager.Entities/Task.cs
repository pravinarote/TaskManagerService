using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Entities
{
    [Table("tblTask")]
    public class Task
    {
        [Key]
        [Required]
        public int TaskId { get; set; }

        [StringLength(50)]
        public string TaskName { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime EndDate { get; set; }
        [ForeignKey("TaskId")]
        public Task ParentTask { get; set; }
        [ForeignKey("ParentTask")]
        public int? ParentTaskId { get; set; }

        public bool IsTaskEnded { get; set; }

    }
}
