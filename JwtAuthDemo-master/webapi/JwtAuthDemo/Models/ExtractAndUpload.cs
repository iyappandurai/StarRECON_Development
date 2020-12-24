using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIDemo.Models
{
    public class ExtractAndUpload
    {
        public string FIN_ID { get; set; }
        public string ENTITY_ID { get; set; }
        public string FILENAME { get; set; }
        public DateTime EXTRACTDATE { get; set; }
        public string TOTALCOUNT { get; set; }
        public string SUCCESSCOUNT { get; set; }
        public string REJECTEDCOUNT { get; set; }
        public string STATUS { get; set; }
        public string UPLOAD_FNAME { get; set; }
        public DateTime FILEDATE { get; set; }
        public string FILE_ID { get; set; }
        public string USER_NAME { get; set; }
    }
}
