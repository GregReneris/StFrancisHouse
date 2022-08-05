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


//Adding cors to override defaults.
//builder.Services.AddCors(c =>
//{
//    //should allow any website to use ours.
//    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
//});

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

//not sure if Cors needs to go here or elsewhere.
app.UseCors();

app.UseRouting();

app.UseAuthorization();

//Adding in this authentication because postman was breaking. 
//app.UseAuthentication();

//app.MapRazorPages();

app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
            });

            // Mapping API routes + responses
app.MapGet("/helloworld",  () => "Hello World!"    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
