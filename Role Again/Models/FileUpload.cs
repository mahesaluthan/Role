using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Role_Again.Models
{
    public class FileUpload
    {
        public int ID { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public string Userid { get; set; }
        public bool Approve { get; set; }
    }
}