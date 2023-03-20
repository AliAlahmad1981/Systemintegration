using AccountService.Data;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AccountDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AccountDBContext>();
    db.Database.Migrate();
}

app.MapPost("/register", async (User user, AccountDBContext db) =>
{
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Created("/login", "User registered !");
});

app.MapPost("/login", async (UserLogin userLogin, AccountDBContext db) =>
{
    User? user = await db.Users.FirstOrDefaultAsync(user => user.EmailAddress.Equals(userLogin.EmailAddress) && user.Password.Equals(userLogin.Password));
    if (user == null)
    {
        return Results.NotFound("Invalid login credentials !");
    }
    var key = builder.Configuration["Jwt:Key"];
    if (key == null)
    {
        return Results.StatusCode(500);
    }
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.EmailAddress),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Role, user.Role)
    };
    var jwt = new JwtSecurityToken
    (
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
    );
    var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);
    return Results.Ok(jwtString);
});

app.Run();
