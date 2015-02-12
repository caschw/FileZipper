using System;
using System.IO;
using System.Linq;
using FileZipper.Helpers;
using FileZipper.Models;
using Microsoft.Win32;

namespace FileZipper.ViewModels
{
    public class FileZipperViewModel : BaseViewModel
    {

        #region << Initilization/Constructors >>

        public FileZipperViewModel()
        {
            Docs = new SmartObservableCollection<Document>();
        }

        #endregion

        #region << Properties >>

        private SmartObservableCollection<Document> _docs;
        public SmartObservableCollection<Document> Docs
        {
            get { return _docs; }
            set
            {
                _docs = value;
                OnPropertyChanged( "Docs" );
            }
        }

        private Document _selectedDoc;
        public Document SelectedDoc
        {
            get { return _selectedDoc; }
            set
            {
                _selectedDoc = value;
                OnPropertyChanged( "SelectedDoc" );
            }
        }

        #endregion

        #region << Public Methods >>

        /// <summary>
        /// When files are dropped into the textblock this is triggered.
        /// </summary>
        /// <param name="filePaths"> </param>
        public void OnFileDrop(string[] filePaths)
        {
            var client = new DocumentClient();
            Docs.AddItems( client.GetDocuments( filePaths ) );
        }

        /// <summary>
        /// Browse button click.
        /// </summary>
        public void OnBrowse()
        {
            //Open file selection dialog to default local
            var dialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Multiselect = true,
                Filter = "All Files|*.*"
            };
            //Check for result files.
            if(dialog.ShowDialog() == true)
            {
                if(dialog.FileNames.Any())
                {
                    var client = new DocumentClient();
                    Docs.AddItems( client.GetDocuments( dialog.FileNames ) );
                }
            }
        }

        /// <summary>
        /// Zip button click.
        /// </summary>
        public void OnZip()
        {
            if(Docs.Any())
            {
                //Create Zip File
                var zipFile = new ZipClient().ZipFiles( Docs );

                //Configure File dialog box
                var dialog = new SaveFileDialog
                {
                    FileName = "Documents",
                    DefaultExt = ".zip",
                    Filter = "All Files|*.*",
                    InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments )
                };
                //Show File Dialog Box
                if(dialog.ShowDialog() == true)
                {
                    //Save zip file to location.
                    File.WriteAllBytes( dialog.FileName, zipFile );
                }
            }
        }

        /// <summary>
        /// Remove Documents
        /// </summary>
        public void RemoveFile()
        {
            if(SelectedDoc != null)
            {
                if(Docs.Any( d => d.Filename == SelectedDoc.Filename ))
                {
                    Docs.Remove( SelectedDoc );
                }
            }
        }

        #endregion

    }
}
