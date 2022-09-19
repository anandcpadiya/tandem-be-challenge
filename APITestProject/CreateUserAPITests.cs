using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TandemBEProject.DTOs;
using static System.Net.Mime.MediaTypeNames;


namespace APITestProject
{
    [TestClass]
    public class CreateUserAPITests
    {
        private readonly HttpClient _client;

        public CreateUserAPITests()
        {
            WebApplicationFactory<Program> webAppFactory = new();

            _client = webAppFactory.CreateDefaultClient();
        }


        [TestMethod]
        public async Task ShouldReturnBadRequestIfEmailIsNotSupplied()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/users");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestMethod]
        public async Task ShouldReturnBadRequestIfEmailIsInvalid()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/users?email=bad_email");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestMethod]
        public async Task ShouldReturnNotFoundIfUserDoesNotExist()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/users?email=fake_email@fakedomain.com");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


        [TestMethod]
        public async Task ShouldReturnBadRequestWhileCreatingUserWhenEmailIsInvalid()
        {
            HttpResponseMessage response = await SendCreateUserRequestAsync("bad_email");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            string responseJson = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(responseJson.Contains("The EmailAddress field is not a valid e-mail address."));
        }


        [TestMethod]
        public async Task ShouldCreateAndGetAndUpdateUserAndReturnConflictSecondTime()
        {
            string randomText = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 5)
                .Select(s => s[new Random().Next(s.Length)]).ToArray()
            );
            string email = $"mock_email_{randomText}@mock.com";

            HttpResponseMessage response = await SendCreateUserRequestAsync(email);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            await VerifyUserResponseDto(response, email, "MockFirstName MockMiddleName MockLastName");


            // Should get the created user by email
            HttpResponseMessage getUserResponse = await _client.GetAsync(
                $"/api/users?email={email}"
            );
            Assert.AreEqual(HttpStatusCode.OK, getUserResponse.StatusCode);
            await VerifyUserResponseDto(getUserResponse, email, "MockFirstName MockMiddleName MockLastName");


            // Should return conflict when we try to create a user with the same email second time
            HttpResponseMessage conflictResponse = await SendCreateUserRequestAsync(email);
            Assert.AreEqual(HttpStatusCode.Conflict, conflictResponse.StatusCode);


            // Should update the user
            CreateUserRequestDto toBeUpdated = GetMockUser();
            toBeUpdated.EmailAddress = email;
            toBeUpdated.MiddleName = "UpdatedMiddleName";
            HttpContent? updateRequestBody = new StringContent(
                JsonConvert.SerializeObject(toBeUpdated), Encoding.UTF8, Application.Json
            );

            HttpResponseMessage updateResponse = await _client.PutAsync("/api/users", updateRequestBody);
            Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

            await VerifyUserResponseDto(updateResponse, email, "MockFirstName UpdatedMiddleName MockLastName");
        }


        [TestMethod]
        public async Task ShouldReturnNotFoundIfUserDoesNotExistWhileUpdating()
        {
            CreateUserRequestDto user = GetMockUser();
            user.EmailAddress = "fake_email@fakedomain.com";

            HttpContent? body = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, Application.Json);

            HttpResponseMessage updateResponse = await _client.PutAsync("/api/users", body);

            Assert.AreEqual(HttpStatusCode.NotFound, updateResponse.StatusCode);
        }

        private static async Task VerifyUserResponseDto(HttpResponseMessage response, string expectedEmail, string expectedName)
        {
            string responseJson = await response.Content.ReadAsStringAsync();
            UserResponseDto responseDto = JsonConvert.DeserializeObject<UserResponseDto>(responseJson);

            Assert.AreEqual(expectedEmail, responseDto.EmailAddress);
            Assert.AreEqual(expectedName, responseDto.Name);
        }


        private async Task<HttpResponseMessage> SendCreateUserRequestAsync(string email)
        {
            CreateUserRequestDto user = GetMockUser();
            user.EmailAddress = email;
            HttpContent? body = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, Application.Json);

            return await _client.PostAsync("/api/users", body);
        }


        private static CreateUserRequestDto GetMockUser()
        {
            return new CreateUserRequestDto()
            {
                FirstName = "MockFirstName",
                MiddleName = "MockMiddleName",
                LastName = "MockLastName",
                PhoneNumber = "+9876543210"
            };
        }
    }
}
