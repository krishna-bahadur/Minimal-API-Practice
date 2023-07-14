using MinimalAPI.Models;

namespace MinimalAPI.DTOs
{
    public class TodoDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        public TodoDTO() { }
        public TodoDTO(Todo todo) =>
            (Id, Name, IsComplete) = (todo.Id, todo.Name, todo.IsComplete);

        // above code is same as this 
        //public TodoDTO(Todo todo)
        //{
        //    Id = todo.Id;
        //    Name = todo.Name;
        //    IsComplete = todo.IsComplete;
        //}
          
    }
}
