using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cutdhijkb
{
    public partial class Start : Form
    {
        History history = new History();
        string sus;
        int sussy;
        public string cont;

        private static readonly string supabaseUrl = "https://vqpqciykbjwvzzvtknjg.supabase.co";
        private static readonly string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZxcHFjaXlrYmp3dnp6dnRrbmpnIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQ5Mzg4MjksImV4cCI6MjA5MDUxNDgyOX0.MuZyRSXUjRTM0TaS7fHAa8DIdASv87daNdvEQz5ZfAM";

        public Start()
        {
            InitializeComponent();
        }

        public class MessageItem
        {
            public string Email { get; set; }
            public string Content { get; set; }
            public override string ToString() { return Email; }
        }

        private void Start_Load(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            List<string> email_redflag = new List<string> { "wwvv", "vvww", "wvvw", "wwww", "gmial", "gmalL", "c0m" };
            List<string> content_redflag = new List<string> { "  ", "   ", "http", "expire", "ban", "Ban", "Expire", "hours", "warning", "WARNING", "DANGER", "danger", "unpaid", "leak", "LEAK", "pay" };

            sus = email.Text;
            sussy = 0;
            cont = content.Text;

            WordDictionary Checker = new WordDictionary();
            Checker.DictionaryFile = "en-US.dic";
            Checker.Initialize();

            Spelling Spell = new Spelling();
            Spell.Dictionary = Checker;

            foreach (string word in email_redflag)
            {
                if (sus.IndexOf(word) >= 0)
                    sussy++;
                //idk man
            }

            char[] separators = new char[] { ' ', '\n', '\r', '.', ',', '!', '?' };
            string[] words = cont.Split(separators);

            foreach (string word in words)
            {
                if (!Spell.TestWord(word))
                    sussy++;
                //will add something when after procrastinating 🙏🙏

                foreach (string flag in content_redflag)
                {
                    if (word.IndexOf(flag) >= 0)
                        sussy++;
                    //asdajdsdasjdsadasdj
                }
            }

            MessageBox.Show($"Suspicion Score: {sussy}");

            await SendToSupabase(email.Text, content.Text);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            history = new History();
            if (email.Text == "" || content.Text == "")
            {
                MessageBox.Show("!!Warning!!\nEmail and/or Content Cannot be empty");
            }
            else
            {
                await SendToSupabase(email.Text, content.Text);
                await history.LoadMessages();
                history.Show();
                this.Hide();
            }
        }

        private async Task SendToSupabase(string email, string content)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("apikey", supabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {supabaseKey}");

                    var obj = new { email = email, content = content };
                    var json = JsonSerializer.Serialize(obj);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{supabaseUrl}/rest/v1/messages", data);

                    if (response.IsSuccessStatusCode)
                        MessageBox.Show("Saved to cloud!");
                    else
                        MessageBox.Show($"Error saving to cloud: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send: {ex.Message}");
            }
        }
    }
}