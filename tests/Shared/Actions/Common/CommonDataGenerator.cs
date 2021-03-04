using Bogus;

namespace Actions.Common
{
    static class CommonDataGenerator
    {
        public static string GenerateString(int minLength, int maxLength)
        {
            var data = new Faker().Random.String2(minLength, maxLength);
            return data;
        }
    }
}
