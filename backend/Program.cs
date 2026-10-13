using System.Text;using Microsoft.AspNetCore.Authentication.JwtBearer;using Microsoft.EntityFrameworkCore;using Microsoft.IdentityModel.Tokens;using EMedicine.Api.Data;using EMedicine.Api.Models;using Microsoft.AspNetCore.Identity;
var builder=WebApplication.CreateBuilder(args);builder.Services.AddControllers();builder.Services.AddEndpointsApiExplorer();builder.Services.AddSwaggerGen();builder.Services.AddDbContext<AppDbContext>(o=>o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));builder.Services.AddScoped<IPasswordHasher<AppUser>,PasswordHasher<AppUser>>();var key=builder.Configuration["Jwt:Key"]??throw new Exception("JWT key missing");if(key.Length<32)throw new Exception("JWT key must be at least 32 characters");builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o=>o.TokenValidationParameters=new(){ValidateIssuer=true,ValidIssuer=builder.Configuration["Jwt:Issuer"],ValidateAudience=true,ValidAudience=builder.Configuration["Jwt:Audience"],ValidateLifetime=true,ValidateIssuerSigningKey=true,IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))});builder.Services.AddAuthorization();builder.Services.AddCors(o=>o.AddPolicy("frontend",p=>p.WithOrigins("http://localhost:5173","http://localhost:3000").AllowAnyHeader().AllowAnyMethod()));var app=builder.Build();app.UseSwagger();app.UseSwaggerUI();app.UseCors("frontend");app.UseAuthentication();app.UseAuthorization();app.MapControllers();
// Optional first-run admin: set EMED_ADMIN_EMAIL and EMED_ADMIN_PASSWORD as environment variables.
using(var scope=app.Services.CreateScope()){
 var email=Environment.GetEnvironmentVariable("EMED_ADMIN_EMAIL");var password=Environment.GetEnvironmentVariable("EMED_ADMIN_PASSWORD");
 if(!string.IsNullOrWhiteSpace(email)&&!string.IsNullOrWhiteSpace(password)&&password.Length>=12){
  var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
  if(db.Database.CanConnect()&&!db.Users.Any(x=>x.Email==email.ToLowerInvariant())){
   var admin=new AppUser{Email=email.ToLowerInvariant(),FirstName="Admin",LastName="User",Role="Admin"};
   admin.PasswordHash=scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>().HashPassword(admin,password);
   db.Users.Add(admin);db.SaveChanges();
  }
 }
}
app.Run();
