//--------------------------------------------------
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//--------------------------------------------------

using Azure.Storage.Blobs;
using VideoTime.Brokers.Blobs;
using VideoTime.Brokers.DateTimes;
using VideoTime.Brokers.Loggings;
using VideoTime.Brokers.Storages;
using VideoTime.Components;
using VideoTime.Services.Blobs;
using VideoTime.Services.Foundations.VideoMetadatas;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        AddTransient(builder);
        builder.Services.AddHttpClient();
        builder.Services.AddControllers();  
        builder.Services.AddRazorPages(options =>
        {
            options.RootDirectory = "/Views/Pages";
        });
        builder.Services.AddSingleton<IBlobBroker, BlobBroker>();
        builder.Services.AddSingleton(_ => new BlobServiceClient(
            builder.Configuration.GetConnectionString("AzureBlobStorageConnection")));
        var app = builder.Build();
       
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);

            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
    private static void AddTransient(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IStorageBroker, StorageBroker>();
        builder.Services.AddTransient<IVideoMetadataService, VideoMetadataService>();
        builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
        builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
        builder.Services.AddTransient<IBlobService, BlobService>(); 
    }
}
       
