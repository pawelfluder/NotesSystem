using AspnetFrontendAdapterProg.Repetition;
using SharpRepoBackendProg.Service;
using Unity;

namespace SharpRepoBackendProg
{
    public class FrontendAdapter
    {
        private readonly IBackendService backendService;

        public FrontendAdapter()
        {
            backendService = MyBorder.Container.Resolve<IBackendService>();
        }

        public void Run(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var myLocalHost8081 = "myLocalHost8081";
            var myLocalHost3000 = "myLocalHost3000";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: myLocalHost8081, policy =>
                {
                    policy.WithOrigins("http://localhost:8081")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: myLocalHost3000, policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors(myLocalHost8081);
            app.UseCors(myLocalHost3000);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/repoApi/{repo}/{loca}", (string repo, string loca) =>
            {
                var result = backendService.RepoApi(repo, loca);
                return result;
            });

            app.MapGet("/commandApi/{cmdName}/{repo}/{loca}",
                (string cmdName, string repo, string loca) =>
            {
                var result = backendService.CommandApi(cmdName, repo, loca);
                return result;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.Run();
        }
    }
}
