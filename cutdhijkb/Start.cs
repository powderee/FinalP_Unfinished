using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System.Drawing;

namespace cutdhijkb
{
    public partial class Start : Form
    {
        History history = new History();
        string sus;
        int sussy;
        public string cont;
        string stat;
        string varss = "";
        readonly string supabaseUrl = System.Configuration.ConfigurationManager.AppSettings["SupabaseUrl"];
        readonly string supabaseKey = System.Configuration.ConfigurationManager.AppSettings["SupabaseKey"];
        public Start()
        {
            InitializeComponent();
        }

        public class MessageItem
        {
            public string email { get; set; }
            public string content { get; set; }
            public string status { get; set; }
            public override string ToString() { return email; }
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
            }

            char[] separators = new char[] { ' ', '\n', '\r', '.', ',', '!', '?' };
            string[] words = cont.Split(separators);

            foreach (string word in words)
            {
                if (!Spell.TestWord(word))
                    sussy++;

                foreach (string flag in content_redflag)
                {
                    if (word.IndexOf(flag) >= 0)
                        sussy++;
                }
            }

            //will try to maybe make it a function if i win my next ranked😭🙏

            if (sussy <= 1) 
            {
                stat = "Safe";
                Status.Text = stat;
                panel1.BackColor = Color.Green;
                panel2.BackColor = Color.Green;
                panel3.BackColor = Color.Green;
            }


            else if (sussy <= 3)
            {
                stat = "Caution";
                Status.Text = stat;
                panel1.BackColor = Color.Red;
                panel2.BackColor = Color.Green;
                panel3.BackColor = Color.Green;

            }

            else if (sussy <= 7)
            {
                stat = "Highly Suspisious";
                Status.Text = stat;
                panel1.BackColor = Color.Red;
                panel2.BackColor = Color.Red;
                panel3.BackColor = Color.Green;
            }
            else
            {
                stat = "!!WARNING AVOID!!";
                Status.Text = stat;
                panel1.BackColor = Color.Red;
                panel2.BackColor = Color.Red;
                panel3.BackColor = Color.Red;
            }
            MessageBox.Show($"Suspicion Score: {sussy}\nThe verdict is {stat}");
            varss = stat;
            //removed SendToSupabase in testsssssss
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
                await SendToSupabase(email.Text, content.Text , varss);
                await history.LoadMessages();
                history.Show();
                this.Hide();
            }
        }

        private async Task SendToSupabase(string email, string content , string status)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("apikey", supabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {supabaseKey}");

                    var obj = new { email = email, content = content , status = stat};
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

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}