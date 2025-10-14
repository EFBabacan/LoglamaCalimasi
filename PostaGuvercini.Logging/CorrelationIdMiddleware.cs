using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace PostaGuvercini.Logging
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeaderName = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Gelen isteğin header'ında Correlation ID var mı diye kontrol et.
            // Varsa onu kullan, yoksa yeni bir tane oluştur.
            var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var id)
                ? id.ToString()
                : Guid.NewGuid().ToString();

            // 2. Serilog'un LogContext'ine bu ID'yi ekle.
            // 'using' bloğu sayesinde, bu ID sadece bu istek bitene kadar geçerli olacak.
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                // 3. İsteğin devam etmesi için bir sonraki middleware'i çağır.
                await _next(context);
            }
        }
    }
}