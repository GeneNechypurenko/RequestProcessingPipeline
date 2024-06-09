namespace RequestProcessingPipeline
{
    public static class FromTenThousandToHundredThousandExtensions
    {
        public static IApplicationBuilder UseFromTenThousandToHundredThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromTenThousandToHundredThousandMiddleware>();
        }
    }
}
