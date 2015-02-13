using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using FileZipper.Models;
using CsvHelper;

namespace FileZipper
{
    /// <summary>
    /// Takes in a url and gives back the downloaded document.
    /// </summary>
    /// 
    /// handle one file from local directory
    /// handle multiple files from one directory
    /// detect and handle when properly formatted csv comes in 
    /// detect and handle any regular csv.
    /// 
    public class DocumentClient
    {
        private readonly WebClient _wc;

        public DocumentClient()
        {
            _wc = new WebClient
            {
                UseDefaultCredentials = true
            };
        }

        public List<Document> GetDocuments(IEnumerable<string> filePaths)
        {
            var documents = new List<Document>();

            foreach(var path in filePaths)
            {
                // Evaluate all csv's to see if they are the custom formatted one containing
                // uris to Sharepoint, if not just add document to collection.
                if(GetFileNameFromPath(path).EndsWith(".csv"))
                {
                    // If we detect our custom formatted csv, then there will be uris in list
                    var uris = ParseDocumentForUris(path);
                    if(uris.Any())
                    {
                        foreach(var uri in uris)
                        {
                            var doc = DownloadDocument(uri);
                            if(doc != null)
                            {
                                documents.Add(doc);
                            }
                        }
                    }
                    // If the document is special, add it anyways, so someone could always repull
                    // If it's not special we need to add it anyways.
                    documents.Add(GenerateDocument(path));
                }
                // Not a csv, add document to zip collection.
                else
                {
                    documents.Add(GenerateDocument(path));
                }
            }
            return documents;
        }

        private Document DownloadDocument(Uri uri)
        {
            var filename = GetFileNameFromUri(uri);
            if(string.IsNullOrEmpty(filename)) return null;
            
            return new Document
            {
                Bytes = _wc.DownloadData(uri),
                Filename = filename
            };
        }

        private string GetFileNameFromUri(Uri uri)
        {
            if(uri.IsFile)
            {
                return Path.GetFileName(uri.LocalPath);
            }
            return null;
        }

        private List<Uri> ParseDocumentForUris(string path)
        {
            var urls = new List<Uri>();
            var csv = new CsvReader(new CsvParser(new StreamReader(new MemoryStream(File.ReadAllBytes(path)))));

            while (csv.Read())
            {
                var record = csv.GetRecord<dynamic>();
                if (record.Url != null)
                {
                    urls.Add(new Uri(record.Url));
                }
            }

            return urls;
        }

        private Document GenerateDocument(string path)
        {
            return new Document
            {
                Filename = GetFileNameFromPath(path),
                Bytes = GetLocalFileContents(path)
            };
        }

        private byte[] GetLocalFileContents(string path)
        {
            return File.ReadAllBytes(path);
        }

        private string GetFileNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
