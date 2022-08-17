using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BetterConsole.ViewModel
{

  public class Monkey
  {
    public string Name { get; }
    public string Location { get; }
    public string ImageUrl { get; }

    public Monkey(string name, string location, string imageUrl)
    {
      Name = name;
      Location = location;
      ImageUrl = imageUrl;
    }
  }

  public class MonkeysViewModel : INotifyPropertyChanged
  {
    Monkey selectedMonkey;

    public ObservableCollection<Monkey> Monkeys = new();

    public Monkey SelectedMonkey
    {
      get
      {
        return selectedMonkey;
      }
      set
      {
        if (selectedMonkey != value)
        {
          selectedMonkey = value;
        }
      }
    }

    public ICommand DeleteCommand => new Command<Monkey>(RemoveMonkey);

    public MonkeysViewModel()
    {
      CreateMonkeyCollection();

      SelectedMonkey = Monkeys.Skip(3).FirstOrDefault();
      OnPropertyChanged("SelectedMonkey");

    }

    void CreateMonkeyCollection()
    {
      Monkeys.Add(
        new Monkey(
          "Baboon",
          "Africa & Asia",
          "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg"));

      Monkeys.Add(
        new Monkey(
          "Capuchin Monkey",
          "Central & South America",
          "https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg"));
    }

    void RemoveMonkey(Monkey monkey)
    {
      if (Monkeys.Contains(monkey))
      {
        Monkeys.Remove(monkey);
      }
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
  }
}
