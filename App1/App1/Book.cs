using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace App1
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
    }
}
