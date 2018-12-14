using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Model
{
    public class ToExcelPartModel
    {
        public DateTime StartTime { get; set; }
        public string Line { get; set; }
        public DateTime EndTime { get; set; }
        public string TableNameBarcodeQuery { get; set; }
        public string TableNameBarcodeData { get;set; }
        public string TableNameOffLine { get; set; }
        public string TableNameStationSet { get; set; }
        public string LineName { get; set; }
    }
}