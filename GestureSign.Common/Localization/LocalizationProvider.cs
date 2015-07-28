﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using GestureSign.Common.Configuration;

namespace GestureSign.Common.Localization
{
    public class LocalizationProvider
    {
        private string _resource;
        private Dictionary<string, string> _texts = new Dictionary<string, string>(10);
        private FlowDirection _flowDirection;
        private FontFamily _font;
        private static LocalizationProvider _instance;
        internal LocalizationProvider()
        {
        }

        public bool HasData
        {
            get { return _texts.Count != 0; }
        }

        public FlowDirection FlowDirection
        {
            get { return _flowDirection; }
        }

        public static LocalizationProvider Instance
        {
            get { return _instance ?? (_instance = new LocalizationProvider()); }
        }

        public FontFamily Font
        {
            get { return _font; }
        }

        public Dictionary<string, string> GetLanguageList(string languageFolderName)
        {
            var languageList = new Dictionary<string, string>(2) { { "Built-in", "English (Built-in)" } };
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", languageFolderName);
            if (!Directory.Exists(folderPath)) return null;
            foreach (string file in Directory.GetFiles(folderPath, "*.xml"))
            {
                using (XmlTextReader xtr = new XmlTextReader(file) { WhitespaceHandling = WhitespaceHandling.None })
                {
                    while (xtr.Read())
                    {
                        if ("language".Equals(xtr.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            string key = xtr.GetAttribute("Culture");
                            if (key != null && !languageList.ContainsKey(key))
                                languageList.Add(key, xtr.GetAttribute("DisplayName"));
                            break;
                        }
                    }
                }
            }
            return languageList;
        }

        public string GetTextValue(string key)
        {
            if (_texts.ContainsKey(key)) return _texts[key];
            if (_resource != null)
                LoadFromResource(_resource);
            return _texts.ContainsKey(key) ? _texts[key] : "";
        }

        public bool LoadFromFile(string languageFolderName, string resource)
        {
            _resource = resource;
            string languageFile = GetLanguageFilePath(languageFolderName);
            if (languageFile == null) return false;
            using (XmlTextReader xtr = new XmlTextReader(languageFile) { WhitespaceHandling = WhitespaceHandling.None })
            {
                LoadLanguageData(xtr);
            }
            return true;
        }

        public void LoadFromResource(string languageResource)
        {
            using (XmlTextReader xtr = new XmlTextReader(languageResource, XmlNodeType.Document, null)
                {
                    WhitespaceHandling = WhitespaceHandling.None
                })
            {
                LoadLanguageData(xtr);
            }
        }

        private string GetLanguageFilePath(string languageFolderName)
        {
            var culture = String.IsNullOrEmpty(AppConfig.CultureName) ? CultureInfo.CurrentUICulture : CultureInfo.CreateSpecificCulture(AppConfig.CultureName);

            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", languageFolderName);
            if (!Directory.Exists(folderPath)) return null;
            foreach (string file in Directory.GetFiles(folderPath, "*.xml"))
            {
                using (XmlTextReader xtr = new XmlTextReader(file) { WhitespaceHandling = WhitespaceHandling.None })
                {
                    while (xtr.Read())
                    {
                        if ("language".Equals(xtr.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            if (culture.Name.Equals(xtr.GetAttribute("Culture"), StringComparison.OrdinalIgnoreCase))
                            {
                                return file;
                            }
                            break;
                        }
                    }
                }
            }
            return null;
        }

        private void LoadLanguageData(XmlTextReader xmlTextReader)
        {
            List<string> nodes = new List<string>(4);
            while (xmlTextReader.Read())
            {
                if (!"language".Equals(xmlTextReader.Name, StringComparison.OrdinalIgnoreCase)) continue;
                while (xmlTextReader.Read())
                {
                    if (xmlTextReader.NodeType == XmlNodeType.Element)
                    {
                        nodes.Add(xmlTextReader.Name);
                    }
                    else if (xmlTextReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (nodes.Count != 0)
                            nodes.RemoveAt(nodes.Count - 1);
                    }
                    else if (xmlTextReader.NodeType == XmlNodeType.Text)
                    {
                        var key = String.Join(".", nodes);
                        if (!_texts.ContainsKey(key))
                            _texts.Add(key, xmlTextReader.Value);
                    }
                }
                break;
            }

            if (_texts.ContainsKey("Font"))
                _font = new FontFamily(_texts["Font"]);
            if (_texts.ContainsKey("IsRightToLeft"))
                _flowDirection = Boolean.Parse(_texts["IsRightToLeft"]) ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }


    }
}
