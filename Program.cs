using MeetingsAPI_V3.Data;
using MeetingsAPI_V3.DatabaseSeed;
using MeetingsAPI_V3.Services;
using Microsoft.EntityFrameworkCore;
using SoapCore;
using System.ServiceModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MeetingsDatabase"), options => options.EnableRetryOnFailure());
});


builder.Services.AddSoapCore();
builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
builder.Services.AddScoped<IMeetingService, MeetingService>();

builder.Services.AddMvc();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

Seed.PrepSeed(app);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.UseSoapEndpoint<IMeetingService>(opt =>
    {
        opt.Path = "/meetings.asmx";
        opt.SoapSerializer = SoapSerializer.DataContractSerializer;
        opt.AdditionalEnvelopeXmlnsAttributes = new Dictionary<string, string>()
        {
            { "meeting", "http://schemas.datacontract.org/2004/07/MeetingsAPI_V3.Models" },
            { "user", "http://schemas.datacontract.org/2004/07/MeetingsAPI_V3.Entities"},
            { "meet", "http://schemas.datacontract.org/2004/07/MeetingsAPI_V3.Entities.Meeting" }
        };
    });
});


app.MapControllers();

app.Run();

