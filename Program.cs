using LojaAPI.Endpoints.Pedidos;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSerilog((context, configuration) =>
{
    configuration
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        context.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions()
        {
            AutoCreateSqlTable = true,
            TableName = "LogAPI"
        });
});
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { //diminuindo a segurança da senha
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization(options => { //por padrão o usuário precisa está autenticado
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
    options.AddPolicy("SomenteFuncionario", p =>
        p.RequireAuthenticatedUser().RequireClaim("CodigoFuncionario"));
    options.AddPolicy("CpfRequisitado", p =>
        p.RequireAuthenticatedUser().RequireClaim("Cpf"));
});
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero, //tempo de bonus depois da expiração do token
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});
builder.Services.AddScoped<QueryAllUsersWithClaims>();
builder.Services.AddScoped<QueryProductReport>();
builder.Services.AddScoped<UsuarioCreator>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//criando endpoints
app.MapMethods(CategoriaPost.Template, CategoriaPost.Methods, CategoriaPost.Handle);
app.MapMethods(CategoriaGetAll.Template, CategoriaGetAll.Methods, CategoriaGetAll.Handle);
app.MapMethods(CategoriaPut.Template, CategoriaPut.Methods, CategoriaPut.Handle);
app.MapMethods(CategoriaDelete.Template, CategoriaDelete.Methods, CategoriaDelete.Handle);

app.MapMethods(ProdutoPost.Template, ProdutoPost.Methods, ProdutoPost.Handle);
app.MapMethods(ProdutoGetAll.Template, ProdutoGetAll.Methods, ProdutoGetAll.Handle);
app.MapMethods(ProdutoGetVitrine.Template, ProdutoGetVitrine.Methods, ProdutoGetVitrine.Handle);
app.MapMethods(ProdutoPut.Template, ProdutoPut.Methods, ProdutoPut.Handle);
app.MapMethods(ProdutoDelete.Template, ProdutoDelete.Methods, ProdutoDelete.Handle);
app.MapMethods(RelatorioProdutos.Template, RelatorioProdutos.Methods, RelatorioProdutos.Handle);

app.MapMethods(FuncionarioPost.Template, FuncionarioPost.Methods, FuncionarioPost.Handle);
app.MapMethods(FuncionarioGetAll.Template, FuncionarioGetAll.Methods, FuncionarioGetAll.Handle);

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);

app.MapMethods(ClientePost.Template, ClientePost.Methods, ClientePost.Handle);
app.MapMethods(ClienteGet.Template, ClienteGet.Methods, ClienteGet.Handle);

app.MapMethods(PedidoPost.Template, PedidoPost.Methods, PedidoPost.Handle);
app.MapMethods(PedidoGet.Template, PedidoGet.Methods, PedidoGet.Handle);

app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext http) => {
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;
    if (error != null) 
    {
        if(error is SqlException)
        {
            return Results.Problem("Banco de dados offline", statusCode: 500);
        }
        else if(error is BadHttpRequestException)
        {
            return Results.Problem("Erro de conversão de tipo. Verifique todas as informações enviadas", statusCode: 500);
        }
    }
    return Results.Problem("Um erro ocorreu", statusCode: 500);
});

app.Run();