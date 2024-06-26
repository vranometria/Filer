﻿using Filer.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace Filer
{
    public static class Utils
    {
        public static readonly SolidColorBrush Background = new(Color.FromRgb(67, 67, 68));

        public static List<string> GetObjects(string directoryPath)
        {             
            var objects = new List<string>();
            foreach (var file in Directory.GetFiles(directoryPath)) { objects.Add(file); }
            foreach (var dir in Directory.GetDirectories(directoryPath)) { objects.Add(dir); }
            return objects;
        }

        public static bool IsObjectExists(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        public static void MoveObject(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath)) { File.Move(sourcePath, destinationPath); }
            else if (Directory.Exists(sourcePath)) 
            {
                MoveDirectory(sourcePath, destinationPath);
            }
        }

        public static void DeleteObject(string path)
        {
            if (File.Exists(path)) { File.Delete(path); }
            else if (Directory.Exists(path)) { Directory.Delete(path, true); }
        }

        public static void Execute(string filePath)
        {
            try
            {
                ProcessStartInfo processStartInfo = new(filePath) { UseShellExecute = true };
                Process.Start(processStartInfo);
            }
            catch (Win32Exception win32e) { 
                MessageBox.Show(win32e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void CopyDirectory(string sourceDir, string destinationDir)
        {
            // 移動先のディレクトリが存在しない場合は作成
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // ファイルをコピー
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, true); // trueは既存ファイルの上書きを許可
            }

            // サブディレクトリを再帰的にコピー
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destinationDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, destSubDir);
            }
        }

        public static void MoveDirectory(string sourceDir, string destinationDir)
        {
            CopyDirectory(sourceDir, destinationDir);

            // 移動元のディレクトリを削除
            Directory.Delete(sourceDir, true);
        }

        public static bool IsAlphabetKey(Key key)
        {
            //a-zの場合はtrueを返す
            if (key >= Key.A && key <= Key.Z) { return true; }

            //0-9の場合はtrueを返す
            if (key >= Key.D0 && key <= Key.D9) { return true; }

            // -_の場合はtrueを返す
            if (key == Key.OemMinus || key == Key.OemPeriod) { return true; }

            return false;
        }

        public static bool IsNumberKey(Key key)
        {
            //0-9の場合はtrueを返す
            if (key >= Key.D0 && key <= Key.D9) { return true; }

            return false;
        }

        public static string GetNumberKeyString(Key key)
        {
            if (key >= Key.D0 && key <= Key.D9) { return key.ToString().Substring(1); }
            return "";
        }

        public static ModifierKeys GetModifireKey(bool isControl, bool isAlt, bool isShift)
        {
            ModifierKeys modifierKeys =  ModifierKeys.None;
            if (isControl) { modifierKeys |= ModifierKeys.Control; }
            if (isAlt) { modifierKeys |= ModifierKeys.Alt; }
            if (isShift) { modifierKeys |= ModifierKeys.Shift; }
            return modifierKeys;
        }
    }
}
