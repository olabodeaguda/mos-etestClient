using MOSEtestClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MOSEtestClient.Dao
{
    public class QuestionDao
    {
        private static void SQLiteAdaptor(DataSet dt, SQLiteCommand cmd)
        {
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
            {
                DataTable dt_mReport = new DataTable();
                da.Fill(dt_mReport);
                dt.Tables.Add(dt_mReport);
            }
        }

        private string getURl(string img_name)
        {
            string nm = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "images", img_name + ".jpg");

            if (!File.Exists(nm))
            {
                nm = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "images", "noimage.png");
            }
            return nm;
        }

        public List<QstnOption> OptionBy_QId(List<Guid> qIds)
        {
            string qId = "";
            for (int i = 0; i < qIds.Count; i++)
            {
                if (i == (qIds.Count - 1))
                {
                    qId = qId + "'" + qIds[i] + "'";
                }
                else
                {
                    qId = qId + "'" + qIds[i] + "',";
                }
            }
            List<QstnOption> lst = new List<QstnOption>();
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = $"select * from questionOption where question_Id in({qId})";
                cmd.CommandType = CommandType.Text;
                SQLiteAdaptor(dt, cmd);

                foreach (var x in dt.Tables[0].Rows.Cast<DataRow>())
                {
                    QstnOption qo = new QstnOption();
                    qo.Id = int.Parse(x["id"].ToString());
                    qo.question_Id = Guid.Parse(x["question_id"].ToString());
                    qo.option_name = x["option_name"].ToString();
                    qo.isPicture = int.Parse(x["isPicture"].ToString());
                    qo.picture_url = x["picture_url"].ToString();
                    qo.optionType = x["optionType"].ToString();
                    qo.pictureFullUrl = getURl(x["picture_url"].ToString());
                    qo.picVisible = int.Parse(x["isPicture"].ToString()) == 1 || int.Parse(x["isPicture"].ToString()) == 3 ? Visibility.Visible : Visibility.Collapsed;
                    qo.txVisible = int.Parse(x["isPicture"].ToString()) == 0 || int.Parse(x["isPicture"].ToString()) == 3 ? Visibility.Visible : Visibility.Collapsed;
                    //qo.chk_ans = qo.optionType == ans ? true : false;
                    lst.Add(qo);
                }

            }
            return lst.OrderBy(x => x.optionType).ToList();
        }

        public List<Question> Questions(int count)
        {
            List<Question> lst = new List<Question>();

            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from question where mode = 1 order by random() limit " + count;
                cmd.CommandType = CommandType.Text;
                SQLiteAdaptor(dt, cmd);

                List<string> ans = new List<string>() { "A", "B", "C", "D" };

                var questns = dt.Tables[0].Rows.Cast<DataRow>();
                List<Guid> lstId = questns.Select(x => Guid.Parse(x["id"].ToString())).ToList();
                List<QstnOption> options = OptionBy_QId(lstId);
                foreach (var x in questns)
                {
                    Question q = new Question();
                    q.id = Guid.Parse(x["id"].ToString());
                    q.subject_id = int.Parse(x["subject_id"].ToString());
                    q.question_name = x["question_name"].ToString();
                    q.isPicture = int.Parse(x["isPicture"].ToString());
                    q.pictureUrl = getURl(x["pictureUrl"].ToString());
                    q.picture_name = x["pictureUrl"].ToString();
                    q._ispicture = int.Parse(x["isPicture"].ToString()) == 0 ? "False" : "True";
                    q.answer = string.IsNullOrEmpty(x["answer"].ToString()) ? "1" : x["answer"].ToString();
                    q.explanation = x["explanation"].ToString();
                    q.pictureVisible = int.Parse(x["isPicture"].ToString()) == 1 || int.Parse(x["isPicture"].ToString()) == 3 ? Visibility.Visible : Visibility.Collapsed;
                    q.txtVisible = Visibility.Visible; //int.Parse(x["isPicture"].ToString()) == 0 || int.Parse(x["isPicture"].ToString()) == 3 ? Visibility.Visible : Visibility.Collapsed;
                    q.answerTranslation = string.Format("Answer is {0}", ans[int.Parse(q.answer) - 1]);
                    q.questionoption = options.Where(p => p.question_Id == q.id).ToList();
                    lst.Add(q);
                }
            }
            return lst;
        }

        public bool updateBackUp(ResultBackUp rb)
        {
            bool result = false;

            string query = "insert into resultBackUp(username,dateCreated,content) values(";
            query = query + "'" + rb.username + "',";
            query = query + "'" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "',";
            query = query + "'" + rb.content + "'";
            query = query + ")";
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public SubmitModel getArchiveData(string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = $"select * from resultBackUp where username ='{username}'";
                cmd.CommandType = CommandType.Text;
                SQLiteAdaptor(dt, cmd);
                var t = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ResultBackUp()
                {
                    content = x["content"].ToString(),
                    datecreated = x["dateCreated"].ToString(),
                    username = x["username"].ToString()
                }).FirstOrDefault();

                if (t != null)
                {
                    string nm = t.content;
                    byte[] b = Convert.FromBase64String(t.content);
                    string cont = Encoding.ASCII.GetString(b);
                    SubmitModel r = JsonConvert.DeserializeObject<SubmitModel>(cont);
                    return r;
                }
            }
            return null;
        }

        public bool deleteArchiveData(string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = $"delete from resultBackUp where username ='{username}'";
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
