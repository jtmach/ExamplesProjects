namespace TestAsync
{
  using System.ComponentModel;
  using System.Threading.Tasks;
  using System.Windows;

  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
      this.Login.IsEnabled = false;
      this.Login.Content = "Trying to login, please wait";

      Task<bool> loginTask = Task<bool>.Factory.StartNew(() => login());
      await loginTask;

      if (!loginTask.Result)
      {
        MessageBox.Show("This will never work.\nPlease try again.");
      }

      this.Login.IsEnabled = true;
      this.Login.Content = "Login";
    }

    private void LoginBw_Click(object sender, RoutedEventArgs e)
    {
      this.LoginBw.IsEnabled = false;
      this.LoginBw.Content = "Trying to login, please wait";

      BackgroundWorker bw = new BackgroundWorker();
      bw.DoWork += Bw_DoWork;
      bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
      bw.RunWorkerAsync();
    }

    private void Bw_DoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = login();
    }

    private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (!(bool)e.Result)
      {
        MessageBox.Show("This will never work.\nPlease try again.");
      }

      this.LoginBw.IsEnabled = true;
      this.LoginBw.Content = "Login";
    }

    private bool login()
    {
      System.Threading.Thread.Sleep(10000);
      return false;
    }
  }
}
