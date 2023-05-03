using System.Net;
using Microsoft.AspNetCore.Builder;

namespace m01_Start
{
     public static class AppExtends
     {
          public static void AddStatusCodePage(this IApplicationBuilder app)
          {
               app.UseStatusCodePages(
                   appError =>
                   {
                        appError.Run(async context =>
                        {
                             var response = context.Response;
                             var code = response.StatusCode;
                             var content = @$"<html>
                              <head>
                                   <meta charset='UTF-8'/>
                                   <title>Error {code}</title>

                              </head>
                              <body style='color: red; font-size:24px'>
                                   There was an error {code} - {(HttpStatusCode)code}
                              </body>
                              </html>";
                             await response.WriteAsync(content);
                        });
                   }
               );
          }
     }
}