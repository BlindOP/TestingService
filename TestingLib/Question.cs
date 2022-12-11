using System.Collections;
using System.Text.Json.Serialization;

namespace TestingLib
{
    public class Question
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
        [JsonPropertyName("answer")]
        public string? Answer { get; set; }
        public Question()
        {

        }
        public Question(string text, string answer)
        {
            this.Text = text;
            this.Answer = answer;
        }
    }
    public class Answer
    {
        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }
        [JsonPropertyName("correctAnswer")]
        public string CorrectAnswer { get; set; } = string.Empty;

        public Answer()
        {

        }
        public Answer(bool isCorrect, string correctAnswer)
        {
            this.IsCorrect = isCorrect;
            this.CorrectAnswer = correctAnswer;
        }
    }
}
