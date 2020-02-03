using System;
using ResultType;
using static ResultType.Result;

namespace ResultTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var res = ParseString("45u")
                .Then(n => Ok(new DateTime(2020, 4, n)))
                .Then(d => Ok(d.ToString()));

            var dateRes = CreateDate(2020, 14, 20);
            
            if (dateRes.IsOk)
                Console.WriteLine(dateRes.Value);
            else
                Console.WriteLine(dateRes.ErrorMessage);
        }

        static Result<DateTime> CreateDate(int year, int month, int day)
        {
            if (year > 2020 || month > 12 || month < 1 || day < 1 || day > 31)
                return Error<DateTime>("Wrong Date");
            return new DateTime(year, month, day);
        }
        
        static Result<int> Multiply(int a, int b)
        {
            if (a < 0) return Error<int>(a + " is less than zero");
            if (b < 0) return Error<int>(b + " is less than zero");
            else return a * b;
        }

        static Result<int> ParseString(string text)
        {
            return int.TryParse(text, out int num) ?
                num : 
                Error<int>($"{text} is not a number");
        }
    }
}