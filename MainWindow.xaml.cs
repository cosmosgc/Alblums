using Alblums.Class;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Path = System.IO.Path;

namespace Alblums
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<gal> galeria = new List<gal>();
        public List<gal> targetGaleria = new List<gal>();
        public string SourceDirectoryPath;
        int selectedImage = 0;
        int perPage = 10;
        int page = 1;
        int TargetPage = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectGalleryFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Por favor escolha uma pasta.";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            {
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            }

            if ((bool)dialog.ShowDialog(this))
            {
                galeria.Clear();
                targetGaleria.Clear();
                SourceDirectoryPath = dialog.SelectedPath;
                string[] fileEntries = Directory.GetFiles(dialog.SelectedPath);
                foreach (string fileName in fileEntries)
                {
                    string ext = Path.GetExtension(fileName);
                    if (ext == ".jpg" || ext == ".png")
                    {
                        galeria.Add(new gal("Image", 22.22, fileName));
                    }
                }
                initSourceGallery();
            }
        }
        private string SelectFolder(string description = "Por favor escolha uma pasta")
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = description;
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            {
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            }

            if ((bool)dialog.ShowDialog(this))
            {
                return dialog.SelectedPath;
            }
            return null;
        }

        private void initSourceGallery()
        {

            //Force garbage collection.
            GC.Collect();

            // Wait for all finalizers to complete before continuing.
            GC.WaitForPendingFinalizers();
            SourceGallery.ItemsSource = null;
            List<gal> tmpglr = new List<gal>();
            for (int i = (page-1) * perPage; i < (page) * perPage; i++)
            {
                if(i >= galeria.Count)
                {
                    break;
                }
                tmpglr.Add(galeria[i]);
            }
            SourceGallery.ItemsSource = tmpglr;
        }
        private void initTargetGallery()
        {
            TargetGallery.ItemsSource = null;
            List<gal> tmpglr = new List<gal>();
            for (int i = (TargetPage - 1) * perPage; i < (TargetPage) * perPage; i++)
            {
                if (i >= targetGaleria.Count)
                {
                    break;
                }
                tmpglr.Add(targetGaleria[i]);
            }
            TargetGallery.ItemsSource = tmpglr;
        }
        public void nextPage()
        {
            Console.WriteLine("FUNCIONA");
            if (galeria.Count > (page + 1) * perPage)
                page++;
            pageLabel.Content = page;
            initSourceGallery();
        }
        public void prevPage()
        {
            if ((page - 1) * perPage > 0)
                page--;
            pageLabel.Content = page;
            initSourceGallery();
        }
        public void nextPageTarget()
        {
            if (targetGaleria.Count > (TargetPage + 1) * perPage)
                TargetPage++;
            targetPageLabel.Content = TargetPage;
            initTargetGallery();
        }
        public void prevPageTarget()
        {
            if ((TargetPage - 1) * perPage > 0)
                TargetPage--;
            targetPageLabel.Content = TargetPage;
            initTargetGallery();
        }
        private void ItemClick(object sender, RoutedEventArgs e)
        {
            object tag = (sender as Button).Tag;
            string imagePath = (tag as string).ToString();
            Uri imageUri = new Uri(imagePath, UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            ImagePreview.ImageSource = imageBitmap;
            SelectedPath.Text = imagePath;
            getGalleryIndex(imagePath);
        }

        private int getGalleryIndex(string image)
        {
            for(int i = 0; i < galeria.Count; i++)
            {
                if(galeria[i].Image == image)
                {
                    return i;
                }
            }
            return -1;
        }
        private void button1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                object tag = (sender as Button).Tag;
                string imagePath = (tag as string).ToString();
                DragDrop.DoDragDrop(this, imagePath, DragDropEffects.Move);

            }
        }

        private void button1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void targetDropChoose(object sender, DragEventArgs e)
        {
            string obj = e.Data.GetData(typeof(string)) as string;
            Console.WriteLine(obj);
        }

        private void backPage(object sender, RoutedEventArgs e)
        {
            prevPage();
        }

        private void proxPage(object sender, RoutedEventArgs e)
        {
            nextPage();
        }

        private void fixarImage(object sender, RoutedEventArgs e)
        {
            FixedImagePreview.ImageSource = ImagePreview.ImageSource;
            FixedPath.Text = SelectedPath.Text;
        }

        private void AddFixedToChosen(object sender, RoutedEventArgs e)
        {
            int i = getGalleryIndex(FixedPath.Text);
            Console.WriteLine("ACHOU IMAGEM "+i);
            if (i == -1)
            {
                FixedPath.Text = "ERRO";
                return;
            }
            targetGaleria.Add(galeria[i]);
            initTargetGallery();
        }

        private void AddSelectedToChosen(object sender, RoutedEventArgs e)
        {
            int i = getGalleryIndex(SelectedPath.Text);
            if (i == -1)
            {
                SelectedPath.Text = "ERRO";
                return;
            }
            targetGaleria.Add(galeria[i]);
            initTargetGallery();
        }

        private void backPageTarget(object sender, RoutedEventArgs e)
        {
            prevPageTarget();
        }

        private void proxPageTarget(object sender, RoutedEventArgs e)
        {
            nextPageTarget();
        }

        private void SalvarTargetGallery(object sender, RoutedEventArgs e)
        {
            List<gal> tmpglr = new List<gal>();
            
            for(int i = 0; i < targetGaleria.Count; i++)
            {
                string fileName = Path.GetFileName(targetGaleria[i].Image);
                tmpglr.Add(new gal("",0,fileName));
            }
            string targetPath = Path.GetDirectoryName(targetGaleria[0].Image);
            imageGallery saveGal = new imageGallery(tmpglr, targetPath);

            string save = JsonConvert.SerializeObject(saveGal, Formatting.Indented);
            string saveName = "salvo.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Save\", saveName);
            if (!Directory.Exists(path))
            {
                // Try to create the directory.
                string FolderPath = Path.Combine(Environment.CurrentDirectory, @"Save\");
                DirectoryInfo di = Directory.CreateDirectory(FolderPath);
            }
            string DestinyPath = Path.Combine(Environment.CurrentDirectory, @"Save\");
            File.WriteAllText(path, save);
            Process.Start("explorer.exe", DestinyPath);
        }

        private void DropFileToWindow(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                HandleFileOpen(files[0]);
            }
        }

        private void HandleFileOpen(string v)
        {
            FileAttributes attr = File.GetAttributes(v);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                loadSourceFolder(v);
            }
            string type = Path.GetExtension(v);
            if(type == ".json")
            {
                loadTargetJson(v);
            }

        }
        public void loadSourceFolder(string SelectedPath)
        {
            galeria.Clear();
            targetGaleria.Clear();
            SourceDirectoryPath = SelectedPath;
            string[] fileEntries = Directory.GetFiles(SelectedPath);
            foreach (string fileName in fileEntries)
            {
                string ext = Path.GetExtension(fileName);
                if (ext == ".jpg" || ext == ".png")
                {
                    galeria.Add(new gal("Image", 22.22, fileName));
                }
            }
            initSourceGallery();
        }

        public void loadTargetJson(string v)
        {
            imageGallery tmpGallery = new imageGallery(new List<gal>(), v);

            if (SourceDirectoryPath == string.Empty || SourceDirectoryPath == null)
            {
                SelectGalleryFolder(v, new RoutedEventArgs());
            }
            if (SourceDirectoryPath == string.Empty || SourceDirectoryPath == null)
            {
                return;
            }
            string jsonString = File.ReadAllText(v);
            tmpGallery = JsonConvert.DeserializeObject<imageGallery>(jsonString);
            targetGaleria.Clear();

            for (int i = 0; i < tmpGallery.gallery.Count; i++)
            {
                string pathLoad = Path.Combine(SourceDirectoryPath, tmpGallery.gallery[i].Image);
                if (!File.Exists(pathLoad))
                {
                    MessageBox.Show(this, "Um ou mais arquivos não são identicos, pasta errada talvez?");
                    return;
                }
                gal tmpgal = new gal("", 0, pathLoad);
                targetGaleria.Add(tmpgal);
            }
            initTargetGallery();
        }

        private void CopyTargetToAFolder(object sender, RoutedEventArgs e)
        {
            List<FileInfo> files = new List<FileInfo>();
            string destinationDir = SelectFolder("Onde gostaria de salvar as imagens?");
            if (destinationDir == null)
                return;

            for (int i = 0; i < targetGaleria.Count; i++)
            {
                files.Add(new FileInfo(targetGaleria[i].Image));
            }
            foreach (FileInfo file in files)
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                var file2 = new FileInfo(targetFilePath);
                file2.Delete();
                file.CopyTo(targetFilePath);
            }
            Process.Start("explorer.exe", destinationDir);
        }
    }
}
