using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static cutdhijkb.Start;

namespace cutdhijkb
{
    public partial class History : Form
    {
        private static readonly string supabaseUrl = "https://vqpqciykbjwvzzvtknjg.supabase.co";
        private static readonly string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZxcHFjaXlrYmp3dnp6dnRrbmpnIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQ5Mzg4MjksImV4cCI6MjA5MDUxNDgyOX0.MuZyRSXUjRTM0TaS7fHAa8DIdASv87daNdvEQz5ZfAM";

        public History()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();

            DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 150
            };

            DataGridViewTextBoxColumn contentColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Message",
                DataPropertyName = "Content",
                Width = 500,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True }
            };

            dataGridView1.Columns.Add(emailColumn);
            dataGridView1.Columns.Add(contentColumn);
        }

        private async void History_Load(object sender, EventArgs e)
        {
            await LoadMessages();
        }

        public async Task LoadMessages()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("apikey", supabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {supabaseKey}");
                    var response = await client.GetAsync($"{supabaseUrl}/rest/v1/messages?select=*");
                
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var messages = JsonSerializer.Deserialize <List<MessageItem>> (json) ?? new List <MessageItem> ();

                        messages.Reverse();
                        dataGridView1.DataSource = messages;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to load messages: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading messages: {ex.Message}");   
            }
        }
        private void History_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}