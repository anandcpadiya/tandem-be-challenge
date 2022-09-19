using AutoMapper;
using TandemBEProject;
using TandemBEProject.DAL;
using TandemBEProject.DTOs;
using TandemBEProject.Models;
using TandemBEProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Configured AutoMapper
MapperConfiguration mapperConfig = new(mc =>
{
    mc.CreateMap<CreateUserRequestDto, UserModel>();
    mc.CreateMap<UserModel, UserResponseDto>();
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


// Configured Cosmos DB
builder.Services.AddSingleton<IDbService>(
    CosmosHelper.InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"))
        .GetAwaiter()
        .GetResult()
);

// Configured Services
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
