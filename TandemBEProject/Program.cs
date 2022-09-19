using AutoMapper;
using TandemBEProject.DAL;
using TandemBEProject.DAL.Cosmos;
using TandemBEProject.DTOs;
using TandemBEProject.Models;
using TandemBEProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

MapperConfiguration mapperConfig = new(mc =>
{
    mc.CreateMap<CreateUserRequestDto, UserModel>();
    mc.CreateMap<UserModel, UserResponseDto>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton<IDbService, CosmosDbService>();
builder.Services.AddScoped<UsersService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
