
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;

using Newtonsoft.Json;

namespace Tools
{
    public class WordPress_CPT
    {
        public string route = "http://bppost.local/wp-json/bp/v3";

        public WordPress_CPT()
        {   
             
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
            return bp_Data;
        }

        public bp_data Get_Read_obj2(string post_id)
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="read";
            bp_Data.cmd_string= "SELECT * FROM wp_postmeta WHERE post_id = "+post_id;//    "SELECT* FROM `wp_postmeta` WHERE `post_title` = 'bp' AND `post_status` = 'publish'";
            return bp_Data;
        }

        public bp_data Get_Delete_row_obj(string id)
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="delete_row";
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
            bp_Data.cmd_string="update wp_postmeta set meta_value=121 where post_id=95 and meta_key= 'sys'";// "<p>"+"User_ID="+User_ID+","+"SYS="+sys+","+"DIA="+dia+","+"PUL="+pul+"</p>";
            return bp_Data;
        }
         
        public object Get_Update_data2(string table_name, string param, string value)
        {
            bp_data bp_Data = new bp_data(); 
            bp_Data.cmd="update";
            bp_Data.cmd_string="update "+table_name+ " set "+param+" ="+value+" where "+"post_id=95 and meta_key= 'sys'";// "<p>"+"User_ID="+User_ID+","+"SYS="+sys+","+"DIA="+dia+","+"PUL="+pul+"</p>";
            return bp_Data;
        }

        public object Get_Update_post_meta(string value, string post_id, string meta_key) 
        {
            bp_data bp_Data = new bp_data();
            bp_Data.cmd="update";
            bp_Data.cmd_string="update wp_postmeta set meta_value ="+value+" where post_id="+post_id+" and meta_key= '"+meta_key+ "'"; 
            return bp_Data;
        }




        public async void Fill_DataGrid2(String data_string, DataGridView dg)
        {
            List<string> id = new List<string>();
            List<string> dt = new List<string>();

            dg.Rows.Clear();
            int row_count = 0;

            string user_id="", sys="", dia="", pul="";


            WordPress_Connect wc =new WordPress_Connect();

            dynamic jsonDe = JsonConvert.DeserializeObject(data_string);
            int i = 0;
            dg.Rows.Clear();

            foreach (var data in jsonDe)
            {
                string s = data.ID;
                id.Add(s);
                string str_date = data.post_date;
                dt.Add(str_date);
            }
            /////////////////////////////////////////////
            ///
            foreach(string str_id in id)
            {
                object obj = Get_Read_obj2(str_id);
                await wc.send(obj);

                jsonDe = JsonConvert.DeserializeObject(wc.str_response);
                i = 0;

                foreach (var data in jsonDe)
                {
                            
                    string k = data.meta_key;
                    string v = data.meta_value;

                    switch(k)
                    {
                        case "user_id":
                            user_id=v;
                        break;
                        ////////////
                        case "sys":
                            sys=v;
                        break;
                        ////////////
                        case "dia":
                            dia=v;
                        break;
                        ////////////
                        case "pul":
                            pul=v;
                        break;
                        ////////////
                        default:
                            break;
                    }
                }

                dg.Rows.Add();
                dg[0, row_count].Value=id[row_count];
                dg[1, row_count].Value= user_id;
                dg[2, row_count].Value=dt[row_count];
                dg[3, row_count].Value=sys;
                dg[4, row_count].Value=dia;
                dg[5, row_count].Value=pul;
                
                row_count++;
            }//foreach
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
