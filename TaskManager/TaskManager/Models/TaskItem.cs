using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; } // Идентификатор в БД

        [Required(ErrorMessage = "Название задачи обязательно")]
        [MaxLength(100, ErrorMessage = "Максимум 100 символов")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Максимум 500 символов для описания")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Нужно указать дедлайн")]
        public DateTime DueDate { get; set; } = DateTime.Now.Date;

        public bool IsCompleted { get; set; }

        [Range(1, 5, ErrorMessage = "Приоритет должен быть от 1 до 5")]
        public int Priority { get; set; } = 3;
    }
}
