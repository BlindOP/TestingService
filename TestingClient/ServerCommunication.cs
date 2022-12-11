using System.Text.Json;
using TestingLib;

namespace TestingClient
{
    internal class ServerCommunication : ITestsService
    {
        private readonly AppSettings _config;
        public ServerCommunication(AppSettings testConfig)
        {
            _config = testConfig;
        }

        public IEnumerable<string> GetListOfTests()
        {
            List<string>? result;

            var reqestResult = requestToServer("/Tests/GetListOfTests").Result;
            result = JsonSerializer.Deserialize<List<string>>(reqestResult);

            return result;
        }

        public IEnumerable<Question> GetTest(string testName)
        {

            var reqestResult = requestToServer($"/Tests/GetTest?testName={testName}").Result;
            var result = JsonSerializer.Deserialize<List<Question>>(reqestResult);

            return result;
        }

        public Answer CheckAnswer(string testName, string question, string answer)
        {
            Answer result;

            var reqestResult = requestToServer($"/Tests/CheckAnswer?testName={testName}&question={question}&answer={answer}").Result;
            result = JsonSerializer.Deserialize<Answer>(reqestResult);

            return result;
        }

        internal async Task<string> requestToServer(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync($"{_config.serverAddres}{url}");
            }
        }
    }

}
