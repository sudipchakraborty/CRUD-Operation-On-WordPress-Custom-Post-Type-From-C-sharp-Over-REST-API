
using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;

using Newtonsoft.Json;

namespace Tools
{
    public class WordPress_post
    {
        public string route = "http://bppost.local/wp-json/bp/v2";
      
        
        public string table = "wp_bp2";
        public string Read_All = "select * from wp_bp2";

        public WordPress_post()
        {   
            
        }

        public bp_data Get_Create_Table_obj()
        {
            string createTableQuery =
              "CREATE TABLE IF NOT EXISTS "+table+"( "+
              "id mediumint(11) NOT NULL AUTO_INCREMENT, "+
              "User_ID varchar(10) NOT NULL, "+
              "DateTime datetime NOT NULL, "+
              "SIS INT(3) unsigned NOT NULL, "+
              "DIA INT(3) unsigned NOT NULL, "+
              "PUL INT(3) unsigned NOT NULL, "+
              "PRIMARY KEY id(id) )";

            bp_data bp_Data = new bp_data();
            bp_Data.cmd="create";
            bp_Data.cmd_string=createTableQuery;
            return bp_Data;
        }

        public bp_data Get_Connect_obj()
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="connect";
            return bp_Data;
        }

        public bp_data Get_Insert_String(string id, string sys, string dia, string pul)
        {              
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="insert";
            bp_Data.cmd_string="<p>"+"User_ID="+id+","+"SYS="+sys+","+"DIA="+dia+","+"PUL="+pul+"</p>";
            return bp_Data;
        }

        public bp_data Get_Read_obj()
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="read";


            bp_Data.cmd_string="SELECT* FROM `wp_posts` WHERE `post_title` = 'bp' AND `post_status` = 'publish'";

            //bp_Data.cmd_string="SELECT wp_posts.* "+
            //    " FROM wp_posts"+
            //    " JOIN wp_term_relationships ON(wp_posts.ID = wp_term_relationships.object_id)"+
            //    " JOIN wp_term_taxonomy ON(wp_term_relationships.term_taxonomy_id = wp_term_taxonomy.term_taxonomy_id)"+
            //    " JOIN wp_terms ON(wp_term_taxonomy.term_id = wp_terms.term_id)"+
            //    " WHERE wp_posts.post_type = 'post'"+
            //    " AND wp_posts.post_status = 'publish'"+
            //    " AND wp_terms.name = 'bp_data'";
            return bp_Data;
        }

        public bp_data Get_Delete_row_obj(string id)
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="delete_row";
            bp_Data.tbl_name=table;
            bp_Data.id=id;

            return bp_Data;
        }

        public bp_data Get_Delete_table_obj(string id)
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="delete_table";
            bp_Data.id = id;
            return bp_Data;
        }

        public object Get_Update_data(string Record_ID, string User_ID, string sys, string dia, string pul)
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="update";
            bp_Data.id = Record_ID;
            bp_Data.cmd_string="<p>"+"User_ID="+User_ID+","+"SYS="+sys+","+"DIA="+dia+","+"PUL="+pul+"</p>";
            return bp_Data;
        }


        public void Fill_DataGrid(String data_string, DataGridView dg)
        {
            dynamic jsonDe = JsonConvert.DeserializeObject(data_string);
            int i = 0;
            dg.Rows.Clear();

            foreach (var data in jsonDe)
            {
                string id = data.ID;

                string content = data.post_content;

                string User_ID = Get_String(content, "User_ID=", ",");
                string SYS = Get_String(content, "SYS=", ",");
                string DIA = Get_String(content, "DIA=", ",");
                string PUL = Get_String(content, "PUL=", "<");

                string dt = data.post_date;

                dg.Rows.Add();
                dg[0, i].Value=data.ID;
                dg[1, i].Value= User_ID;
                dg[2, i].Value=dt;
                dg[3, i].Value=SYS;
                dg[4, i].Value=DIA;
                dg[5, i].Value=PUL;
                i++;
            }
        }



        public string Get_String(string src,string str_find, string str_End)
        {
            int idx=src.IndexOf(str_find);
            if (idx==-1) return "";
            string s=src.Substring(idx+str_find.Length);
            idx=s.IndexOf(str_End);
            if (idx==-1) return "";
            s=s.Substring(0,idx);
            return s;
        }






    }
}
