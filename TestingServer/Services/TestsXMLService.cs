using Microsoft.Extensions.Options;
using TestingServer.Config;
using System.Xml.Serialization;
using System;
using TestingLib;

namespace TestingServer.Services
{
    internal class TestsXMLService : ITestsService
    {
        private readonly AppSettings _config;
        public TestsXMLService(IOptions<AppSettings> testConfig)
        {
            _config = testConfig.Value;
        }

        public IEnumerable<string> GetListOfTests()
        {
            var allfiles = Directory.EnumerateFiles(_config.PathToFiles, "*.xml").Select(x => Path.GetFileNameWithoutExtension(x));
            return allfiles;  
        }

        public IEnumerable<Question> GetTest(string testName)
        {
            XmlSerializer? xmlSerializer = new XmlSerializer(typeof(List<Question>));
            using (FileStream? stream = new FileStream($"{_config.PathToFiles}{testName}.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                List<Question> test = xmlSerializer.Deserialize(stream) as List<Question>;
                return test;
            }
            
        }

        public Answer CheckAnswer(string testName, string question, string userAnswer)
        {
            var questionWithAnswer = GetTest(testName).First(x => x.Text == question);
            if (questionWithAnswer.Answer.ToLower().Trim() == userAnswer.ToLower().Trim())
            {
                return new Answer(true, questionWithAnswer.Answer );
            }
            else
            {
                return new Answer(false, questionWithAnswer.Answer );
            }
        }
    }
}
