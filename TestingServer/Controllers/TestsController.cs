using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Serialization;
using TestingLib;
using TestingServer.Services;

namespace TestingServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly ITestsService _testsService;
        public TestsController(ITestsService mailService)
        {
            _testsService = mailService;
        }

        [HttpGet()]
        [Route(nameof(GetListOfTests))]
        public IActionResult GetListOfTests()
        {
            try
            {
                var listOfTests = _testsService.GetListOfTests();
                return Ok(listOfTests);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet()]
        [Route(nameof(GetTest))]

        public IActionResult GetTest(string testName)
        {
            try
            {
                var currentTest = _testsService.GetTest(testName).Select(x => new Question {Text = x.Text, Answer = ""}).ToList();
                return Ok(currentTest);
            }
            catch
            {
                return BadRequest("Не можем получить тест с таким именем");
            }
        }
        [HttpGet()]
        [Route(nameof(CheckAnswer))]
        public IActionResult CheckAnswer(string testName, string question, string answer)
        {
            try
            {
                var chekedAnswer = _testsService.CheckAnswer(testName, question, answer);
                return Ok(chekedAnswer);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost()]
        [Route(nameof(CreateTest))]
        public  IActionResult CreateTest(string testName)
        {
            var listOfQuestion = new List<Question>();
            for (int i = 0; i < 10; i++)
            {
                listOfQuestion.Add(new Question($"question{i}", $"answ{i}"));
            }

            XmlSerializer? xmlSerializer = new XmlSerializer(typeof(Question[]));
            using (FileStream? stream = new FileStream($"./{testName}.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(stream, listOfQuestion);
                return Ok();
            }


        }
    }
}
