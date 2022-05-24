using LojaAPI.Endpoints.Categorias;
using LojaAPI.Endpoints.Funcionarios;
using LojaAPI.Endpoints.Produtos;
using LojaAPI.Endpoints.Segurança;
using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { //diminuindo a segurança da senha
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))
    };
});
builder.Services.AddScoped<QueryAllUsersWithClaims>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthorization();
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//criando endpoints
app.MapMethods(CategoriaPost.Template, CategoriaPost.Methods, CategoriaPost.Handle);
app.MapMethods(CategoriaGetAll.Template, CategoriaGetAll.Methods, CategoriaGetAll.Handle);
app.MapMethods(CategoriaPut.Template, CategoriaPut.Methods, CategoriaPut.Handle);
app.MapMethods(CategoriaDelete.Template, CategoriaDelete.Methods, CategoriaDelete.Handle);

app.MapMethods(ProdutoPost.Template, ProdutoPost.Methods, ProdutoPost.Handle);
app.MapMethods(ProdutoGetAll.Template, ProdutoGetAll.Methods, ProdutoGetAll.Handle);
app.MapMethods(ProdutoPut.Template, ProdutoPut.Methods, ProdutoPut.Handle);
app.MapMethods(ProdutoDelete.Template, ProdutoDelete.Methods, ProdutoDelete.Handle);

app.MapMethods(FuncionarioPost.Template, FuncionarioPost.Methods, FuncionarioPost.Handle);
app.MapMethods(FuncionarioGetAll.Template, FuncionarioGetAll.Methods, FuncionarioGetAll.Handle);

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);
app.Run();