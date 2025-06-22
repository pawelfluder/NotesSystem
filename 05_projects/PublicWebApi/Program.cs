using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

WebApplicationBuilder builder = WebApplication
    .CreateBuilder(args);

OutBorder01.GetPreparer("DefaultPreparer").Prepare();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("https://localhost:7269")
            .WithOrigins("http://localhost:5035")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();
app.Run();
