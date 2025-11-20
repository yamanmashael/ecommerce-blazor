using Blazor;
using Blazor.Authentication;
using Blazor.Data;
using Blazor.Services;
using Blazored.Toast;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSyncfusionBlazor();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie();




builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();



builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ProductImageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AuthenticationStateProvider,CustonAuthStateProvider>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<Authentication>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<Complaint>();
builder.Services.AddScoped<ColorService>();
builder.Services.AddScoped<SizeService>();
builder.Services.AddScoped<OrderStatusService>();
builder.Services.AddScoped<GenderService>();

builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<CategoryItemService>();
builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<ProductItemService>();
builder.Services.AddScoped<ProductItemSizeService>();





builder.Services.AddAuthorizationCore();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


//builder.Services.AddScoped(sp => new HttpClient
//{
//    BaseAddress = new Uri("https://localhost:7233/")  
//});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://webappcommerceapi-csgravgyd4egh9cz.indonesiacentral-01.azurewebsites.net/") });



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();


app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();
