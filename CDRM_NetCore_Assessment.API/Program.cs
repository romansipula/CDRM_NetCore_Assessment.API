using CDRM_NetCore_Assessment.API.Storage;
using CDRM_NetCore_Assessment.API.Formatters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register storage
builder.Services.AddSingleton<IDocumentStorage, InMemoryDocumentStorage>();

// Register formatters
builder.Services.AddSingleton<IDocumentFormatter, JsonDocumentFormatter>();
builder.Services.AddSingleton<IDocumentFormatter, XmlDocumentFormatter>();
builder.Services.AddSingleton<IDocumentFormatter, MessagePackDocumentFormatter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
