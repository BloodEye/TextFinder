using Xceed.Wpf.Toolkit;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace TextFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String name;
        public MainWindow()
        {
            InitializeComponent();
            Result.MouseDoubleClick += new MouseButtonEventHandler(Result_dblClick);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "XAML|*.xaml|Текстовий документ|*.txt|Всі файли|*.*";
            dlg.Title = "Оберіть текстовий файл";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {            
                RTB.Document.Blocks.Clear();
                this.name = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
                //Stopwatch sw = Stopwatch.StartNew();
                if (System.IO.Path.GetExtension(dlg.FileName).Equals(".xaml"))
                {
                    TextRange t = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
                    FileStream file = new FileStream(dlg.FileName, FileMode.Open);
                    
                    t.Load(file, System.Windows.DataFormats.XamlPackage);
                    file.Close();
                }
                else
                {
                    var sr = new StreamReader(dlg.FileName, Encoding.Default);
                    string text = sr.ReadToEnd();

                    var document = new FlowDocument();
                    var paragraph = new Paragraph();

                    paragraph.Inlines.Add(text);
                    document.Blocks.Add(paragraph);
                    RTB.Document = document;
                }

                MainFrame.Title = dlg.FileName.ToString();

                QuantityChar.Content = getText().Length.ToString();
                QuantityEndl.Content = CountLinesInString(getText());
            } 
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (RTB == null | getText().Trim().Length == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Спочатку відкрийте файл");
            }
            else
            {
                SaveFileDialog _SD = new SaveFileDialog();
                _SD.Filter = "XAML|*.xaml|Всі файли|*.*";
                _SD.FileName = this.name;
                _SD.Title = "Сберегти Як...";

                Nullable<bool> result = _SD.ShowDialog();

                if (result == true)
                {
                    using (FileStream file = new FileStream(_SD.FileName, FileMode.Create))
                    {
                        TextRange t = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
                        t.Save(file, System.Windows.DataFormats.XamlPackage);
                        file.Close();
                    }
                }
            }            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DockPanel dp = new DockPanel();
            dp.Width = 200;
            dp.Height = 23;
            dp.HorizontalAlignment = HorizontalAlignment.Left;
            dp.VerticalAlignment = VerticalAlignment.Top;

            Button X = new Button();
            X.Height = 23;
            X.Width = 30;
            X.Content = "X";
            X.VerticalAlignment = VerticalAlignment.Top;
            X.HorizontalAlignment = HorizontalAlignment.Left;
            X.Click += X_Click;

            TextBox tb = new TextBox();
            tb.Height = 23;
            tb.Width = 170;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.FontFamily = new FontFamily("Times New Roman");
            tb.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));

            dp.Children.Add(tb);
            dp.Children.Add(X);
            
            SearchGrid.Children.Add(dp);
        }

        private void X_Click(object sender, RoutedEventArgs e)
        {
            Button X = (Button) sender;
            SearchGrid.Children.Remove((DockPanel) X.Parent);
        }

        static long CountLinesInString(string s)
        {
            long count = 1;
            int start = 0;
            while ((start = s.IndexOf('\n', start)) != -1)
            {
                count++;
                start++;
            }
            return count;
        }

        private void Find_click(object sender, RoutedEventArgs e)
        {
            clear();

            if (check_open())
            {
                if(Pattern.Text.Trim().Length > 0)
                {
                    // Create new stopwatch
                    Stopwatch stopwatch = new Stopwatch();

                    // Begin timing
                    stopwatch.Start();
                    List<TextBox> LTb = GetTextBox();
                    List<String> pats = new List<String>();

                    for (int i = 0; i < LTb.Count; i++)
                    {
                        if (LTb[i].Text.Trim() != "")
                        {
                            pats.Add(LTb[i].Text.Trim());
                        }
                    }

                    SearchRK rk = new SearchRK(pats.ToArray(), IgnoreCase.IsChecked.Value);
                    Result[] result = rk.search(getText(), false);

                    if (result != null)
                    {
                        addResult(result);
                        QuantityFinds.Content = result.Length;
                        
                        stopwatch.Stop();
                        TimeSearch.Content = stopwatch.ElapsedMilliseconds.ToString() + "ms";
                    }
                    else
                    {
                        stopwatch.Stop();
                        TimeSearch.Content = stopwatch.ElapsedMilliseconds.ToString() + "ms";
                        Xceed.Wpf.Toolkit.MessageBox.Show("Нічого не знайдено");
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Ви не ввели запит для пошуку");
                }                    
            }
            else 
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Спочатку відкрийте файл");
            }
        }

        private void Clear_click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void clear()
        {
            Result.Items.Clear();
        }

        private bool check_open()
        {
            return getText().Trim().Length > 0 ? true : false;
        }

        private string getText()
        {
            TextRange textRange = new TextRange(
                RTB.Document.ContentStart,
                RTB.Document.ContentEnd
            );

            return textRange.Text;
        }

        private void addResult(Result result) 
        {
            Result.Items.Add(result);
        }

        private void addResult(Result[] results)
        {
            int size = results.Length;            

            for (int i = 0; i < size; i++) {
                Result.Items.Add(results[i]);
            }
        }

        private void ClearBox_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> tbs = GetTextBox();
            foreach (TextBox tb in tbs)
            {
                tb.Text = "";
            }
        }

        private List<TextBox> GetTextBox()
        {
            List<TextBox> lTBox = new List<TextBox>();
            List<DockPanel> dps = SearchGrid.Children.OfType<DockPanel>().ToList();

            lTBox.Add(Pattern);

            foreach(DockPanel dp in dps){
                lTBox.Add(dp.Children.OfType<TextBox>().First());
            }

            return lTBox;
        }

        private void FindAll_Click(object sender, RoutedEventArgs e)
        {
            clear();

            if (check_open())
            {
                if (Pattern.Text.Trim().Length > 0)
                {
                    Stopwatch stopwatch = new Stopwatch();

                    stopwatch.Start();

                    List<TextBox> LTb = GetTextBox();
                    List<String> pats = new List<String>();

                    for (int i = 0; i < LTb.Count; i++)
                    {
                        if (LTb[i].Text.Trim() != "")
                        {
                            pats.Add(LTb[i].Text.Trim());
                        }
                    }

                    SearchRK rk = new SearchRK(pats.ToArray(), IgnoreCase.IsChecked.Value);
                    Result[] result = rk.search(getText(), true);

                    if (result != null)
                    {
                        addResult(result);

                        QuantityFinds.Content = result.Length;
                        stopwatch.Stop();
                        TimeSearch.Content = stopwatch.ElapsedMilliseconds.ToString() + "ms";
                    }
                    else
                    {
                        stopwatch.Stop();
                        TimeSearch.Content = stopwatch.ElapsedMilliseconds.ToString() + "ms";
                        Xceed.Wpf.Toolkit.MessageBox.Show("Нічого не знайдено");
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Ви не ввели запит для пошуку");
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Спочатку відкрийте файл");
            }
        }

        private void Result_dblClick(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType().Name.Equals("ListViewItem")) {
                var item = ((ListViewItem)sender).Content as Result;

                TextPointer start = RTB.Document.ContentStart.GetPositionAtOffset(item.Index + 2, LogicalDirection.Forward);
                TextPointer end = RTB.Document.ContentStart.GetPositionAtOffset(item.Index + item.Length + 2, LogicalDirection.Backward);

                RTB.Focus();                
                RTB.CaretPosition = start;
                RTB.Selection.Select(start, end);  
                RTB.Focus();   
            }
        }

        private void CPMenu_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            if (RTB == null) return;

            TextSelection ts = RTB.Selection;
            if (ts != null)
            {
                if (((ComboBoxItem)CBox.SelectedItem).Name == "_Background"){
                    ts.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(CPMenu.SelectedColor));
                }
                else
                {
                    ts.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(CPMenu.SelectedColor));
                }                    
            } 
     
            RTB.Focus();
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = RTB.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = RTB.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = RTB.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));
        }

        private void btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (RTB == null) return;

            TextSelection ts = RTB.Selection;

            if (ts != null)
            {                
                if ((sender as ToggleButton).IsChecked == true)
                {
                    ts.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
                else
                {
                    ts.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
            }
         }

        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (RTB == null) return;

            TextSelection ts = RTB.Selection;

            if (ts != null)
            {
                if ((sender as ToggleButton).IsChecked == true)
                {
                    ts.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
                }
                else
                {
                    ts.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
                }
            }
        }

        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            if (RTB == null) return;

            TextSelection ts = RTB.Selection;

            if (ts != null)
            {
                if ((sender as ToggleButton).IsChecked == true)
                {
                    ts.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                }
                else
                {
                    ts.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }
    }
}
