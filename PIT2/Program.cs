using Microsoft.AspNetCore.Connections;
using PIT2.Factory;
using PIT2.Service;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Server connection
builder.Services.AddSingleton<IDbConnectionFactory>(new SqlConnectionFactory(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<CupomService>();
builder.Services.AddScoped<PedidoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
