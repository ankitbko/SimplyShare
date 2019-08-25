using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SimplyShare.Core.Models;
using Microsoft.Extensions.Logging;
using SimplyShare.Core;
using Microsoft.AspNetCore.Http;

namespace SimplyShare.Tracker.Formatters
{
    public class BencodeOutputFormatter : TextOutputFormatter
    {
        public BencodeOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(AnnounceResponse).IsAssignableFrom(type))
                return base.CanWriteType(type);

            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService(typeof(ILogger<BencodeOutputFormatter>)) as ILogger;

            var response = context.HttpContext.Response;

            var bencoded = BencodeConvert.Serialize(context.Object);

            await response.WriteAsync(bencoded, selectedEncoding);
        }
    }
}
