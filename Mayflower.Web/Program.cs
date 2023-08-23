using Mayflower.Web.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Mayflower.Web;
using Mayflower.Core.Infrastructure.Helpers;
using LazyCache;
using Mayflower.Core.Infrastructure.Queries;
using Mayflower.Core.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();

builder.Services.AddMayflowerData(configuration);
builder.Services.AddInfrastructure(configuration);

// Add custom data services
builder.Services.AddSingleton<TransactionService>();

builder.Services.AddOptions().Configure<ApplicationOptions>(configuration.GetSection(nameof(ApplicationOptions)));

if(!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseStaticWebAssets();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
