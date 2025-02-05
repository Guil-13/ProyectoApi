using FluentValidation;
using Microsoft.AspNetCore.Cors;

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

app.UseStaticFiles();

app.UseCors();
app.UseOutputCache();
app.MapGet("/", [EnableCors(policyName: "libre")] () => "Web Api - Proyecto API para Diplomado LANIA - ©2025");

#endregion
app.Run();
