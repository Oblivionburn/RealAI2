using RealAI.Util;

namespace RealAI;

public partial class App : Application
{
    public static Window Window { get; private set; }

    public App()
	{
		InitializeComponent();

        MainPage = new AppShell();
	}
}
