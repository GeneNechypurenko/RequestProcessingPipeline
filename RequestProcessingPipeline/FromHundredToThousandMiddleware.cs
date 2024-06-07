namespace RequestProcessingPipeline
{
    public class FromHundredToThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromHundredToThousandMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 101)
                {
                    await _next.Invoke(context);
                }
                else if(number == 1000)
                {
                    await context.Response.WriteAsync("thousand");
                }
                else if (number > 1000)
                {
                    await context.Response.WriteAsync("Out of range");
                }
                else
                {
                    string result = ConvertNumberToWords(number);
                    await context.Response.WriteAsync("Your number is " + result);
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }

        private string ConvertNumberToWords(int number)
        {
            string toWords = "";

            if ((number / 100) > 0)
            {
                toWords += ConvertNumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (toWords != "")
                    toWords += "and ";

                string[] units = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                string[] tens = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    toWords += units[number];
                else
                {
                    toWords += tens[number / 10];
                    if ((number % 10) > 0)
                        toWords += units[number % 10];
                }
            }

            return toWords;
        }
    }
}
