using BetterConsole.IPC;

namespace BetterConsole
{
  public partial class MainPage : ContentPage
  {
    public ContentViewModel ViewModel => (ContentViewModel)BindingContext;

    public MainPage()
    {
      InitializeComponent();
      BindingContext = new ContentViewModel();
    }

    private void OnStartService(object sender, EventArgs e)
    {
      ServiceHost.Run(this);
    }

    private void OnStopService(object sender, EventArgs e)
    {
      ServiceHost.Abort();
    }
  }
}