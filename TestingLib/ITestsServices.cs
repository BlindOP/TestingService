using TestingLib;

namespace TestingLib
{
    public interface ITestsService
    {
        IEnumerable<string> GetListOfTests();
        IEnumerable<Question> GetTest(string testName);
        Answer CheckAnswer(string testName, string question, string userAnswer);
    }
}
