using AutoMapper;
using Mesagger.BLL.Security.Interface;
using Mesagger.BLL.Security.Realizations;
using Messenger.BLL.Mapping;
using Messenger.BLL.Mapping.PersonalChat;
using Messenger.DAL.Persistence;
using Messenger.DAL.Repositories.Interfaces.Base;
using Messenger.DAL.Repositories.Realizations.Base;
using Messenger.WebAPI.Extensions;
using Messenger.WebAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<MessengerDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("Messenger")));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:3001");
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(currentAssemblies);
});

builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(ProfileProfile));
builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting(); 
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Run();
