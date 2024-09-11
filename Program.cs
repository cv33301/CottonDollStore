using DbCottonDollStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//註冊資料庫
builder.Services.AddDbContext<CottonDollStoreContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbCottonDollStoreConnection")));

//註冊Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);//保留時間
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");//500的錯誤
}
app.UseStatusCodePagesWithReExecute("/Home/Error");//400的錯誤

app.UseSession();//啟用Session

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
