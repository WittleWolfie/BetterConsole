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
      Server.Instance.Initialize(ViewModel);
    }

    private void OnStopService(object sender, EventArgs e)
    {
      Server.Instance.Dispose();
    }
  }
}