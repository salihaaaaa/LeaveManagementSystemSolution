using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Infrastructure.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.Core.Services;
using LeaveManagementSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILeaveTypesRepository, LeaveTypesRepository>();
builder.Services.AddScoped<ILeavesRepository, LeavesRepository>();

builder.Services.AddScoped<ILeaveTypesAdderService, LeaveTypesAdderService>();
builder.Services.AddScoped<ILeaveTypesGetterService, LeaveTypesGetterService>();
builder.Services.AddScoped<ILeaveTypesUpdaterService, LeaveTypesUpdaterService>();
builder.Services.AddScoped<ILeaveTypesDeleterService, LeaveTypesDeleterService>();
builder.Services.AddScoped<ILeavesAdderService, LeavesAdderService>();
builder.Services.AddScoped<ILeavesGetterService, LeavesGetterService>();
builder.Services.AddScoped<ILeavesUpdaterService, LeavesUpdaterService>();
builder.Services.AddScoped<ILeavesDeleterService, LeavesDeleterService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Enable Identity in this project
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredUniqueChars = 5; //Non repeated character
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

//Enforce authorization policy (user must be authenticated)  for all the action methods
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser().Build();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting(); //Identifying action method based route
app.UseAuthentication(); //Reading Identity cookie (login or not)
app.UseAuthorization(); //Validate access permission of the user
app.MapControllers();

app.Run();
