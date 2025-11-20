using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        private TaskItem? _selectedTask;
        public TaskItem? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            _taskService = new TaskService();

            LoadCommand = new RelayCommand(async _ => await LoadAsync());
            AddCommand = new RelayCommand(_ => AddTask());
            SaveCommand = new RelayCommand(async _ => await SaveTaskAsync(), _ => SelectedTask != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteTaskAsync(), _ => SelectedTask != null);

            
            _ = LoadAsync();
        }

        public async Task LoadAsync()
        {
            Tasks.Clear();
            var items = await _taskService.GetAllAsync();
            foreach (var item in items)
                Tasks.Add(item);

            if (Tasks.Any())
                SelectedTask = Tasks.First();
        }

        private void AddTask()
        {
            var newTask = new TaskItem
            {
                Title = "Новая задача",
                DueDate = DateTime.Now.Date.AddDays(1),
                Priority = 3
            };

            Tasks.Add(newTask);
            SelectedTask = newTask;
        }

        private async Task SaveTaskAsync()
        {
            if (SelectedTask == null)
                return;

         
            var context = new ValidationContext(SelectedTask);
            var results = new System.Collections.Generic.List<ValidationResult>();

            if (!Validator.TryValidateObject(SelectedTask, context, results, true))
            {
                var msg = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                MessageBox.Show(msg, "Ошибки валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedTask.Id == 0)
                await _taskService.AddAsync(SelectedTask);
            else
                await _taskService.UpdateAsync(SelectedTask);

            await LoadAsync();
        }

        private async Task DeleteTaskAsync()
        {
            if (SelectedTask == null)
                return;

            if (SelectedTask.Id == 0)
            {
                
                Tasks.Remove(SelectedTask);
                return;
            }

            if (MessageBox.Show("Удалить задачу?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _taskService.DeleteAsync(SelectedTask);
                await LoadAsync();
            }
        }
    }
}
