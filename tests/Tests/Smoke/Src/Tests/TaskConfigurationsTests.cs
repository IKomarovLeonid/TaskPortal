using Actions.Src;
using API.Src.ApiConnector;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xbehave;

namespace Smoke.Src.Tests
{
    public class TaskConfigurationsTests
    {
        // move to infrastructure
        private readonly ConfigurationsClient _httpClient;
        public TaskConfigurationsTests()
        {
            _httpClient = new ConfigurationsClient();
        }

        [Scenario]
        public async Task Can_CreateNewConfiguration_By_CorrectName()
        {
            // arrange
            var configurationRequest = TaskConfigurationActions.ValidOnlyRequiredRequest();

            // act 
            var result = await _httpClient.CreateAsync(configurationRequest);

            // assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.AffectionTimeUtc.Should().BeAfter(DateTimeOffset.MinValue);
         

        }

        [Scenario]
        public async Task Can_NotCreateConfigurationWith_InvalidName()
        {
            // arrange
            var configurationRequest = TaskConfigurationActions.InvalidOnlyRequiredRequest();

            // act 
            var task = new Func<Task<AffectionViewModel>>(async () => await _httpClient.CreateAsync(configurationRequest));

            // assert
            var result = await task.Should().ThrowAsync<ApiException>();
            result.And.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        
        }

        [Scenario]
        public async Task Can_GetAllTaskConfigurations()
        {
            // act
            var configurations = await _httpClient.GetConfigurationsAsync();

            // assert
            configurations.Should().NotBeNull();
            configurations.Items.Should().NotBeEmpty();
        }

        [Scenario] // Need to investigate why i receive null in name when get by ID
        public async Task Can_GetConfiguration_By_CreatedId()
        {
            // arrange
            var configurationRequest = TaskConfigurationActions.ValidOnlyRequiredRequest();
            var createdConfiguration = await _httpClient.CreateAsync(configurationRequest);
            // act 
            var configurations = await _httpClient.GetConfigurationsAsync();
            var configuration = configurations.Items.FirstOrDefault(t => t.Id == createdConfiguration.Id);
         
            // assert
            configuration.Should().NotBeNull();
            configuration.Name.Should().NotBeNull();
            configuration.Permissions.Should().NotBeNull();
            configuration.SystemSettings.Should().NotBeNull();
            configuration.UpdatedTimeUtc.Should().BeAfter(DateTimeOffset.MinValue);
            configuration.CreatedTimeUtc.Should().BeAfter(DateTimeOffset.MinValue);
        }

        [Scenario]
        public async Task Can_DeleteConfiguration_If_Exists()
        {
            // arrange
            var configurationRequest = TaskConfigurationActions.ValidOnlyRequiredRequest();
            var createdConfiguration = await _httpClient.CreateAsync(configurationRequest);

            // act 
            await _httpClient.DeleteAsync(createdConfiguration.Id);

            // assert
            var configurations = await _httpClient.GetConfigurationsAsync();
            var configuration = configurations.Items.FirstOrDefault(t => t.Id == createdConfiguration.Id);
            configuration.Should().BeNull();
        }

        [Scenario]
        public async Task Can_NotGetNotExistedConfiguration()
        {
            // arrange
            var task = new Func<Task<ConfigurationModel>>(async () => await _httpClient.GetByIdAsync(int.MaxValue));

            // act and assert
            var result = await task.Should().ThrowAsync<ApiException>();
            result.And.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }

        [Scenario]
        public async Task Always_Has_DefaultTaskConfiguration()
        {
            // act
            var configurations = await _httpClient.GetConfigurationsAsync();
            var defaultConfiguration = configurations.Items.FirstOrDefault(t => t.IsDefault);

            // assert
            defaultConfiguration.Should().NotBeNull();
        }

        [Scenario]
        public async Task Can_Not_RemoveDefaultTaskConfiguration()
        {
            // arrange
            var configurations = await _httpClient.GetConfigurationsAsync();
            var defaultConfiguration = configurations.Items.FirstOrDefault(t => t.IsDefault);

            // act
            var task = new Func<Task<AffectionViewModel>>(async () => await _httpClient.DeleteAsync(defaultConfiguration.Id));

            var expected = await task.Should().ThrowAsync<ApiException>();
            expected.And.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        // can not remove if has active tasks
    }
}
