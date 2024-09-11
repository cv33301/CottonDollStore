using DbCottonDollStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//���U��Ʈw
builder.Services.AddDbContext<CottonDollStoreContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbCottonDollStoreConnection")));

//���USession
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);//�O�d�ɶ�
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");//500�����~
}
app.UseStatusCodePagesWithReExecute("/Home/Error");//400�����~

app.UseSession();//�ҥ�Session

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
