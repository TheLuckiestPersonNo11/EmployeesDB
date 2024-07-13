using MainProject25.Model;
using MainProject25.MVVM;
using Microsoft.Win32;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
namespace MainProject25.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private static readonly Random random = new Random();

        public ObservableCollection<Item> Items { get; set; }

        public RelayCommand AddCommand => new RelayCommand(execute => AddItem());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteItem(), canExecute => SelectedItem != null);
        public RelayCommand SaveFileCommand => new RelayCommand(execute => SaveFileExcel());

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<Item>();
        }

        private Item selectedItem;

        public Item SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        private void AddItem()
        {
            Items.Add(new Item
            {
                Name = "(введіть ім'я та прізвище)",
                Age = 0,
                Identifier = GenerateRandomString(16),
            });
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "0123456789";
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return stringBuilder.ToString();
        }

        private void DeleteItem()
        {
            Items.Remove(SelectedItem);
        }

        private void SaveFileExcel()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xls;*.xlsx;*.xlsm)|*.xls;*.xlsx;*.xlsm";
            saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
            saveFileDialog.FileName = $"Dataset_{DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx";

            bool? dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                string path = saveFileDialog.FileName;
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("Sheet1");

                IFont font = workbook.CreateFont();
                font.FontName = "Calibri";
                font.IsBold = true;
                font.FontHeightInPoints = 11;
                font.Color = HSSFColor.Black.Index;

                XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                cellStyle.SetFont(font);

                string[] headers = { "Ім'я та прізвище співробітника", "Вік співробітника" };

                IRow row = sheet1.CreateRow(0);
                row.RowStyle = cellStyle;
                for (int i = 0; i < headers.Length; i++)
                {
                    row.CreateCell(i).SetCellValue(headers[i]);
                }

                int index = 1;
                foreach (Item item in Items)
                {
                    row = sheet1.CreateRow(index);
                    row.CreateCell(0).SetCellValue(item.Name.ToString());
                    row.CreateCell(1).SetCellValue(item.Age.ToString());
                    row.CreateCell(2).SetCellValue(item.Identifier.ToString());
                    index++;
                }

                for (int i = 0; i < headers.Length; i++)
                {
                    sheet1.AutoSizeColumn(i);
                }

                using (FileStream sw = File.Create(path))
                {
                    workbook.Write(sw);
                    FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);

                    System.Windows.MessageBox.Show($"Файл \"{fileInfo.Name}\" збережено успішно!");
                }
            }
        }
    }
}
