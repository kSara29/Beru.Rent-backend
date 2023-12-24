using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Application;
using User.Infrastructure;
using User.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureService();
builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
}).AddIdentity<User.Domain.Models.User, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 6;
    }).AddEntityFrameworkStores<UserContext>();;
var app = builder.Build();

app.UseFastEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.MapPost("/createUser", 
//     async ([Microsoft.AspNetCore.Mvc.FromBody] CreateUserDto model, IUserService service) =>
//     {
//         var result = model.CreateUserValidate();
//         if (result.Count > 0) return new JsonResult(result);
//         var results = await service.CreateUserAsync(model, model.Password);
//         return new ActionResult<User.Domain.Models.User>(results); 
//     });
//
// app.MapPut("/editUser", 
//     async ([Microsoft.AspNetCore.Mvc.FromBody] UpdateUserDto? model, IUserService service) =>
// {
//     if (model is null) return new BadRequestResult();
//     var validateResult = model.UpdateUserValidate();
//     if (validateResult.Count > 0) return new JsonResult(validateResult);
//     
//     var result = await service.UpdateUserAsync(model);
//     return new ActionResult<User.Domain.Models.User>(result);
// });
//
// app.MapDelete("/deleteUser",
//     async ([Microsoft.AspNetCore.Mvc.FromBody] string userId, IUserService service) =>
//     {
//         if (string.IsNullOrWhiteSpace(userId))
//             return new BadRequestResult();
//
//         var result = await service.DeleteUserAsync(userId);
//         return new ActionResult<UserDto>(result);
//     });
//
// app.MapPost("/getUserById",
//     async ([Microsoft.AspNetCore.Mvc.FromBody] string userId, IUserService service) =>
//     {
//         if (string.IsNullOrWhiteSpace(userId))
//             return new BadRequestResult();
//
//         var result = await service.GetUserByIdAsync(userId);
//         return new ActionResult<UserDto>(result);
//     });
app.Run();

