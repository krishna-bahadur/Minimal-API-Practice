using Microsoft.EntityFrameworkCore;
using MinimalAPI.DB;
using MinimalAPI.DTOs;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

//configure database in memory by using Microsoft.EntityFrameworkCore.InMemory
builder.Services.AddDbContext<TodoDb>(options => options.UseInMemoryDatabase("TodoList"));

//It adds a developer-friendly error page for database-related exceptions during development. Using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

#region Simple Map methods with OUT MapGroup and Data Transfer Object (DTO)
////Get all todo lists
//app.MapGet("/todoitems", async (TodoDb db) =>
//    await db.Todos.ToListAsync()
//);

//// Get all todo lists which are complete or true
//app.MapGet("/todoitems/complete", async (TodoDb db) =>
//    await db.Todos.Where(t => t.IsComplete).ToListAsync()
//);

////Get todo by Id
//app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
//    await db.Todos.FindAsync(id)
//    is Todo todo 
//    ? Results.Ok(todo)
//    : Results.NotFound()
//);

////Create todo
//app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
//{
//    db.Todos.Add(todo);
//    await db.SaveChangesAsync();
//    return Results.Created($"/todoitems/{todo.Id}", todo);
//});

////Update todo
//app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
//{
//    var todo = await db.Todos.FindAsync(id);
//    if (todo is null) return Results.NotFound();
//    todo.Name = inputTodo.Name;
//    todo.IsComplete = inputTodo.IsComplete;
//    await db.SaveChangesAsync();
//    return Results.NoContent();
//});

////Delete todo by Id
//app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
//{
//    if (await db.Todos.FindAsync(id) is Todo todo)
//    {
//        db.Todos.Remove(todo);
//        await db.SaveChangesAsync();
//        return Results.Ok(todo);
//    }
//    return Results.NotFound();
//});

//app.MapGet("/", () => "Learning Mininal API in .NET Core !");

//app.Run();

#endregion


#region Simple Map methods with MapGroup and Data Transfer Object (DTO)

var todoItems = app.MapGroup("/todoitems");
todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodoById);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);

app.MapGet("/", () => "Learning Mininal API in .NET Core !");

app.Run();


static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Select(x=> new TodoDTO(x)).ToListAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(x => x.IsComplete).Select(x => new TodoDTO(x)).ToListAsync());
}
static async Task<IResult> GetTodoById(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo
        ? TypedResults.Ok(new TodoDTO(todo))
        : TypedResults.NotFound();
}
static async Task<IResult> CreateTodo(TodoDTO todoDTO, TodoDb db)
{
    var todo = new Todo()
    {
        Name = todoDTO.Name,
        IsComplete = todoDTO.IsComplete,
    };
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/todoitems/{todo.Id}", new TodoDTO(todo));
}

static async Task<IResult> UpdateTodo(int id, TodoDTO todoDTO, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return TypedResults.NotFound();

    todo.Name = todoDTO.Name;
    todo.IsComplete = todoDTO.IsComplete;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id,  TodoDb db)
{
    if(await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo); 
        await db.SaveChangesAsync();

        return TypedResults.Ok(new TodoDTO(todo));
    }
    return TypedResults.NotFound();
}




#endregion

