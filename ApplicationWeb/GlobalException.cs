using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Text;

namespace ApplicationWeb {
    public static class GlobalException {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app) {

            app.UseExceptionHandler(errorApp => {
                errorApp.Run(async context => {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null) {
                        var ex = error.Error;

                        var result = new ErroResponse(ex.Message);

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
                    }
                });
            });

        }
    }

    public class ErroResponse {
        public string erro { get; set; }

        public ErroResponse(string erro) {
            this.erro = erro;
        }
    }

}
