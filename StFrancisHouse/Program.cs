using MySql.Data.EntityFrameworkCore;
using StFrancisHouse.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<UserContext>();

builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddMvc();

builder.Services.Add(new ServiceDescriptor(typeof(UserContext), new UserContext(builder.Configuration.GetConnectionString("DefaultConnection"))));


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapRazorPages();

app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
            });

            // Mapping API routes + responses
app.MapGet("/helloworld",  () => "Hello World!"    );

app.Run();
