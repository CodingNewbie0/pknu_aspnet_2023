using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using TodoItemApp.Models;
using System.Net.Http.Headers;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Diagnostics;

namespace TodoItemApp
{
    public class DivCode
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private List<DivCode> divCodes = new List<DivCode>();
        HttpClient client = new HttpClient();
        TodoItemsCollection todoItems = new TodoItemsCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            divCodes.Add(new DivCode { Key = "True", Value = "1" });
            divCodes.Add(new DivCode { Key = "False", Value = "0" });
            CboIsComplete.ItemsSource = divCodes;
            CboIsComplete.DisplayMemberPath = "Key"; // 콤보박스에 True/False 추가

            // yyy-MM-dd HH:mm:ss 날짜 포멧(오전/오후 포함)
            // ko -> yyyy.M.d H:m:ss / ko-KR -> yyy-MM-dd HH:mm:ss
            DtpTodoDate.Culture = new CultureInfo("ko-KR");

            // RestAPI 호출 시작 // 아래 두 문장은 다시 호출하면 안됨
            client.BaseAddress = new System.Uri("https://localhost:7202/"); // RestAPI 서버 기본 URL
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetData(); // 데이터 로드 메서드 호출
        }

        // RestAPI Get Method 호출
        private async void GetData()
        {
            GrdTodoItems.ItemsSource = todoItems; // 미리 바인딩

            try // API 호출
            {
                // https://localhost:7202/api/TodoItems
                HttpResponseMessage? response = await client.GetAsync("api/TodoItems"); // GET method 비동기 호출
                response.EnsureSuccessStatusCode(); // 에러가 났으면 오류코드를 던진다(예외발생)

                // 응답에서 List<TodoItem> 형식으로 읽어옴
                var items = await response.Content.ReadAsAsync<IEnumerable<TodoItem>>();
                todoItems.CopyFrom(items);
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true

                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            try // API 호출
            {
                var todoItem = new TodoItem()
                {
                    Id = 0,
                    Title = TxtTitle.Text,
                    TodoDate = ((DateTime)DtpTodoDate.SelectedDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    IsComplete = Int32.Parse((CboIsComplete.SelectedItem as DivCode).Value)
                };

                var response = await client.PostAsJsonAsync("api/TodoItems", todoItem);
                response.EnsureSuccessStatusCode();

                GetData();

                TxtId.Text = TxtTitle.Text = string.Empty;
                CboIsComplete.SelectedIndex = -1;
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true

                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try // API 호출
            {
                var todoItem = new TodoItem()
                {
                    Id = Int32.Parse(TxtId.Text),
                    Title = TxtTitle.Text,
                    TodoDate = ((DateTime)DtpTodoDate.SelectedDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    IsComplete = Int32.Parse((CboIsComplete.SelectedItem as DivCode).Value)
                };

                // Update할때는 Put 메서드
                var response = await client.PutAsJsonAsync($"api/TodoItems/{todoItem.Id}", todoItem);
                response.EnsureSuccessStatusCode();

                GetData();

                TxtId.Text = TxtTitle.Text = string.Empty;
                CboIsComplete.SelectedIndex = -1;
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true

                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try // API 호출
            {
                var Id = Int32.Parse(TxtId.Text); // 삭제는 Id만 보내면 됨

                // Update할때는 Put 메서드
                var response = await client.DeleteAsync($"api/TodoItems/{Id}");
                response.EnsureSuccessStatusCode();

                GetData();

                TxtId.Text = TxtTitle.Text = string.Empty;
                CboIsComplete.SelectedIndex = -1;
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true

                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void GrdTodoItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Debug.WriteLine(GrdTodoItems.SelectedIndex.ToString());
            
            try // API 호출
            {
                var Id = ((TodoItem)GrdTodoItems.SelectedItem).Id;
                Debug.WriteLine(Id);

                // https://localhost:7202/api/TodoItems
                HttpResponseMessage? response = await client.GetAsync($"api/TodoItems/{Id}"); // GET method 비동기 호출
                response.EnsureSuccessStatusCode(); // 에러가 났으면 오류코드를 던진다(예외발생)

                // 응답에서 List<TodoItem> 형식으로 읽어옴
                var item = await response.Content.ReadAsAsync<TodoItem>();
                Debug.WriteLine(item.Title);

                TxtId.Text = item.Id.ToString();
                TxtTitle.Text = item.Title;
                DtpTodoDate.SelectedDateTime = DateTime.Parse(item.TodoDate);
                CboIsComplete.SelectedIndex = item.IsComplete == 1 ? 0: 1;
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true

                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"이외 예외 {ex.Message}");
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }
    }
}
