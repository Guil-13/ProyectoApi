using FluentValidation;
using Microsoft.AspNetCore.Cors;
using ProyectoApi.Endpoints;
using ProyectoApi.Repositorios;
using ProyectoApi.Servicios;

var builder = WebApplication.CreateBuilder(args);
//poner la ip de quien puede acceder en el appsettings
var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!;
#region Servicios
// Politicas de CORS para permitir acceso desde otro origen web.
builder.Services.AddCors(opciones =>
{
    // solo origenes especificos (ver appsettings.Development.json)
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
    //Cualquier pagina web puede comunicarse con nosotros
    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
builder.Services.AddScoped<IRepositorioStoredProcedures, RepositorioStoredProcedures>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

//servicio fluent validation 
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();
#endregion
var app = builder.Build();

#region Middleware
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePages();
app.UseStaticFiles();

app.UseCors();
app.UseOutputCache();
app.MapGet("/", [EnableCors(policyName: "libre")] () => "Web Api - Proyecto API para Diplomado LANIA - ©2025");

app.MapGroup("/usuarios").MapUsuarios();
app.MapGroup("/personas").MapPersonas();
app.MapGroup("/domicilios").MapDomicilios();
app.MapGroup("/contactos").MapContactos();
app.MapGroup("/estudios").MapEstudios();//File
app.MapGroup("/documentos").MapDocumentos();//File
app.MapGroup("/pagos").MapPagos();//File
app.MapGroup("/inscripciones").MapInscripciones();
app.MapGroup("/convocatorias").MapConvocatorias();
app.MapGroup("/sp").MapStoredProcedures();
#endregion
app.Run();
