using BetterConsole.IPC;
using BetterConsole.ViewModel;

namespace BetterConsole;

public partial class LogWindow : ContentPage
{
	public LogWindow()
	{
		InitializeComponent();
		var viewModel = new LogWindowModel();
		BindingContext = viewModel;
		Server.Instance.Initialize(viewModel);
	}
}