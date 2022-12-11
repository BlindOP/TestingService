using Microsoft.Extensions.Configuration;
using TestingClient;
using TestingLib;



IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

AppSettings settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();


ServerCommunication communication = new ServerCommunication(settings);

while (true)
{
    Console.WriteLine("Список доступных тестов:");

    var listOfTests = WriteAndGetListOfTests(communication);
    if (listOfTests == null || listOfTests.Count == 0) break;

    Console.WriteLine("Выберите тест");
    var testIndex = selectTest();

    var listOfQuestions = WriteListOfQuestions(communication, listOfTests[testIndex - 1]);

    Console.WriteLine("Нажмите Enter для начала решения теста, либо другую клавишу для выбора другого теста");

    if (Console.ReadKey().Key != ConsoleKey.Enter) continue;

    TestResolving(communication, listOfTests[testIndex - 1], listOfQuestions);

    Console.WriteLine("Тест пройден, нажмите любую клавишу для выбора нового теста");

    Console.ReadLine();
}

static int selectTest()
{
    bool parseResult;
    int testNumber;
    do
    {
        var strTestNumber = Console.ReadLine();


        parseResult = int.TryParse(strTestNumber, out testNumber);
    } while (!parseResult);
    return testNumber;
}

static List<Question> WriteListOfQuestions(ServerCommunication communication, string testName)
{
    List<Question> listOfQuestins = communication.GetTest(testName).ToList();

    foreach (var item in listOfQuestins)
    {
        Console.WriteLine(item.Text);
    }
    return listOfQuestins;
}

static List<string> WriteAndGetListOfTests(ServerCommunication communication)
{
    try
    {
        List<string> list = communication.GetListOfTests().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {list[i]}");
        }
        if (list.Count == 0) Console.WriteLine("Нет доступных тестов");
        return list;
    }
    catch (Exception e)
    {
        Console.WriteLine("Нет соединения с сервером");
        return null;
    }

}

static void TestResolving(ServerCommunication communication, string testName, List<Question> listOfQuestions)
{
    for (int i = 0; i < listOfQuestions.Count; i++)
    {
        Console.WriteLine($"Вопрос №{i + 1}: {listOfQuestions[i].Text}");
        Console.Write("Ответ:");
        var answer = Console.ReadLine();
        var answerAfterCheck = communication.CheckAnswer(testName, listOfQuestions[i].Text, answer);
        if (answerAfterCheck.IsCorrect == true)
        {
            Console.WriteLine("Правильно");
        }
        else
        {
            Console.WriteLine($"Неправильно. Ответ {answerAfterCheck.CorrectAnswer}");
        }

    }
}