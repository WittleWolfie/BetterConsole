using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
  public class ContentViewModel : BindableObject
  {
    private string _status;
    public string Status
    {
      get => _status;
      set
      {
        _status = value;
        OnPropertyChanged();
      }
    }

    private string _message;
    public string Message
    {
      get => _message;
      set
      {
        _message = value;
        OnPropertyChanged();
      }
    }
  }
}
