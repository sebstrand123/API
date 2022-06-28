using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Context;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Books") ?? "Data Source=Books.db";
builder.Services.AddSqlite<BookDb>(connectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/", () => "Välkommen till mitt API!");

app.MapGet("/books", async (BookDb db) =>
    await db.Books.ToListAsync());

app.MapGet("/books/{id}", async (int id, BookDb db) =>
    await db.Books.FindAsync(id)
        is Book book
            ? Results.Ok(book)
            : Results.NotFound());

app.MapPost("books", async (Book book, BookDb db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{book.Id}", book);
});

app.MapPut("/books/{id}", async (int id, Book inputBook, BookDb db) =>
{
    var book = await db.Books.FindAsync(id);

    if(book is null) return Results.NotFound();

    book.Name = inputBook.Name;
    book.InStore = inputBook.InStore;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/books/{id}", async (int id, BookDb db) =>
{
    if(await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.Ok(book);
    }
    return Results.NotFound();
});

app.Run();