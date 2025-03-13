using Asp.Versioning;
using BoilerPlate.Application;
using BoilerPlate.Graphql;
using BoilerPlate.Infrastructure;
using BoilerPlate.Web.Extensions;
using BoilerPlate.Web.Filters.Swashbuckle;
using BoilerPlate.Web.ModelStateError;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }).
    ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = actionContext =>
        {
            var errors = actionContext.ModelState
                        .Where(e => e.Value!.Errors.Count > 0)
                        .Select(e => new ModelError { Field = e.Key, Message = e.Value!.Errors.First().ErrorMessage })
                        .ToList();
            return new BadRequestObjectResult(new ModelStateErrorResponse { Errors = errors });
        };
    });

builder.Services.AddApiVersioning(Option =>
{
    Option.AssumeDefaultVersionWhenUnspecified = true;
    Option.DefaultApiVersion = ApiVersion.Default;
    Option.ReportApiVersions = true;
}).AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV";
    option.SubstituteApiVersionInUrl = true;
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs();
builder.Services.AddApplicationConfig(builder.Configuration);

builder.Services.AddGraphql();
builder.Services.AddInfrastructureConfig(builder.Configuration.GetConnectionString("DefaultConnectionString")!);

builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<SnakeCaseDictionaryFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomSchemaIds(type => type.FullName);

    c.SchemaFilter<SnakeCaseDictionaryFilter>();
});



var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "BoilerPlateApiV1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "BoilerPlateApiV2");
});

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();