﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sia.Gateway.Middleware;
using System;
using System.Collections.Generic;

namespace Sia.Gateway.Initialization
{
    public static class MiddlewareStartup
    {
             

        public static void AddMiddleware(this IApplicationBuilder app, IHostingEnvironment env, IConfigurationRoot configuration)
        {
            app.UseCors(builder =>
                builder
                .WithOrigins(LoadAcceptableOriginsFromConfig(configuration))
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            );
            app.UseAuthentication();
            app.UseSession();

            if (env.IsDevelopment() || env.IsStaging()) app.UseDeveloperExceptionPage();
            app.UseMiddleware<ExceptionHandler>();

            app.UseMvc();
        }

        private static string[] LoadAcceptableOriginsFromConfig(IConfigurationRoot configuration)
        {
            List<string> corsOrigins = new List<string>();

            int i = 0;
            while (!string.IsNullOrWhiteSpace(configuration[$"CORS:AcceptableOrigins:{i}"]))
            {
                corsOrigins.Add(configuration[$"CORS:AcceptableOrigins:{i}"]);
                i++;
            }

            return corsOrigins.ToArray();
        }
    }
}
