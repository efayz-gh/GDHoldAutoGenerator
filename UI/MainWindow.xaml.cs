using System.IO;
using System.Text.Json;
using System.Windows;
using GDHoldAutoGenerator;
using Microsoft.Win32;

namespace UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string? _levelFile;
    private string? _replayFile;

    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void ButtonBrowseLvl_OnClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "GDShare Level Files (*.gmd)|*.gmd",
            Title = "Select a GDShare level file"
        };

        if (openFileDialog.ShowDialog() != true) 
            return;
        
        if (openFileDialog.CheckFileExists && openFileDialog.FileName.EndsWith(".gmd"))
        {
            _levelFile = openFileDialog.FileName;
            BoxBrowseLvl.Text = openFileDialog.SafeFileName;
        }
        else
        {
            MessageBox.Show("Invalid file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ButtonBrowseReplay_OnClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Geometry Dash Replay Files (*.gdr.json)|*.gdr.json",
            Title = "Select a Geometry Dash replay file"
        };

        if (openFileDialog.ShowDialog() != true) 
            return;
        
        if (openFileDialog.CheckFileExists && openFileDialog.FileName.EndsWith(".gdr.json"))
        {
            _replayFile = openFileDialog.FileName;
            BoxBrowseReplay.Text = openFileDialog.SafeFileName;
        }
        else
        {
            MessageBox.Show("Invalid file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ButtonGenerate_OnClick(object sender, RoutedEventArgs e)
    {
        if (_levelFile == null)
        {
            MessageBox.Show("No level file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (_replayFile == null)
        {
            MessageBox.Show("No replay file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        var levelString = new LevelString();
        levelString.LoadFromGmdFile(_levelFile);
        
        var replay = JsonSerializer.Deserialize<GDReplay>(
            File.ReadAllText(_replayFile)
        );
            
        if (replay == null)
        {
            MessageBox.Show("Replay file is invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        levelString.AppendObjects(replay.ToObjects(CheckGenerateClicks.IsChecked == true));
        
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "GDShare Level Files (*.gmd)|*.gmd",
        };

        if (saveFileDialog.ShowDialog() != true)
            return;
        
        if (!File.Exists(_levelFile))
            File.Copy(BoxBrowseLvl.Text, saveFileDialog.FileName);
        
        levelString.WriteToGmdFile(saveFileDialog.FileName);
        
        MessageBox.Show("Level file generated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}