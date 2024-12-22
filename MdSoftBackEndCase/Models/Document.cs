namespace MdSoftBackEndCase.Models;
public class Document
{
    public int Id { get; set; } // Belgeye ait ID
    public string FileName { get; set; } // Dosya adı
    public string FileType { get; set; } // Dosya türü (MIME type)
    public long FileSize { get; set; } // Dosya boyutu
    public string UploadedBy { get; set; } // Dosyayı yükleyen kullanıcı
    public DateTime UploadedAt { get; set; } // Dosya yükleme tarihi
    public string FilePath { get; set; } // Dosyanın sunucuda bulunduğu yol
}