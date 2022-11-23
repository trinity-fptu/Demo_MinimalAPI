using Demo_MinimalAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Demo_MinimalAPIDbContext>(opt
    => opt.UseInMemoryDatabase("Demo_MinimalAPI"));

var app = builder.Build();

app.MapGet("/shoppinglist", async (Demo_MinimalAPIDbContext db) =>
{
    await db.Groceries.ToListAsync();
});

app.MapGet("/shoppinglist/{id}", async (Demo_MinimalAPIDbContext db, int id) =>
{
    var grocery = db.Groceries.FindAsync(id);
    return grocery != null ? Results.Ok(grocery) : Results.NotFound();
});

app.MapPost("/shoppinglist", async (Grocery grocery, Demo_MinimalAPIDbContext db) =>
{
    db.Groceries.Add(grocery);
    await db.SaveChangesAsync();
    return Results.Created($"/shoppinglist/{grocery.Id}", grocery);
});

app.MapDelete("/shoppinglist/{id}", async (Demo_MinimalAPIDbContext db, int id) =>
{
    var grocery = await db.Groceries.FindAsync(id);
    if (grocery != null)
    {
        db.Groceries.Remove(grocery);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapPut("/shoppinglist/{id}", async (Grocery grocery, Demo_MinimalAPIDbContext db, int id) =>
{
    var find = await db.Groceries.FindAsync(id);
    if (find != null)
    {
        find.Name = grocery.Name;
        find.Purchased = grocery.Purchased;
        await db.SaveChangesAsync();
        return Results.Ok(find);
    }
    return Results.NotFound();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();