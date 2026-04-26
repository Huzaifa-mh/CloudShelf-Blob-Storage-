using Microsoft.EntityFrameworkCore;
using CloudShelf_Blob_Storage_.Models;

namespace CloudShelf_Blob_Storage_.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options ) : Microsoft.EntityFrameworkCore.DbContext (options)
    {

        public DbSet<Image> Images => Set<Image>();
    }
}
