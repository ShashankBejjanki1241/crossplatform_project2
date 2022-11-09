using System.Windows.Input;

namespace Calculator;

public partial class AppShell : Shell
{
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
    public ICommand HelpCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
        BindingContext = this;
    }
    void test(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MathQuiz());
    }
    void RegisterRoutes()
    {
        Routes.Add(nameof(MainPage), typeof(MainPage));
        Routes.Add(nameof(MathQuiz), typeof(MathQuiz));

        foreach (var item in Routes)
        {
            Routing.RegisterRoute(item.Key, item.Value);
        }
    }
}