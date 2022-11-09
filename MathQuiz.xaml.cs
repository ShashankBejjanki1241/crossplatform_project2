using Newtonsoft.Json;
namespace Calculator;

public partial class MathQuiz : ContentPage
{
    List<MathTest> questions = new List<MathTest>();
    int questionIndex = 0;
    MathTest question = new MathTest();
    public MathQuiz()
    {
        InitializeComponent();
        getQuestions();

    }
    void OnStart(object sender, EventArgs e)
    {
        SetQuestion();
        this.StartButton.IsVisible = false;
    }
    void SetQuestion()
    {
        this.question = this.questions[questionIndex];
        this.OptionContainer.IsVisible = false;
        this.OptionContainer.IsVisible = true;
        this.questionText.Text = this.question.Question;
        this.OptionOne.Text = this.question.Options[0].ToString();
        this.OptionTwo.Text = this.question.Options[1].ToString();
        this.OptionThree.Text = this.question.Options[2].ToString();
        this.NextButton.IsVisible = false;
        this.SkipButton.IsVisible = false;
        this.RetryButton.IsVisible = false;
        this.OptionOne.IsEnabled = true;
        this.OptionTwo.IsEnabled = true;
        this.OptionThree.IsEnabled = true;
        this.QuestionStatus.Text = "";
        var blue = Color.Parse("CornflowerBlue");
        this.OptionOne.BackgroundColor = blue;
        this.OptionThree.BackgroundColor = blue;
        this.OptionTwo.BackgroundColor = blue;

    }
    void OnNext(object sender, EventArgs e)
    {
        questionIndex++;
        SetQuestion();

    }
    async void OnStartAgain(object sender, EventArgs e)
    {
        await getQuestions();
        this.questionIndex = 0;
        OnStart(null, null);
        this.StartAgain.IsVisible = false;
        this.QuestionStatus.Text = "";

    }
    void OptionSelected(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        if (Int32.Parse(button.Text) == this.question.Result)
        {
            button.BackgroundColor = Color.Parse("ForestGreen");
            this.QuestionStatus.Text = "Correct!";
            this.QuestionStatus.TextColor = Color.Parse("ForestGreen");
            this.NextButton.IsVisible = questionIndex < this.questions.Count() - 1;
            this.SubmitButton.IsVisible = questionIndex == this.questions.Count() - 1;
            this.SkipButton.IsVisible = false;

        }
        else
        {
            button.BackgroundColor = Color.Parse("Red");
            this.QuestionStatus.Text = "InCorrect!";
            this.QuestionStatus.TextColor = Color.Parse("Red");
            this.SkipButton.IsVisible = true;
            this.RetryButton.IsVisible = true;
        }
        this.OptionOne.IsEnabled = false;
        this.OptionTwo.IsEnabled = false;
        this.OptionThree.IsEnabled = false;
    }
    void OnRetry(object sender, EventArgs e)
    {
        this.OptionOne.IsEnabled = true;
        this.OptionTwo.IsEnabled = true;
        this.OptionThree.IsEnabled = true;
        this.QuestionStatus.Text = "";
        this.RetryButton.IsVisible = false;
    }
    void OnSubmit(object sender, EventArgs e)
    {
        this.OptionContainer.IsVisible = true;
        this.questionText.Text = "Successfully Completed";
        this.OptionContainer.IsVisible = false;
        this.SubmitButton.IsVisible = false;
        this.StartAgain.IsVisible = true;
        this.QuestionStatus.Text = "";
    }
    private async Task<List<MathTest>> getQuestions()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7284/Questions");
        HttpResponseMessage response = await client.GetAsync("");
        if (response.IsSuccessStatusCode)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            this.questions = JsonConvert.DeserializeObject<List<MathTest>>(content);
        }
        return this.questions;
    }
    void GoToCalculator(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("MainPage");
    }
}
