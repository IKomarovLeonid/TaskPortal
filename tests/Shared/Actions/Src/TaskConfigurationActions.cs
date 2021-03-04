using Actions.Common;
using API.Src.ApiConnector;
using Bogus;

namespace Actions.Src
{
    public static class TaskConfigurationActions
    {
        public static Faker<CreateConfigurationRequestModel> ValidOnlyRequiredRequest()
        {
            var request = new Faker<CreateConfigurationRequestModel>()
                .RuleFor(t => t.Name, f => CommonDataGenerator.GenerateString(5,50));

            return request;
        }

        public static Faker<CreateConfigurationRequestModel> InvalidOnlyRequiredRequest()
        {
            var request = new Faker<CreateConfigurationRequestModel>()
                .RuleFor(t => t.Name, f => CommonDataGenerator.GenerateString(0,5));

            return request;
        }
    }
}
