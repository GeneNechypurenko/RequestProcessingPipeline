namespace RequestProcessingPipeline
{
    public class FromTenThousandToHundredThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTenThousandToHundredThousandMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            int number = Convert.ToInt32(token);
            number = Math.Abs(number);
            if (number < 10001)
            {
                await _next.Invoke(context);
            }
            else if (number > 100000)
            {
                await context.Response.WriteAsync("Number is out of range (1-100000)");
            }
            else
            {
                string result = ConvertNumberToWords(number);
                await context.Response.WriteAsync("Your number is " + result);
            }
        }

        private string ConvertNumberToWords(int number)
        {
            string toWords = "";

            if ((number / 1000) > 0)
            {
                toWords += ConvertNumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

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
                        toWords += "-" + units[number % 10];
                }
            }

            return toWords;
        }
    }
}
