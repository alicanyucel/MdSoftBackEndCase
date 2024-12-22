namespace MdSoftBackEndCase.Models
{
    public class Document
    {
        public int Id { get; set; }                  
        public string FileName { get; set; }          
        public int FileSize { get; set; }             
        public DateOnly UploadDate { get; set; }     
    }
}
