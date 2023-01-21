using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Extensions;

public static class StringExtensions
{
    public static string ToBase64Guid(this Guid input)
    {
        return new Regex("[^a-zA-Z0-9 -]").Replace(Convert.ToBase64String(input.ToByteArray()), ""); ;
    }
    public static string ToMd5(this string input)
    {
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
    public static string ToStringUtc(this DateTime time)
    {
        return $"DateTime({time.Ticks}, DateTimeKind.Utc)";
    }
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => string.Empty,
            "" => input,
            _ => input.First().ToString().ToUpper() + input.Substring(1)
        };

    public static string ClearPhones(this string input) => input switch
    {
        null => string.Empty,
        "" => input,
        _ => input.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "")
    };

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}