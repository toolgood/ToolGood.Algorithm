using Microsoft.AspNetCore.StaticFiles;

namespace ToolGood.Algorithm2.WebTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
            }
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".dll"] = "application/octet-stream";//配置添加新的映射关系
            provider.Mappings[".gz"] = "application/octet-stream";
            provider.Mappings[".dat"] = "application/octet-stream";
            provider.Mappings[".blat"] = "application/octet-stream";
            provider.Mappings[".pdb"] = "application/octet-stream";
            provider.Mappings[".wasm"] = "application/octet-stream";
            app.UseStaticFiles(new StaticFileOptions {
                ContentTypeProvider = provider,//应用新的映射关系
            });
			


			app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
