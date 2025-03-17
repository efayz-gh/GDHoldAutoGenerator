using System.IO;
using System.Windows;
using GDHoldAutoGenerator;
using Microsoft.Win32;

namespace UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
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
            Filter = "GD Replay Files (*.gdr.json)|*.gdr.json",
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
        if (_replayFile == null && _levelFile == null)
        {
            MessageBox.Show("No files selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

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

        var replay = GDReplay.Deserialize(File.ReadAllText(_replayFile));

        if (replay == null)
        {
            MessageBox.Show("Replay file is invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var levelObjects = levelString.GetObjects(); // for editorLayer and linkId
        levelString.AppendObjects(replay.ToObjects(CheckGenerateClicks.IsChecked == true,
            levelObjects.GetNextFreeEditorLayer(), levelObjects.GetNextFreeLinkId()));

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "GDShare Level Files (*.gmd)|*.gmd",
        };

        if (saveFileDialog.ShowDialog() != true)
            return;

        // copy the original level file if the save file doesn't exist
        if (!File.Exists(saveFileDialog.FileName))
            File.Copy(_levelFile, saveFileDialog.FileName);

        levelString.WriteToGmdFile(saveFileDialog.FileName);

        MessageBox.Show("Level file generated successfully.", "Success", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}