using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = "Cookies";
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie("Cookies", options =>
    {
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://oauth.pslib.cz";
        options.ClientId = "demo";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.RequireHttpsMetadata = true;
        options.Scope.Clear();
        options.Scope.Add("profile");
        options.Scope.Add("openid");
        options.Scope.Add("demo.api");
        //options.Scope.Add("offline_access");
        //options.Scope.Add("email");
        options.GetClaimsFromUserInfoEndpoint = false; // true
        options.TokenValidationParameters = new()
        {
            NameClaimType = "name",
            RoleClaimType = "role",
            ValidateIssuer = true,
            ValidIssuer = "https://oauth.pslib.cz"
        };
        options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
        {/*
            OnTokenValidated = ctx =>
            {
                
                var email = ctx.Principal?.FindFirst("email")?.Value;
                if (email is null || !email.EndsWith("@pslib.cz", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Fail("Email není z povolené domény.");
                }
                return Task.CompletedTask;
            }
            */
        };
        options.SignedOutCallbackPath = "/signout-callback-oidc";
        options.SignedOutRedirectUri = "/";
    });

builder.Services.AddHttpClient("WeatherApi", (sp, http) =>
{
    //var cfg = sp.GetRequiredService<IConfiguration>();
    http.BaseAddress = new Uri("https://localhost:7175");
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
